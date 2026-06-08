using MediatR;
using LanguageExt.Common;

namespace Application.Features.Jobinfos;

public record DeleteJobinfoCommand(int Id) : IRequest<Result<Unit>>;