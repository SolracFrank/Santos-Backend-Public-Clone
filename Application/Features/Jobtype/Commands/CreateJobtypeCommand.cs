using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Jobtypes;

public record CreateJobtypeCommand(int Idjobtype, string Jtname, int? Idjobfk) : IRequest<Result<Jobtype>>;