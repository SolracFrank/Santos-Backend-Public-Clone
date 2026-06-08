using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using LanguageExt.Common;

namespace Application.Features.Jobtypes;

public record GetJobtypesQuery : IRequest<Result<List<Jobtype>>>;