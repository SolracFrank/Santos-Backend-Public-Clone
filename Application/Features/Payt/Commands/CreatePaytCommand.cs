using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Payts;

public record CreatePaytCommand(int Idpayt, string? Ptname) : IRequest<Result<Payt>>;