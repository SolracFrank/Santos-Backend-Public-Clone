using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using LanguageExt.Common;

namespace Application.Features.Customermessages;

public record GetCustomermessagesQuery : IRequest<Result<List<Customermessage>>>;