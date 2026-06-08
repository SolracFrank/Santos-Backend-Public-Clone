using MediatR;
using LanguageExt.Common;

namespace Application.Features.Repsales;

public record DeleteRepsaleCommand(int Id) : IRequest<Result<Unit>>;