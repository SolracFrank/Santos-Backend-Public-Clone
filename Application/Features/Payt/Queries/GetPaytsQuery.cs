using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using LanguageExt.Common;

namespace Application.Features.Payts;

public record GetPaytsQuery : IRequest<Result<List<Payt>>>;