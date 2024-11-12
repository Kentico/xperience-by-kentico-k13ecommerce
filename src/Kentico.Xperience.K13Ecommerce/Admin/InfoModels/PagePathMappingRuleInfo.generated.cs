using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Integration.K13Ecommerce;

[assembly: RegisterObjectType(typeof(PagePathMappingRuleInfo), PagePathMappingRuleInfo.OBJECT_TYPE)]

namespace CMS.Integration.K13Ecommerce
{
    /// <summary>
    /// Data container class for <see cref="PagePathMappingRuleInfo"/>.
    /// </summary>
    [Serializable]
    public partial class PagePathMappingRuleInfo : AbstractInfo<PagePathMappingRuleInfo, IInfoProvider<PagePathMappingRuleInfo>>, IInfoWithId
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "kenticok13ecommerce.pagepathmappingrule";


        /// <summary>
        /// Type information.
        /// </summary>
        public static readonly ObjectTypeInfo TYPEINFO = new(typeof(IInfoProvider<PagePathMappingRuleInfo>), OBJECT_TYPE, "KenticoK13ecommerce.PagePathMappingRule", nameof(PagePathMappingRuleID), null, nameof(PagePathMappingRuleGuid), null, null, null, null, null)
        {
            TouchCacheDependencies = true,
            OrderColumn = nameof(PagePathMappingRuleOrder),
        };


        /// <summary>
        /// PagePathMappingRule ID.
        /// </summary>
        [DatabaseField]
        public virtual int PagePathMappingRuleID
        {
            get => ValidationHelper.GetInteger(GetValue(nameof(PagePathMappingRuleID)), 0);
            set => SetValue(nameof(PagePathMappingRuleID), value);
        }

        /// <summary>
        /// K13 ecommerce settings Guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid PagePathMappingRuleGuid
        {
            get => ValidationHelper.GetGuid(GetValue(nameof(PagePathMappingRuleGuid)), Guid.Empty);
            set => SetValue(nameof(PagePathMappingRuleGuid), value);
        }

        /// <summary>
        /// K13 node alias path.
        /// </summary>
        [DatabaseField]
        public virtual string PagePathMappingRuleK13NodeAliasPath
        {
            get => ValidationHelper.GetString(GetValue(nameof(PagePathMappingRuleK13NodeAliasPath)), string.Empty);
            set => SetValue(nameof(PagePathMappingRuleK13NodeAliasPath), value);
        }


        /// <summary>
        /// XbK page path.
        /// </summary>
        [DatabaseField]
        public virtual string PagePathMappingRuleXbKPagePath
        {
            get => ValidationHelper.GetString(GetValue(nameof(PagePathMappingRuleXbKPagePath)), string.Empty);
            set => SetValue(nameof(PagePathMappingRuleXbKPagePath), value);
        }

        /// <summary>
        /// Channel name.
        /// </summary>
        [DatabaseField]
        public virtual string PagePathMappingRuleChannelName
        {
            get => ValidationHelper.GetString(GetValue(nameof(PagePathMappingRuleChannelName)), string.Empty);
            set => SetValue(nameof(PagePathMappingRuleChannelName), value);
        }

        /// <summary>
        /// Order.
        /// </summary>
        [DatabaseField]
        public virtual int PagePathMappingRuleOrder
        {
            get => ValidationHelper.GetInteger(GetValue(nameof(PagePathMappingRuleOrder)), int.MaxValue);
            set => SetValue(nameof(PagePathMappingRuleOrder), value);
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
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected PagePathMappingRuleInfo(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="PagePathMappingRuleInfo"/> class.
        /// </summary>
        public PagePathMappingRuleInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="PagePathMappingRuleInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public PagePathMappingRuleInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}
