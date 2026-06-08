using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Jobtypes;

public record GetJobtypeByIdQuery(int Id) : IRequest<Result<Jobtype?>>;