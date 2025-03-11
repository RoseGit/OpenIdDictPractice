using Microsoft.AspNetCore.Hosting;

namespace OpenIdDictPractice
{
    /// <summary>
    /// Primary class
    /// </summary>
    public class Program
    {
        /// <summary>
        /// main program entrance
        /// </summary>
        /// <param name="args">Program Arguments.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Create default configuration
        /// </summary>
        /// <param name="args">Program Arguments.</param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}