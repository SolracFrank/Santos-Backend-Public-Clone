using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Estimate
{
    public int Idestimate { get; set; }

    public int Estimatenumber { get; set; }

    public DateTime Estimatedate { get; set; }

    public DateTime? Expdate { get; set; }

    public DateTime? Datetobecompleted { get; set; }

    public DateTime? Dateordered { get; set; }

    public string? Memo { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Markup { get; set; }

    public decimal Total { get; set; }

    public bool? Activeestimate { get; set; }

    public decimal Openbalance { get; set; }

    public int? Idusers { get; set; }

    public int? Idterms { get; set; }

    public int? Idrepsales { get; set; }

    public int? Idcustomermessage { get; set; }

    public string? Customermessagetext { get; set; }

    public bool IsCopy { get; set; }

    public int? CopiedFromEstimateId { get; set; }

    public virtual Estimate? CopiedFromEstimate { get; set; }

    public DateTime LastModifiedUtc { get; set; }

    public virtual ICollection<Estimatedetail> Estimatedetails { get; set; } = new List<Estimatedetail>();

    public virtual Customermessage? IdcustomermessageNavigation { get; set; }

    public virtual Repsale? IdrepsalesNavigation { get; set; }

    public virtual Term? IdtermsNavigation { get; set; }

    public virtual User? IdusersNavigation { get; set; }

    public virtual ICollection<Estimate> InverseCopiedFromEstimate { get; set; } = new List<Estimate>();

}
