namespace Kentico.Xperience.StoreApi.Orders;

public class KPaymentResult
{
    public string PaymentDate { get; set; }

    public bool PaymentIsCompleted { get; set; }

    public bool PaymentIsFailed { get; set; }

    public bool PaymentIsAuthorized { get; set; }

    public string PaymentDescription { get; set; }

    public string PaymentTransactionId { get; set; }

    public string PaymentAuthorizationID { get; set; }

    public string PaymentMethodName { get; set; }

    public int PaymentMethodID { get; set; }

    public string PaymentStatusValue { get; set; }

    public string PaymentStatusName { get; set; }

    public string PaymentApprovalUrl { get; set; }
}
