using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using LanguageExt.Common;

namespace Application.Features.Invoices;

public record GetInvoicesQuery : IRequest<Result<List<Domain.Entities.Invoice>>>;