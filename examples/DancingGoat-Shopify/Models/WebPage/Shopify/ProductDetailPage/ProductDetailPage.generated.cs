//--------------------------------------------------------------------------------------------------
// <auto-generated>
//
//     This code was generated by code generator tool.
//
//     To customize the code use your own partial class. For more info about how to use and customize
//     the generated code see the documentation at https://docs.xperience.io/.
//
// </auto-generated>
//--------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using CMS.Websites;
using Shopify;

namespace DancingGoat.Models
{
	/// <summary>
	/// Represents a page of type <see cref="ProductDetailPage"/>.
	/// </summary>
	public partial class ProductDetailPage : IWebPageFieldsSource
	{
		/// <summary>
		/// Code name of the content type.
		/// </summary>
		public const string CONTENT_TYPE_NAME = "Shopify.ProductDetailPage";


		/// <summary>
		/// Represents system properties for a web page item.
		/// </summary>
		public WebPageFields SystemFields { get; set; }


		/// <summary>
		/// Product.
		/// </summary>
		public IEnumerable<Product> Product { get; set; }
	}
}