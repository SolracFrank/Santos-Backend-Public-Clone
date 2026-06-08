using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Repsales;

public record UpdateRepsaleCommand(int Id, int Idrepsales, string Rinitials, int? Idemployee, int? Idothern) : IRequest<Result<Repsale>>;