namespace Domain.Common.Audit
{
    public static class AuditActions
    {
        public const string Login = "Login";
        public const string LoginFailed = "LoginFailed";

        public const string CreateUser = "CreateUser";
        public const string UpdateUser = "UpdateUser";
        public const string DisableUser = "DisableUser";
        public const string EnableUser = "EnableUser";
        public const string ChangePassword = "ChangePassword";
        public const string ChangePermissions = "ChangePermissions";

        public const string CreateEstimate = "CreateEstimate";
        public const string UpdateEstimate = "UpdateEstimate";
        public const string DeleteEstimate = "DeleteEstimate";

        public const string CreateInvoice = "CreateInvoice";
        public const string UpdateInvoice = "UpdateInvoice";
        public const string DeleteInvoice = "DeleteInvoice";

        public const string ReceivePayment = "ReceivePayment";
        public const string DeletePayment = "DeletePayment";
    }
}