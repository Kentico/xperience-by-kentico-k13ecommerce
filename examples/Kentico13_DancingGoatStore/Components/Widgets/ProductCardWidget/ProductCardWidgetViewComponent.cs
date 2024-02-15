using System.Linq;
using DancingGoat.Models;
using DancingGoat.Widgets;

using Kentico.PageBuilder.Web.Mvc;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

[assembly: RegisterWidget(ProductCardWidgetViewComponent.IDENTIFIER, typeof(ProductCardWidgetViewComponent), "Product cards", typeof(ProductCardProperties), Description = "Displays products.", IconClass = "icon-box")]

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Controller for product card widget.
    /// </summary>
    public class ProductCardWidgetViewComponent : ViewComponent
    {
        /// <summary>
        /// Widget identifier.
        /// </summary>
        public const string IDENTIFIER = "DancingGoat.LandingPage.ProductCardWidget";


        private readonly ProductRepository repository;


        /// <summary>
        /// Creates an instance of <see cref="ProductCardWidgetViewComponent"/> class.
        /// </summary>
        /// <param name="repository">Repository for retrieving products.</param>
        public ProductCardWidgetViewComponent(ProductRepository repository)
        {
            this.repository = repository;
        }


        public ViewViewComponentResult Invoke(ProductCardProperties properties)
        {
            var products = repository.GetProducts(properties.SelectedProducts.Select(i => i.NodeGuid).ToList());

            return View("~/Components/Widgets/ProductCardWidget/_ProductCardWidget.cshtml", ProductCardListViewModel.GetViewModel(products));
        }
    }
}