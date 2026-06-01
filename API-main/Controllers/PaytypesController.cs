using EsApi.Data;
using EsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaytypesController : ControllerBase
{
    private readonly AppDbContext _context;

    public PaytypesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _context.Paytypes.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _context.Paytypes.FindAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Payt item)
    {
        _context.Paytypes.Add(item);
        await _context.SaveChangesAsync();
        return Ok(item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Payt item)
    {
        if (id != item.Idpayt) return BadRequest();

        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync();
 
        return Ok(item);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.Paytypes.FindAsync(id);
        if (item == null) return NotFound();

        _context.Paytypes.Remove(item);
        await _context.SaveChangesAsync();

        return Ok();
    }
}