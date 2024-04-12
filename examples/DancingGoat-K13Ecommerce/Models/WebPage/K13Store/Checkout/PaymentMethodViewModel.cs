using System.ComponentModel.DataAnnotations;

using Kentico.Xperience.K13Ecommerce.StoreApi;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DancingGoat.Models
{
    public class PaymentMethodViewModel
    {
        [Required(ErrorMessage = "Select payment method")]
        [Display(Name = "How would you like to pay?")]
        public int PaymentMethodID { get; set; }


        [BindNever]
        public SelectList PaymentMethods { get; set; }


        public PaymentMethodViewModel()
        {
        }


        public PaymentMethodViewModel(KPaymentOption paymentMethod, SelectList paymentMethods)
        {
            PaymentMethods = paymentMethods;

            if (paymentMethod != null)
            {
                PaymentMethodID = paymentMethod.PaymentOptionId;
            }
        }
    }
}
