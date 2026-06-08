using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Jobinfos;

public record GetJobinfoByIdQuery(int Id) : IRequest<Result<Jobinfo?>>;