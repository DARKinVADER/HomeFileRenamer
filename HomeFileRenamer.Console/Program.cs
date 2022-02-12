using HomeFileRenamer.Application.Services;
using HomeFileRenamer.Console;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Serilog;

Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Console()
                    .WriteTo.File("logs/HomeFileRenamer.log", rollingInterval: RollingInterval.Day)
                    .CreateLogger();

var fileService = new FileService();
RenamerOptions options = new();

using IHost host = CreateHostBuilder(args).Build();

var renamedFiles = fileService.RenameFiles(options.SourceFilesToRename, options.FoldersWithNames, options.TestRun);

foreach (var file in renamedFiles)
{
    Log.Information(file);
}


IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, configuration) =>
        {
            configuration.Sources.Clear();

            IHostEnvironment env = hostingContext.HostingEnvironment;

            configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configurationRoot = configuration.Build();


            configurationRoot.GetSection(nameof(RenamerOptions)).Bind(options);

            Log.Debug($"SourceFilesToRename: {options.SourceFilesToRename}");
            Log.Debug($"FoldersWithNames: {options.FoldersWithNames}");
            if (options.TestRun)
            {
                Log.Debug($"!!!!!!!!!!!!!!TESTRUN!!!!!!!!!!!!!!!!");
            }
        });
