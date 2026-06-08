using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Othernames;

public record UpdateOthernameCommand(int Id, int Idother, string Oname, string Olastname, string? Oaddress, string? Ocity, string? Ophone, string? Oemail, int? Ozip, int? Idstate) : IRequest<Result<Othername>>;