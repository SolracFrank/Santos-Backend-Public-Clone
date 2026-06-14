using Domain.Entities.Dtos;
using LanguageExt.Common;
using MediatR;

namespace Application.Features.EmailHistories;

public record GetEmailHistoryByCustomerQuery(int IdUser)
    : IRequest<Result<List<EmailHistoryDto>>>;