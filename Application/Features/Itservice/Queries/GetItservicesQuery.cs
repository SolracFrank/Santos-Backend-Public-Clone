using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using LanguageExt.Common;

namespace Application.Features.Itservices;

public record GetItservicesQuery : IRequest<Result<List<Itservice>>>;