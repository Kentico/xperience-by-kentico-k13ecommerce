using System.ComponentModel.DataAnnotations;

using Kentico.Xperience.K13Ecommerce.Countries;
using Kentico.Xperience.K13Ecommerce.StoreApi;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DancingGoat.Models
{
    public class BillingAddressViewModel
    {
        [Required]
        [Display(Name = "Address line")]
        [MaxLength(100, ErrorMessage = "Maximum allowed length of the input text is {1}")]
        public string BillingAddressLine1 { get; set; }


        [Display(Name = "Address line 2")]
        [MaxLength(100, ErrorMessage = "Maximum allowed length of the input text is {1}")]
        public string BillingAddressLine2 { get; set; }


        [Required]
        [Display(Name = "City")]
        [MaxLength(100, ErrorMessage = "Maximum allowed length of the input text is {1}")]
        public string BillingAddressCity { get; set; }


        [Required]
        [Display(Name = "Postal code")]
        [MaxLength(20, ErrorMessage = "Maximum allowed length of the input text is {1}")]
        public string BillingAddressPostalCode { get; set; }


        public CountryStateViewModel BillingAddressCountryStateSelector { get; set; }


        public AddressSelectorViewModel BillingAddressSelector { get; set; }


        [BindNever] public SelectList Countries { get; set; }


        public string BillingAddressState { get; set; }


        public string BillingAddressCountry { get; set; }


        public BillingAddressViewModel()
        {
        }


        public static async Task<BillingAddressViewModel> GetModel(KAddress address, SelectList countries,
            ICountryService countryRepository, SelectList addresses = null)
        {
            var model = new BillingAddressViewModel();
            if (address != null)
            {
                if (countryRepository == null)
                {
                    throw new ArgumentNullException(nameof(countryRepository));
                }

                model.BillingAddressLine1 = address.AddressLine1;
                model.BillingAddressLine2 = address.AddressLine2;
                model.BillingAddressCity = address.AddressCity;
                model.BillingAddressPostalCode = address.AddressZip;
                model.BillingAddressState =
                    (await countryRepository.GetState(address.AddressStateId))?.StateDisplayName ?? string.Empty;
                model.BillingAddressCountry = (await countryRepository.GetCountry(address.AddressCountryId))?.CountryDisplayName ??
                                        string.Empty;
                model.Countries = countries;
            }

            model.BillingAddressCountryStateSelector = new CountryStateViewModel
            {
                Countries = countries,
                CountryID = address?.AddressCountryId ?? 0,
                StateID = address?.AddressStateId ?? 0
            };

            model.BillingAddressSelector = new AddressSelectorViewModel
            {
                Addresses = addresses,
                AddressID = address?.AddressId ?? 0
            };

            return model;
        }


        public void ApplyTo(KAddress address)
        {
            address.AddressLine1 = BillingAddressLine1;
            address.AddressLine2 = BillingAddressLine2;
            address.AddressCity = BillingAddressCity;
            address.AddressZip = BillingAddressPostalCode;
            address.AddressCountryId = BillingAddressCountryStateSelector.CountryID;
            address.AddressStateId = BillingAddressCountryStateSelector.StateID ?? 0;
        }
    }
}
