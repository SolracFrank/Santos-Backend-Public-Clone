using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using LanguageExt.Common;

namespace Application.Features.Addresses;

public record GetAddresssQuery : IRequest<Result<List<Address>>>;