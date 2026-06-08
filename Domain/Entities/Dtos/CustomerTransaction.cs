using System;

namespace Domain.Entities.Dtos;

public class CustomerTransaction
{
    public int? Idestimate { get; set; }
    public string Type { get; set; } = null!;
    public string Num { get; set; } = null!;
    public DateTime Date { get; set; }
    public string Account { get; set; } = null!;
    public decimal Amount { get; set; }
}
