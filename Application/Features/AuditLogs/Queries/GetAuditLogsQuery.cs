using Domain.Entities.Dtos;
using LanguageExt.Common;
using MediatR;

namespace Application.Features.AuditLogs.Queries
{
    public record GetAuditLogsQuery(int Take = 200)
        : IRequest<Result<List<AuditLogDto>>>;
}