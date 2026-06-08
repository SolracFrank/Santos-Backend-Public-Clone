using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using LanguageExt.Common;

namespace Application.Features.Jobinfos;

public record GetJobinfosQuery : IRequest<Result<List<Jobinfo>>>;