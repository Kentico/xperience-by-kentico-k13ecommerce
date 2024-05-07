using CMS;
using CMS.Base;
using CMS.Core;
using CMS.DataEngine;

using Kentico.Xperience.K13Ecommerce.Synchronization;
using Kentico.Xperience.K13Ecommerce.Synchronization.Products;

[assembly: RegisterModule(typeof(SynchronizationModule))]
namespace Kentico.Xperience.K13Ecommerce.Synchronization;


/// <summary>
/// Module which executes thread worker for product synchronization.
/// </summary>
internal class SynchronizationModule() : Module(nameof(SynchronizationModule))
{
    protected override void OnInit(ModuleInitParameters parameters)
    {
        base.OnInit(parameters);

        RequestEvents.RunEndRequestTasks.Execute += (_, _) => ProductSynchronizationWorker.Current.EnsureRunningThread();
    }
}
