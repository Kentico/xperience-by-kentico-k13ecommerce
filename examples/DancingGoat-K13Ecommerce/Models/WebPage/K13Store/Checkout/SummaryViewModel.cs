namespace DancingGoat.Models
{
    public class SummaryViewModel
    {
        public DeliveryDetailsViewModel DeliveryDetails { get; set; }


        public CartContentViewModel CartModel { get; set; }


        public CustomerViewModel CustomerDetails => DeliveryDetails?.Customer;


        public BillingAddressViewModel BillingAddress => DeliveryDetails?.BillingAddress;


        public PaymentMethodViewModel PaymentMethod { get; set; }


        public string ShippingName { get; set; }


        public string CartPreviousStepUrl { get; set; }
    }
}
