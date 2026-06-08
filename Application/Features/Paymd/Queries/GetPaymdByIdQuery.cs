using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Paymds;

public record GetPaymdByIdQuery(int Id) : IRequest<Result<Paymd?>>;