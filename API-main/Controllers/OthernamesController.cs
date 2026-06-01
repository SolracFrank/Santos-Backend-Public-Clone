using EsApi.Data;
using EsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OthernamesController : ControllerBase
{
    private readonly AppDbContext _context;

    public OthernamesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _context.Othernames.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _context.Othernames.FindAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Othername item)
    {
        _context.Othernames.Add(item);
        await _context.SaveChangesAsync();
        return Ok(item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Othername item)
    {
        if (id != item.Idother) return BadRequest();

        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync();
 
        return Ok(item);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.Othernames.FindAsync(id);
        if (item == null) return NotFound();

        _context.Othernames.Remove(item);
        await _context.SaveChangesAsync();

        return Ok();
    }
}