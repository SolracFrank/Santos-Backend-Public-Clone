using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Addresses;

public record GetAddressByIdQuery(int Id) : IRequest<Result<Address?>>;