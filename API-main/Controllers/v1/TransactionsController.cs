using Application.Features.Transaction;
using Domain.Entities.Dtos;
using EsApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EsApi.Controllers.v1;

public class TransactionsController : BaseApiController
{
    public TransactionsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("customer/{idUser:int}")]
    [SwaggerOperation(Summary = "Gets transactions by customer.", Description = "Returns transactions associated with the provided user id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns customer transactions.", typeof(IEnumerable<CustomerTransaction>))]
    public async Task<IActionResult> GetByCustomer(int idUser, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetTransactionsByCustomerQuery(idUser), cancellationToken);
    }
}
