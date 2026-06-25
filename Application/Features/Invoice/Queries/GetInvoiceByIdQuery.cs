using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Invoices;

public record GetInvoiceByIdQuery(int Id) : IRequest<Result<Domain.Entities.Invoice?>>;