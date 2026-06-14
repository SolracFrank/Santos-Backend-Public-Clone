using Domain.Entities.Dtos;
using Infrastructure.Data;
using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.EmailHistories;

public class GetEmailHistoryByCustomerQueryHandler
    : IRequestHandler<GetEmailHistoryByCustomerQuery, Result<List<EmailHistoryDto>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetEmailHistoryByCustomerQueryHandler> _logger;

    public GetEmailHistoryByCustomerQueryHandler(
        AppDbContext context,
        ILogger<GetEmailHistoryByCustomerQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<List<EmailHistoryDto>>> Handle(
        GetEmailHistoryByCustomerQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var emails = await _context.EmailHistories
                .Where(x => x.Idusers == request.IdUser)
                .OrderByDescending(x => x.Datesent)
                .Select(x => new EmailHistoryDto
                {
                    Idemailhistory = x.Idemailhistory,
                    Idusers = x.Idusers,
                    Entitytype = x.Entitytype,
                    Documentnumber = x.Documentnumber,
                    Emailto = x.Emailto,
                    Subject = x.Subject,
                    Datesent = x.Datesent
                })
                .ToListAsync(cancellationToken);

            return new Result<List<EmailHistoryDto>>(emails);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading email history.");

            return new Result<List<EmailHistoryDto>>(ex);
        }
    }
}