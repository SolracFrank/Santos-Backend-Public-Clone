using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Jobtypes;

public record UpdateJobtypeCommand(int Id, int Idjobtype, string Jtname, int? Idjobfk) : IRequest<Result<Jobtype>>;