
using Domain.Entities;
using Infrastructure.Data;
using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.EmailHistories;

public class CreateEmailHistoryCommandHandler
    : IRequestHandler<CreateEmailHistoryCommand, Result<EmailHistory>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateEmailHistoryCommandHandler> _logger;

    public CreateEmailHistoryCommandHandler(
        AppDbContext context,
        ILogger<CreateEmailHistoryCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<EmailHistory>> Handle(
        CreateEmailHistoryCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var email = new EmailHistory
            {
                Idusers = request.Idusers,
                Entitytype = request.Entitytype,
                Documentnumber = request.Documentnumber,
                Emailto = request.Emailto,
                Subject = request.Subject,
                Datesent = request.Datesent
            };

            _context.EmailHistories.Add(email);

            await _context.SaveChangesAsync(cancellationToken);

            return new Result<EmailHistory>(email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating email history.");
            return new Result<EmailHistory>(ex);
        }
    }
}