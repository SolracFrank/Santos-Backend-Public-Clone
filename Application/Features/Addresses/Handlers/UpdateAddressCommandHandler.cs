using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LanguageExt.Common;

namespace Application.Features.Addresses;

public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, Result<Address>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateAddressCommandHandler> _logger;

    public UpdateAddressCommandHandler(AppDbContext context, ILogger<UpdateAddressCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Address>> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(UpdateAddressCommandHandler));

        try
        {
            if (request.Id != request.Idaddress)
                throw new ArgumentException("Id mismatch");

            var entity = new Address
            {
                Idaddress = request.Idaddress,
                Acompany = request.Acompany,
                Afirstname = request.Afirstname,
                Alastname = request.Alastname,
                Ajobtitle = request.Ajobtitle,
                Amainphone = request.Amainphone,
                Aemail = request.Aemail,
                Aother = request.Aother,
                Accemail = request.Accemail,
                Ainvoice = request.Ainvoice,
                Aaddressline = request.Aaddressline,
                Acity = request.Acity,
                Astate = request.Astate,
                Azip = request.Azip,
                Acountry = request.Acountry
            };
            _logger.LogInformation("Updating {EntityName} with id {Id}. Handler: {HandlerName}.", "Addres", request.Id, nameof(UpdateAddressCommandHandler));
            _context.Entry(entity).State = EntityState.Modified;
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(UpdateAddressCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling UpdateAddressCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(UpdateAddressCommandHandler));
                return new Result<Address>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(UpdateAddressCommandHandler));
            return new Result<Address>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(UpdateAddressCommandHandler));
            return new Result<Address>(ex);
        }
    }
}