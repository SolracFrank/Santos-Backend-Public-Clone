namespace Domain.Common.Audit
{
    public static class AuditActions
    {
        // Authentication
        public const string Login = "Login";
        public const string LoginFailed = "Login Failed";
        public const string Logout = "Logout";

        // User Management
        public const string UserCreated = "User Created";
        public const string UserUpdated = "User Updated";
        public const string UserPasswordChanged = "User Password Changed";
        public const string UserEnabled = "User Enabled";
        public const string UserDisabled = "User Disabled";

        // Customers
        public const string CustomerCreated = "Customer Created";
        public const string CustomerUpdated = "Customer Updated";
        public const string CustomerHidden = "Customer Hidden";

        // Estimates
        public const string EstimateCreated = "Estimate Created";
        public const string EstimateUpdated = "Estimate Updated";
        public const string EstimateDeleted = "Estimate Deleted";
        public const string EstimateEmailed = "Estimate Emailed";
        public const string EstimatePrinted = "Estimate Printed";
        public const string EstimateConverted = "Estimate Converted";

        // Invoices
        public const string InvoiceCreated = "Invoice Created";
        public const string InvoiceUpdated = "Invoice Updated";
        public const string InvoiceDeleted = "Invoice Deleted";
        public const string InvoiceEmailed = "Invoice Emailed";
        public const string InvoicePrinted = "Invoice Printed";

        // Payments
        public const string PaymentCreated = "Payment Created";
        public const string PaymentDeleted = "Payment Deleted";
        public const string PaymentApplied = "Payment Applied";
    }
}