using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LanguageExt.Common;

namespace Application.Features.Itservices;

public class UpdateItserviceCommandHandler : IRequestHandler<UpdateItserviceCommand, Result<Itservice>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateItserviceCommandHandler> _logger;

    public UpdateItserviceCommandHandler(AppDbContext context, ILogger<UpdateItserviceCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Itservice>> Handle(UpdateItserviceCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(UpdateItserviceCommandHandler));

        try
        {
            if (request.Id != request.Iditem)
                throw new ArgumentException("Id mismatch");

            var entity = new Itservice
            {
                Iditem = request.Iditem,
                Iname = request.Iname,
                Idescription = request.Idescription,
                Irate = request.Irate,
                Idescpurchase = request.Idescpurchase,
                Icost = request.Icost,
                Idescsales = request.Idescsales,
                Isales = request.Isales,
                Iditemt = request.Iditemt,
                Idaccountp = request.Idaccountp,
                Idaccounts = request.Idaccounts,
                Idaccountn = request.Idaccountn,
                Idpaymd = request.Idpaymd,
                Iditemfk = request.Iditemfk
            };
            _logger.LogInformation("Updating {EntityName} with id {Id}. Handler: {HandlerName}.", "Itservice", request.Id, nameof(UpdateItserviceCommandHandler));
            _context.Entry(entity).State = EntityState.Modified;
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(UpdateItserviceCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling UpdateItserviceCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(UpdateItserviceCommandHandler));
                return new Result<Itservice>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(UpdateItserviceCommandHandler));
            return new Result<Itservice>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(UpdateItserviceCommandHandler));
            return new Result<Itservice>(ex);
        }
    }
}