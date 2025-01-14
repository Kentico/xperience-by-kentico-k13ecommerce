using CMS.DataEngine;
using CMS.FormEngine;
using CMS.Integration.K13Ecommerce;
using CMS.Modules;

namespace Kentico.Xperience.K13Ecommerce.Admin;

internal interface IK13EcommerceModuleInstaller
{
    void Install();
}

internal class K13EcommerceModuleInstaller : IK13EcommerceModuleInstaller
{
    private readonly IInfoProvider<ResourceInfo> resourceInfoProvider;
    private readonly IInfoProvider<K13EcommerceSettingsInfo> k13EcommerceSettingsInfoProvider;

    public K13EcommerceModuleInstaller(IInfoProvider<ResourceInfo> resourceInfoProvider, IInfoProvider<K13EcommerceSettingsInfo> k13EcommerceSettingsInfoProvider)
    {
        this.resourceInfoProvider = resourceInfoProvider;
        this.k13EcommerceSettingsInfoProvider = k13EcommerceSettingsInfoProvider;
    }

    public void Install()
    {
        var resourceInfo = InstallModule();
        InstallPagePathMappingRuleInfo(resourceInfo);
        InstallSettingsInfo(resourceInfo);
    }

    private void InstallSettingsInfo(ResourceInfo resourceInfo)
    {
        var info = DataClassInfoProvider.GetDataClassInfo(K13EcommerceSettingsInfo.TYPEINFO.ObjectClassName) ??
            DataClassInfo.New(K13EcommerceSettingsInfo.OBJECT_TYPE);

        info.ClassName = K13EcommerceSettingsInfo.TYPEINFO.ObjectClassName;
        info.ClassTableName = K13EcommerceSettingsInfo.TYPEINFO.ObjectClassName.Replace(".", "_");
        info.ClassDisplayName = "K13Ecommerce Settings info";
        info.ClassResourceID = resourceInfo.ResourceID;
        info.ClassType = ClassType.OTHER;

        var formInfo = FormHelper.GetBasicFormDefinition(nameof(K13EcommerceSettingsInfo.K13EcommerceSettingsID));
        var formItem = new FormFieldInfo
        {
            Name = nameof(K13EcommerceSettingsInfo.K13EcommerceSettingsProductSKUFolderGuid),
            Visible = true,
            DataType = FieldDataType.Guid,
            Caption = K13EcommerceSettingsConstants.SettingsProductSKUFolderGuid,
            Enabled = true,
            AllowEmpty = false,
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(K13EcommerceSettingsInfo.K13EcommerceSettingsProductVariantFolderGuid),
            Visible = true,
            DataType = FieldDataType.Guid,
            Caption = K13EcommerceSettingsConstants.SettingsProductVariantFolderGuid,
            Enabled = true,
            AllowEmpty = false,
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(K13EcommerceSettingsInfo.K13EcommerceSettingsProductImageFolderGuid),
            Visible = true,
            DataType = FieldDataType.Guid,
            Caption = K13EcommerceSettingsConstants.SettingsProductImageFolderGuid,
            Enabled = true,
            AllowEmpty = false,
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(K13EcommerceSettingsInfo.K13EcommerceSettingsGuid),
            Visible = false,
            DataType = FieldDataType.Guid,
            Enabled = true,
            AllowEmpty = false,
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(K13EcommerceSettingsInfo.K13EcommerceSettingsWorkspaceName),
            Visible = true,
            DataType = FieldDataType.Text,
            Enabled = true,
            AllowEmpty = true,
        };
        formInfo.AddFormItem(formItem);

        SetFormDefinition(info, formInfo);

        if (info.HasChanged)
        {
            DataClassInfoProvider.SetDataClassInfo(info);
        }

        var settings = k13EcommerceSettingsInfoProvider.Get().TopN(1).GetEnumerableTypedResult().FirstOrDefault();
        if (settings is null)
        {
            settings = new K13EcommerceSettingsInfo()
            {
                K13EcommerceSettingsProductSKUFolderGuid = Guid.Empty,
                K13EcommerceSettingsProductVariantFolderGuid = Guid.Empty,
                K13EcommerceSettingsProductImageFolderGuid = Guid.Empty
            };
            settings.Insert();
        }
    }

