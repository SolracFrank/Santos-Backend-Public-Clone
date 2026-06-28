using Application.Features.AuditLogs.Mappers;
using Application.Features.AuditLogs.Queries;
using Domain.Common.Audit;
using Domain.Entities.Dtos;
using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.AuditLogs.Handlers
{
    public class GetAuditLogsQueryHandler
        : IRequestHandler<GetAuditLogsQuery, Result<List<AuditLogDto>>>
    {
        private readonly IAuditLogRepository _repository;
        private readonly ILogger<GetAuditLogsQueryHandler> _logger;

        public GetAuditLogsQueryHandler(
            IAuditLogRepository repository,
            ILogger<GetAuditLogsQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result<List<AuditLogDto>>> Handle(
            GetAuditLogsQuery request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Starting handler {HandlerName}.",
                nameof(GetAuditLogsQueryHandler));

            try
            {
                var logs = await _repository.GetRecentAsync(request.Take);

                var result = logs
                    .Select(AuditLogMapper.ToDto)
                    .ToList();

                return new Result<List<AuditLogDto>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while handling {HandlerName}.",
                    nameof(GetAuditLogsQueryHandler));

                return new Result<List<AuditLogDto>>(ex);
            }
        }
    }
}