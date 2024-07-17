using CMS;
using CMS.Base;
using CMS.Core;
using CMS.DataEngine;

using Kentico.Xperience.K13Ecommerce.Admin;

using Microsoft.Extensions.DependencyInjection;

[assembly: RegisterModule(type: typeof(K13EcommerceModule))]

namespace Kentico.Xperience.K13Ecommerce.Admin;

internal class K13EcommerceModule : Module
{
    private IK13EcommerceModuleInstaller? installer;

    public K13EcommerceModule() : base(nameof(K13EcommerceModule))
    {
    }

    protected override void OnInit(ModuleInitParameters parameters)
    {
        base.OnInit(parameters);

        var services = parameters.Services;
        installer = services.GetRequiredService<IK13EcommerceModuleInstaller>();
        ApplicationEvents.Initialized.Execute += InitializeModule;
    }

    private void InitializeModule(object? sender, EventArgs e) => installer?.Install();
}