    private static void InstallPagePathMappingRuleInfo(ResourceInfo resourceInfo)
    {
        var info = DataClassInfoProvider.GetDataClassInfo(PagePathMappingRuleInfo.TYPEINFO.ObjectClassName) ??
            DataClassInfo.New(PagePathMappingRuleInfo.OBJECT_TYPE);

        info.ClassName = PagePathMappingRuleInfo.TYPEINFO.ObjectClassName;
        info.ClassTableName = PagePathMappingRuleInfo.TYPEINFO.ObjectClassName.Replace(".", "_");
        info.ClassDisplayName = "Page Path Mapping Rule info";
        info.ClassResourceID = resourceInfo.ResourceID;
        info.ClassType = ClassType.OTHER;
        var formInfo = FormHelper.GetBasicFormDefinition(nameof(PagePathMappingRuleInfo.PagePathMappingRuleID));

        var formItem = new FormFieldInfo
        {
            Name = nameof(PagePathMappingRuleInfo.PagePathMappingRuleK13NodeAliasPath),
            Visible = true,
            DataType = FieldDataType.Text,
            Caption = K13EcommerceTableConstants.K13NodeAliasPathCaption,
            Enabled = true,
            AllowEmpty = false,
            Size = 500
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(PagePathMappingRuleInfo.PagePathMappingRuleXbKPagePath),
            Visible = true,
            DataType = FieldDataType.Text,
            Caption = K13EcommerceTableConstants.XbKPagePathCaption,
            Enabled = true,
            AllowEmpty = false,
            Size = 500,
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(PagePathMappingRuleInfo.PagePathMappingRuleChannelName),
            Visible = false,
            DataType = FieldDataType.Text,
            Caption = K13EcommerceTableConstants.ChannelName,
            Enabled = false,
            AllowEmpty = false,
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(PagePathMappingRuleInfo.PagePathMappingRuleOrder),
            Visible = false,
            DataType = FieldDataType.Integer,
            Caption = K13EcommerceTableConstants.OrderCaption,
            Enabled = false,
            AllowEmpty = false,
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(PagePathMappingRuleInfo.PagePathMappingRuleGuid),
            Visible = false,
            DataType = FieldDataType.Guid,
            Enabled = true,
            AllowEmpty = false,
        };
        formInfo.AddFormItem(formItem);

        SetFormDefinition(info, formInfo);

        if (info.HasChanged)
        {
            DataClassInfoProvider.SetDataClassInfo(info);
        }
    }

    private ResourceInfo InstallModule()
    {
        var resourceInfo = resourceInfoProvider.Get(K13EcommerceResourceConstants.ResourceName)
            ?? new ResourceInfo();

        resourceInfo.ResourceDisplayName = K13EcommerceResourceConstants.ResourceDisplayName;
        resourceInfo.ResourceName = K13EcommerceResourceConstants.ResourceName;
        resourceInfo.ResourceDescription = K13EcommerceResourceConstants.ResourceDescription;
        resourceInfo.ResourceIsInDevelopment = K13EcommerceResourceConstants.ResourceIsInDevelopment;
        if (resourceInfo.HasChanged)
        {
            resourceInfoProvider.Set(resourceInfo);
        }
        return resourceInfo;
    }

    /// <summary>
    /// Ensure that the form is not upserted with any existing form
    /// </summary>
    /// <param name="info"></param>
    /// <param name="form"></param>
    private static void SetFormDefinition(DataClassInfo info, FormInfo form)
    {
        if (info.ClassID > 0)
        {
            var existingForm = new FormInfo(info.ClassFormDefinition);
            existingForm.CombineWithForm(form, new());
            info.ClassFormDefinition = existingForm.GetXmlDefinition();
        }
        else
        {
            info.ClassFormDefinition = form.GetXmlDefinition();
        }
    }
}
