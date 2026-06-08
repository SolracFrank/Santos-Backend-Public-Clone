using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Addresses;

public record CreateAddressCommand(int Idaddress, string? Acompany, string? Afirstname, string? Alastname, string? Ajobtitle, string? Amainphone, string? Aemail, string? Aother, string? Accemail, string? Ainvoice) : IRequest<Result<Address>>;