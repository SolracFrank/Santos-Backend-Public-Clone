using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;

namespace Application.Features.Addresses;

public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, Result<Address>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateAddressCommandHandler> _logger;

    public CreateAddressCommandHandler(AppDbContext context, ILogger<CreateAddressCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Address>> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(CreateAddressCommandHandler));

        try
        {
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
            _logger.LogInformation("Creating {EntityName}. Handler: {HandlerName}.", nameof(Address), nameof(CreateAddressCommandHandler));
            _context.Set<Address>().Add(entity);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(CreateAddressCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling CreateAddressCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(CreateAddressCommandHandler));
                return new Result<Address>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(CreateAddressCommandHandler));
            return new Result<Address>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(CreateAddressCommandHandler));
            return new Result<Address>(ex);
        }
    }
}