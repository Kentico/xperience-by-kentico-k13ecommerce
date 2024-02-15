using CMS.Ecommerce;

using System.Collections.Generic;
using System.Linq;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// View model for Product card widget.
    /// </summary>
    public class ProductCardListViewModel
    {
        /// <summary>
        /// Collection of products.
        /// </summary>
        public IEnumerable<ProductCardViewModel> Products { get; set; }


        /// <summary>
        /// Gets ViewModels for <paramref name="products"/>.
        /// </summary>
        /// <param name="products">Collection of products.</param>
        /// <returns>Hydrated ViewModel.</returns>
        public static ProductCardListViewModel GetViewModel(IEnumerable<SKUTreeNode> products)
        {
            return new ProductCardListViewModel
            {
                Products = products.Select(i => ProductCardViewModel.GetViewModel(i)).Where(i => i != null)
            };
        }
    }
}