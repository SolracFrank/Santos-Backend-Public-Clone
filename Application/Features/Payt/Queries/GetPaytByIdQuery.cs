using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Payts;

public record GetPaytByIdQuery(int Id) : IRequest<Result<Payt?>>;