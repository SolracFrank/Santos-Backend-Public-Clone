using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using LanguageExt.Common;

namespace Application.Features.Repsales;

public record GetRepsalesQuery : IRequest<Result<List<Repsale>>>;