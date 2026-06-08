using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using LanguageExt.Common;

namespace Application.Features.Terms;

public record GetTermsQuery : IRequest<Result<List<Term>>>;