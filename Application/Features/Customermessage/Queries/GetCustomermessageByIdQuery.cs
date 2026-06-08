using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Customermessages;

public record GetCustomermessageByIdQuery(int Id) : IRequest<Result<Customermessage?>>;