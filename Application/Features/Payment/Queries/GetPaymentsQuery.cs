using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using LanguageExt.Common;

namespace Application.Features.Payments;

public record GetPaymentsQuery : IRequest<Result<List<Payment>>>;