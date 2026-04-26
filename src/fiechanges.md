What I changed
1. New file — src/MailSystem.API/Middlewear/GlobalExceptionMiddleware.cs
A global exception handler that catches any unhandled exception, logs it via Serilog, and returns a standard RFC 7807 application/problem+json response. Maps common exceptions to proper HTTP status codes (UnauthorizedAccessException → 401, KeyNotFoundException → 404, ArgumentException → 400, InvalidOperationException → 409, everything else → 500).

2. Modified — src/MailSystem.API/Program.cs

Added using MailSystem.API.Middlewear; at the top.
Added app.UseMiddleware<GlobalExceptionMiddleware>(); right after app.UseSerilogRequestLogging(); and before app.UseHttpsRedirection();.
No other files touched. No package changes. No DI changes.

Restore prompt (save this — paste it back to me whenever you want to revert)

Revert the GlobalExceptionMiddleware addition from my MailSystem project:

1. Delete the file:
   src/MailSystem.API/Middlewear/GlobalExceptionMiddleware.cs

2. In src/MailSystem.API/Program.cs, remove these two lines:
   - The "using MailSystem.API.Middlewear;" import near the top.
   - The "app.UseMiddleware<GlobalExceptionMiddleware>();" line that sits
     between "app.UseSerilogRequestLogging();" and "app.UseHttpsRedirection();".

Leave everything else in Program.cs and the rest of the project untouched.
