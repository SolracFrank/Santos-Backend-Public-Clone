using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Jobinfos;

public record CreateJobinfoCommand(int Idjobinfo, string? Jdescription, string Jjobstatus, int? Idjobtype) : IRequest<Result<Jobinfo>>;