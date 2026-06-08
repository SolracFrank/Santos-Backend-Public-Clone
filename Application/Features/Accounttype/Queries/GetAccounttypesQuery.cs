using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using LanguageExt.Common;

namespace Application.Features.Accounttypes;

public record GetAccounttypesQuery : IRequest<Result<List<Accounttype>>>;