using Domain.Entities;
using LanguageExt.Common;
using MediatR;

namespace Application.Features.EmailHistories;

public record CreateEmailHistoryCommand(
    int Idusers,
    string Entitytype,
    string Documentnumber,
    string Emailto,
    string? Subject,
    DateTime Datesent
) : IRequest<Result<EmailHistory>>;