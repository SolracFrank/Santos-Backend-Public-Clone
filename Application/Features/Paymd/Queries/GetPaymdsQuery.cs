using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using LanguageExt.Common;

namespace Application.Features.Paymds;

public record GetPaymdsQuery : IRequest<Result<List<Paymd>>>;