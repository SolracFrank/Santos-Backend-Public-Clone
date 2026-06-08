using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using LanguageExt.Common;

namespace Application.Features.Taxes;

public record GetTaxsQuery : IRequest<Result<List<Tax>>>;