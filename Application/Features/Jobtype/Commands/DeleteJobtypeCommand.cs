using MediatR;
using LanguageExt.Common;

namespace Application.Features.Jobtypes;

public record DeleteJobtypeCommand(int Id) : IRequest<Result<Unit>>;