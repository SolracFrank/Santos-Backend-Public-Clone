using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using LanguageExt.Common;

namespace Application.Features.Estates;

public record GetEstatesQuery : IRequest<Result<List<Estate>>>;