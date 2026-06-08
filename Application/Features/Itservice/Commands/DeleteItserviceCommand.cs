using MediatR;
using LanguageExt.Common;

namespace Application.Features.Itservices;

public record DeleteItserviceCommand(int Id) : IRequest<Result<Unit>>;