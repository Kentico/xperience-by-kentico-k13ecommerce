using System.Data;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Integration.K13Ecommerce;

[assembly: RegisterObjectType(typeof(K13EcommerceSettingsInfo), K13EcommerceSettingsInfo.OBJECT_TYPE)]

namespace CMS.Integration.K13Ecommerce
{
    /// <summary>
    /// Data container class for <see cref="K13EcommerceSettingsInfo"/>.
    /// </summary>
    public partial class K13EcommerceSettingsInfo : AbstractInfo<K13EcommerceSettingsInfo, IInfoProvider<K13EcommerceSettingsInfo>>, IInfoWithId, IInfoWithGuid
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "kenticok13ecommerce.settings";


        /// <summary>
        /// Type information.
        /// </summary>
        public static readonly ObjectTypeInfo TYPEINFO = new(typeof(IInfoProvider<K13EcommerceSettingsInfo>), OBJECT_TYPE, "KenticoK13ecommerce.Settings", nameof(K13EcommerceSettingsID), null, nameof(K13EcommerceSettingsGuid), null, null, null, null, null)
        {
            TouchCacheDependencies = true,
        };


        /// <summary>
        /// K13 ecommerce settings ID.
        /// </summary>
        [DatabaseField]
        public virtual int K13EcommerceSettingsID
        {
            get => ValidationHelper.GetInteger(GetValue(nameof(K13EcommerceSettingsID)), 0);
            set => SetValue(nameof(K13EcommerceSettingsID), value);
        }

        /// <summary>
        /// K13 ecommerce settings Guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid K13EcommerceSettingsGuid
        {
            get => ValidationHelper.GetGuid(GetValue(nameof(K13EcommerceSettingsGuid)), Guid.Empty);
            set => SetValue(nameof(K13EcommerceSettingsGuid), value);
        }


        /// <summary>
        /// K13 ecommerce settings product SKU folder Guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid K13EcommerceSettingsProductSKUFolderGuid
        {
            get => ValidationHelper.GetGuid(GetValue(nameof(K13EcommerceSettingsProductSKUFolderGuid)), Guid.Empty);
            set => SetValue(nameof(K13EcommerceSettingsProductSKUFolderGuid), value);
        }


        /// <summary>
        /// K13 ecommerce settings product variant folder Guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid K13EcommerceSettingsProductVariantFolderGuid
        {
            get => ValidationHelper.GetGuid(GetValue(nameof(K13EcommerceSettingsProductVariantFolderGuid)), Guid.Empty);
            set => SetValue(nameof(K13EcommerceSettingsProductVariantFolderGuid), value);
        }


        /// <summary>
        /// K13 ecommerce settings product image folder Guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid K13EcommerceSettingsProductImageFolderGuid
        {
            get => ValidationHelper.GetGuid(GetValue(nameof(K13EcommerceSettingsProductImageFolderGuid)), Guid.Empty);
            set => SetValue(nameof(K13EcommerceSettingsProductImageFolderGuid), value);
        }

        /// <summary>
        /// K13 ecommerce settings workspace name.
        /// </summary>
        [DatabaseField]
        public virtual string K13EcommerceSettingsWorkspaceName
        {
            get => ValidationHelper.GetString(GetValue(nameof(K13EcommerceSettingsWorkspaceName)), string.Empty);
            set => SetValue(nameof(K13EcommerceSettingsWorkspaceName), value, string.Empty);
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            Provider.Delete(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            Provider.Set(this);
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="K13EcommerceSettingsInfo"/> class.
        /// </summary>
        public K13EcommerceSettingsInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="K13EcommerceSettingsInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public K13EcommerceSettingsInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}
