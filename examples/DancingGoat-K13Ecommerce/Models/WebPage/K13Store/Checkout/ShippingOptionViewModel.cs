using System.ComponentModel.DataAnnotations;

using Kentico.Xperience.K13Ecommerce.StoreApi;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DancingGoat.Models
{
    public class ShippingOptionViewModel
    {
        [Display(Name = "Delivery method")]
        public int ShippingOptionID { get; set; }


        [BindNever]
        public SelectList ShippingOptions { get; set; }


        public bool IsVisible { get; set; } = true;


        public ShippingOptionViewModel()
        {
        }


        public ShippingOptionViewModel(KShippingOption shippingOption, SelectList shippingOptions, bool isVisible = true)
        {
            ShippingOptions = shippingOptions;
            ShippingOptionID = shippingOption?.ShippingOptionId ?? 0;
            IsVisible = isVisible;
        }
    }
}
