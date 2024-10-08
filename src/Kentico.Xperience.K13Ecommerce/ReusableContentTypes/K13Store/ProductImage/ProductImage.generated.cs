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
using CMS.ContentEngine;

namespace K13Store
{
	/// <summary>
	/// Represents a content item of type <see cref="ProductImage"/>.
	/// </summary>
	[RegisterContentTypeMapping(CONTENT_TYPE_NAME)]
	public partial class ProductImage : IContentItemFieldsSource
	{
		/// <summary>
		/// Code name of the content type.
		/// </summary>
		public const string CONTENT_TYPE_NAME = "K13Store.ProductImage";


		/// <summary>
		/// Represents system properties for a content item.
		/// </summary>
		[SystemField]
		public ContentItemFields SystemFields { get; set; }


		/// <summary>
		/// ProductImageAsset.
		/// </summary>
		public ContentItemAsset ProductImageAsset { get; set; }


		/// <summary>
		/// ProductImageDescription.
		/// </summary>
		public string ProductImageDescription { get; set; }


		/// <summary>
		/// ProductImageOriginalPath.
		/// </summary>
		public string ProductImageOriginalPath { get; set; }
	}
}