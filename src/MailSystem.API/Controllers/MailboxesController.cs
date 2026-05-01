using MailSystem.Application.Abstractions.CurrentUser;
using MailSystem.Domain.Enums;
using MailSystem.Persistence.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MailSystem.API.Controllers;

[ApiController]
[Route("api/mailboxes")]
[Authorize]
public class MailboxesController : ControllerBase
{
    private readonly MailDbContext _dbContext;
    private readonly ICurrentUserService _currentUser;

    public MailboxesController(MailDbContext dbContext, ICurrentUserService currentUser)
    {
        _dbContext = dbContext;
        _currentUser = currentUser;
    }

    [HttpGet("inbox")]
    public Task<IActionResult> GetInbox([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
        => GetMailbox(MailboxType.Inbox, page, pageSize, cancellationToken);

    [HttpGet("sent")]
    public Task<IActionResult> GetSent([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
        => GetMailbox(MailboxType.Sent, page, pageSize, cancellationToken);

    [HttpGet("drafts")]
    public Task<IActionResult> GetDrafts([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
        => GetMailbox(MailboxType.Draft, page, pageSize, cancellationToken);

    [HttpGet("trash")]
    public Task<IActionResult> GetTrash([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
        => GetMailbox(MailboxType.Trash, page, pageSize, cancellationToken);

    [HttpGet("archive")]
    public Task<IActionResult> GetArchive([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
        => GetMailbox(MailboxType.Archive, page, pageSize, cancellationToken);

    private async Task<IActionResult> GetMailbox(
        MailboxType mailboxType,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        if (userId is null)
            return Unauthorized();

        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 20 : Math.Min(pageSize, 100);

        var query = _dbContext.MailboxEntries
            .AsNoTracking()
            .Where(x => x.UserId == userId.Value && x.MailboxType == mailboxType)
            .Include(x => x.Message)
            .ThenInclude(x => x.SenderUser)
            .OrderByDescending(x => x.ReceivedAt);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new
            {
                mailboxEntryId = x.Id,
                messageId = x.MessageId,
                conversationId = x.Message.ConversationId,
                subject = x.Message.Subject,
                preview = x.Message.BodyText != null && x.Message.BodyText.Length > 120
                    ? x.Message.BodyText.Substring(0, 120)
                    : x.Message.BodyText,
                senderName = x.Message.SenderUser.FullName,
                senderEmail = x.Message.SenderUser.Email,
                isRead = x.IsRead,
                isStarred = x.IsStarred,
                receivedAt = x.ReceivedAt
            })
            .ToListAsync(cancellationToken);

        return Ok(new
        {
            items,
            page,
            pageSize,
            totalCount
        });
    }
}