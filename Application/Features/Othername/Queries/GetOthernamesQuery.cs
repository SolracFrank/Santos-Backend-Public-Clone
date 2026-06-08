using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using LanguageExt.Common;

namespace Application.Features.Othernames;

public record GetOthernamesQuery : IRequest<Result<List<Othername>>>;