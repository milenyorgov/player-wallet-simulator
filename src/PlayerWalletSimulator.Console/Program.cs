using Microsoft.Extensions.Configuration;
using PlayerWalletSimulator.Console.Services;
using PlayerWalletSimulator.Console;
using PlayerWalletSimulator.Console.Domain;
using Microsoft.Extensions.DependencyInjection;
using PlayerWalletSimulator.Console.Configurations;

internal class Program
{
    private const string AllowedCommandsMessage = "Allowed commands: deposit [amount], withdraw [amount], bet [amount], or exit.";
    private static void Main(string[] args)
    {
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
            .AddTransient<GameManager>()
            .Configure<SlotGameConfig>(config.GetSection(nameof(SlotGameConfig)))
            .BuildServiceProvider();

        var gameManager = serviceProvider.GetRequiredService<GameManager>();

        RunGameLoop(gameManager);
    }

    private static void RunGameLoop(GameManager gameManager)
    {
        Console.WriteLine($"Welcome to the gaming simulator. {AllowedCommandsMessage}");

        while (true)
        {
            try
            {
                Console.Write("\nPlease, submit action: ");
                string? input = Console.ReadLine()?.Trim().ToLower();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parts[0] == "exit")
                {
                    Console.WriteLine("Thank you for playing! Hope to see you again soon.");
                    break;
                }

                if (parts.Length != 2 || !decimal.TryParse(parts[1], out decimal amount))
                {
                    Console.WriteLine($"Invalid command. {AllowedCommandsMessage}");
                    continue;
                }

                ProcessUserAction(gameManager, parts[0], amount);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred. Please try again.");
            }
        }

        Console.WriteLine("END");
    }

    private static void ProcessUserAction(GameManager gameManager, string action, decimal amount)
    {
        amount = Math.Round(amount, 2); 

        string response = action switch
        {
            "deposit" => gameManager.HandleDeposit(amount),
            "withdraw" => gameManager.HandleWithdrawal(amount),
            "bet" => gameManager.HandleBet(amount),
            _ => $"Invalid command. {AllowedCommandsMessage}"
        };

        Console.WriteLine(response);
    }
}