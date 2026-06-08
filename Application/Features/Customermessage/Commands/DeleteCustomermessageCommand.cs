using MediatR;
using LanguageExt.Common;

namespace Application.Features.Customermessages;

public record DeleteCustomermessageCommand(int Id) : IRequest<Result<Unit>>;