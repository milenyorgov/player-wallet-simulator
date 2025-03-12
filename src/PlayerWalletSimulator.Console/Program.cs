using Microsoft.Extensions.Configuration;
using PlayerWalletSimulator.Console.Services;
using PlayerWalletSimulator.Console;
using PlayerWalletSimulator.Console.Domain;
using Microsoft.Extensions.DependencyInjection;
using PlayerWalletSimulator.Console.Configurations;


var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddCommandLine(args)
    .AddEnvironmentVariables()
    .Build();

var serviceProvider = new ServiceCollection()
    .AddSingleton<IPlayerWallet, PlayerWallet>()
    .AddSingleton<Random>()
    .AddSingleton<IGameService, SlotGame>()
    .AddTransient<GameSession>()
    .Configure<SlotGameConfig>(config.GetSection(nameof(SlotGameConfig))) 
    .BuildServiceProvider();

var gameSession = serviceProvider.GetRequiredService<GameSession>();

gameSession.Start();

Console.WriteLine("END");