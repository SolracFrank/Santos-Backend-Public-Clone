using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;
using Domain.Common.Security;
using Domain.Exceptions;

namespace Application.Features.Itservices;

public class CreateItserviceCommandHandler : IRequestHandler<CreateItserviceCommand, Result<Itservice>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateItserviceCommandHandler> _logger;
    private readonly IPermissionService _permissionService;
    private readonly ICurrentUserService _currentUserService;

    public CreateItserviceCommandHandler(
     AppDbContext context,
     ILogger<CreateItserviceCommandHandler> logger,
     IPermissionService permissionService,
     ICurrentUserService currentUserService)
    {
        _context = context;
        _logger = logger;
        _permissionService = permissionService;
        _currentUserService = currentUserService;
    }

    public async Task<Result<Itservice>> Handle(CreateItserviceCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(CreateItserviceCommandHandler));

        try
        {
            var canManageItems = await _permissionService.CanManageItemsAsync(
            _currentUserService.IdLoginUser);

            if (!canManageItems)
            {
                throw new ForbiddenException(
                    "You do not have permission to create items.",
                    "ITEM_CREATE_FORBIDDEN");
            }

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
                Iditemfk = request.Iditemfk,
                UseNegativeAmount = request.UseNegativeAmount
            };
            _logger.LogInformation("Creating {EntityName}. Handler: {HandlerName}.", nameof(Itservice), nameof(CreateItserviceCommandHandler));
            _context.Set<Itservice>().Add(entity);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(CreateItserviceCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling CreateItserviceCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(CreateItserviceCommandHandler));
                return new Result<Itservice>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(CreateItserviceCommandHandler));
            return new Result<Itservice>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(CreateItserviceCommandHandler));
            return new Result<Itservice>(ex);
        }
    }
}