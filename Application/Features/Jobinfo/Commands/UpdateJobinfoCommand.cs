using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Jobinfos;

public record UpdateJobinfoCommand(int Id, int Idjobinfo, string? Jdescription, string Jjobstatus, int? Idjobtype) : IRequest<Result<Jobinfo>>;