namespace CMS.Integration.K13Ecommerce
{
    public partial class K13EcommerceSettingsInfo
    {
        static K13EcommerceSettingsInfo()
        {
            TYPEINFO.ContinuousIntegrationSettings.Enabled = true;
        }

        /// <summary>
        /// Gets set workspace name or default workspace name (see: https://docs.kentico.com/developers-and-admins/configuration/users/role-management/workspaces#default-workspace)
        /// </summary>
        public string K13EcommerceSettingsEffectiveWorkspaceName
        {
            get
            {
                if (string.IsNullOrEmpty(K13EcommerceSettingsWorkspaceName))
                {
                    return "KenticoDefault";
                }
                return K13EcommerceSettingsWorkspaceName;
            }
        }
    }
}
