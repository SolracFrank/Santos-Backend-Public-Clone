using Application.Features.TaxRates;
using Application.Features.TaxRates.Commands;
using Application.Features.TaxRates.Queries;
using Domain.Entities;
using EsApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EsApi.Controllers.v1;

public class TaxRatesController : BaseApiController
{
    public TaxRatesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return await SendAsync(new GetTaxRatesQuery(), cancellationToken);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateTaxRateCommand command,
        CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
    int id,
    [FromBody] UpdateTaxRateCommand command,
    CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest();

        return await SendAsync(command, cancellationToken);
    }
}