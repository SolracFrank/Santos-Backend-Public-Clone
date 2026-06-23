using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.ConfigurationModels;

public class MicrosoftGraphEmailConfiguration
{
    public string TenantId { get; set; } = "";
    public string ClientId { get; set; } = "";
    public string ClientSecret { get; set; } = "";
    public string FromEmail { get; set; } = "";
    public string FromName { get; set; } = "";
}