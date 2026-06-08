using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Invoices;

public record UpdateInvoiceCommand(int Id, int Idinvoice, int Invoicenumber, DateTime Invoicedate, DateTime? Duedate, DateTime? Dteordered, string? Ponumber, string? Memo, decimal Subtotal, decimal Markup, decimal Total, decimal Ppenbalance, bool? Activeinvoice, int? Idusers, int? Idterms, int? Idrepsales, int? Idcustomermessage, string? Customermessagetext, int? Createdfromestimateid, DateTime? Dateordered, decimal Openbalance) : IRequest<Result<Invoice>>;