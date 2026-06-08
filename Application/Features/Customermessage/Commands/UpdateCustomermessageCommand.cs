using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Customermessages;

public record UpdateCustomermessageCommand(int Id, int Idcustomermessage, string Message) : IRequest<Result<Customermessage>>;