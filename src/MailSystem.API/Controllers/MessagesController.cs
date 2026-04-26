using MailSystem.API.DTOs.Messages;
using MailSystem.Application.Abstractions.CurrentUser;
using MailSystem.Domain.Entities;
using MailSystem.Domain.Enums;
using MailSystem.Persistence.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MailSystem.API.Controllers;

[ApiController]
[Route("api/messages")]
[Authorize]
public class MessagesController : ControllerBase
{
    private readonly MailDbContext _dbContext;
    private readonly ICurrentUserService _currentUser;

    public MessagesController(
        MailDbContext dbContext,
        ICurrentUserService currentUser)
    {
        _dbContext = dbContext;
        _currentUser = currentUser;
    }

    [HttpPost("draft")]
    public async Task<IActionResult> SaveDraft(
        SaveDraftRequest request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        if (userId is null)
            return Unauthorized();

        var allRecipientIds = request.ToUserIds
            .Concat(request.CcUserIds)
            .Concat(request.BccUserIds)
            .Distinct()
            .ToList();

        if (allRecipientIds.Any())
        {
            var validRecipientCount = await _dbContext.Users
                .CountAsync(x => allRecipientIds.Contains(x.Id), cancellationToken);

            if (validRecipientCount != allRecipientIds.Count)
            {
                return BadRequest(new { message = "One or more recipients are invalid." });
            }
        }

        var now = DateTime.UtcNow;

        var conversation = new Conversation
        {
            Id = Guid.NewGuid(),
            Subject = request.Subject.Trim(),
            CreatedByUserId = userId.Value,
            CreatedAt = now,
            LastMessageAt = now
        };

        var message = new Message
        {
            Id = Guid.NewGuid(),
            ConversationId = conversation.Id,
            SenderUserId = userId.Value,
            Subject = request.Subject.Trim(),
            BodyText = request.BodyText,
            BodyHtml = request.BodyHtml,
            IsDraft = true,
            CreatedAt = now
        };

        var recipients = BuildRecipients(
            message.Id,
            request.ToUserIds,
            request.CcUserIds,
            request.BccUserIds,
            now);

        var draftEntry = new MailboxEntry
        {
            Id = Guid.NewGuid(),
            MessageId = message.Id,
            UserId = userId.Value,
            MailboxType = MailboxType.Draft,
            IsRead = true,
            ReceivedAt = now,
            CreatedAt = now
        };

        _dbContext.Conversations.Add(conversation);
        _dbContext.Messages.Add(message);
        _dbContext.MessageRecipients.AddRange(recipients);
        _dbContext.MailboxEntries.Add(draftEntry);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok(new
        {
            messageId = message.Id,
            conversationId = conversation.Id,
            status = "DraftSaved"
        });
    }

    [HttpPut("draft/{messageId:guid}")]
    public async Task<IActionResult> UpdateDraft(
        Guid messageId,
        UpdateDraftRequest request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        if (userId is null)
            return Unauthorized();

        var draft = await _dbContext.Messages
            .Include(x => x.Recipients)
            .Include(x => x.MailboxEntries)
            .FirstOrDefaultAsync(x => x.Id == messageId, cancellationToken);

        if (draft is null || !draft.IsDraft)
            return NotFound(new { message = "Draft not found." });

        if (draft.SenderUserId != userId.Value)
            return Forbid();

        var draftEntry = draft.MailboxEntries
            .FirstOrDefault(x => x.UserId == userId.Value && x.MailboxType == MailboxType.Draft);

        if (draftEntry is null)
            return BadRequest(new { message = "Draft mailbox entry not found." });

        var allRecipientIds = request.ToUserIds
            .Concat(request.CcUserIds)
            .Concat(request.BccUserIds)
            .Distinct()
            .ToList();

        if (allRecipientIds.Any())
        {
            var validRecipientCount = await _dbContext.Users
                .CountAsync(x => allRecipientIds.Contains(x.Id), cancellationToken);

            if (validRecipientCount != allRecipientIds.Count)
            {
                return BadRequest(new { message = "One or more recipients are invalid." });
            }
        }

        var now = DateTime.UtcNow;

        draft.Subject = request.Subject.Trim();
        draft.BodyText = request.BodyText;
        draft.BodyHtml = request.BodyHtml;
        draft.UpdatedAt = now;

        var conversation = await _dbContext.Conversations
            .FirstAsync(x => x.Id == draft.ConversationId, cancellationToken);

        conversation.Subject = request.Subject.Trim();
        conversation.UpdatedAt = now;

        _dbContext.MessageRecipients.RemoveRange(draft.Recipients);

        var newRecipients = BuildRecipients(
            draft.Id,
            request.ToUserIds,
            request.CcUserIds,
            request.BccUserIds,
            now);

        _dbContext.MessageRecipients.AddRange(newRecipients);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok(new { success = true });
    }

    private static List<MessageRecipient> BuildRecipients(
        Guid messageId,
        List<Guid> toUserIds,
        List<Guid> ccUserIds,
        List<Guid> bccUserIds,
        DateTime now)
    {
        var recipients = new List<MessageRecipient>();

        recipients.AddRange(toUserIds.Distinct().Select(id => new MessageRecipient
        {
            Id = Guid.NewGuid(),
            MessageId = messageId,
            RecipientUserId = id,
            RecipientType = RecipientType.To,
            CreatedAt = now
        }));

        recipients.AddRange(ccUserIds.Distinct().Select(id => new MessageRecipient
        {
            Id = Guid.NewGuid(),
            MessageId = messageId,
            RecipientUserId = id,
            RecipientType = RecipientType.Cc,
            CreatedAt = now
        }));

        recipients.AddRange(bccUserIds.Distinct().Select(id => new MessageRecipient
        {
            Id = Guid.NewGuid(),
            MessageId = messageId,
            RecipientUserId = id,
            RecipientType = RecipientType.Bcc,
            CreatedAt = now
        }));

        return recipients;
    }
}