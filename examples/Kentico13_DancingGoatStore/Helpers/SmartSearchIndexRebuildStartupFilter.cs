using DancingGoat.Infrastructure;

namespace DancingGoat.Helpers
{
    public class SmartSearchIndexRebuildStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                // Ensures smart search index rebuild upon installation or deployment
                builder.UseMiddleware<SmartSearchIndexRebuildMiddleware>();

                next(builder);
            };
        }
    }
}
