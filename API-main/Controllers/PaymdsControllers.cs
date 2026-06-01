using EsApi.Data;
using EsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymdsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PaymdsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _context.Paymds.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _context.Paymds.FindAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Paymd item)
    {
        _context.Paymds.Add(item);
        await _context.SaveChangesAsync();
        return Ok(item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Paymd item)
    {
        if (id != item.Idpaymd) return BadRequest();

        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync();
 
        return Ok(item);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.Paymds.FindAsync(id);
        if (item == null) return NotFound();

        _context.Paymds.Remove(item);
        await _context.SaveChangesAsync();

        return Ok();
    }
}