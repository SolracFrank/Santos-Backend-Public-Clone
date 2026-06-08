using MediatR;
using LanguageExt.Common;

namespace Application.Features.Estates;

public record DeleteEstateCommand(int Id) : IRequest<Result<Unit>>;