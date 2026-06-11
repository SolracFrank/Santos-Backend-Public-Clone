namespace Domain.Entities;

public partial class TaxRate
{
    public int Idtaxrate { get; set; }

    public string Name { get; set; } = null!;

    public decimal Rate { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}