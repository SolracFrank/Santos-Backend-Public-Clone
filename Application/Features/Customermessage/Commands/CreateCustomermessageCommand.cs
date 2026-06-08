using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Customermessages;

public record CreateCustomermessageCommand(int Idcustomermessage, string Message) : IRequest<Result<Customermessage>>;