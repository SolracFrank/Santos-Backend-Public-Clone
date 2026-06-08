using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using LanguageExt.Common;

namespace Application.Features.Itemtypes;

public record GetItemtypesQuery : IRequest<Result<List<Itemtype>>>;