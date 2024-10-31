namespace DancingGoat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // Access configuration and log all settings
            var config = host.Services.GetRequiredService<IConfiguration>();
            LogConfiguration(config);

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseIISIntegration()
                    .UseStartup<Startup>();
                });

        private static void LogConfiguration(IConfiguration config)
        {
            Console.WriteLine("Application Settings:");

            foreach (var kvp in config.AsEnumerable())
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
        }
    }
}
