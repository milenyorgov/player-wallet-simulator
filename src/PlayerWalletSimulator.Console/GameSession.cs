using PlayerWalletSimulator.Console.Domain;
using PlayerWalletSimulator.Console.Services;
using PlayerWalletSimulator.Console.Shared;

namespace PlayerWalletSimulator.Console
{
    public class GameSession
    {
        private readonly IPlayerWallet _wallet;
        private readonly IGameService _gameService;

        public GameSession(IPlayerWallet playerWallet, IGameService gameService)
        {
            _wallet = playerWallet;
            _gameService = gameService;
        }

        public void Start()
        {
            System.Console.WriteLine("Welcome to the gaming simulator. Allowed commands: deposit [amount], withdraw [amount], bet [amount], or exit.");

            while (true)
            {
                try
                {
                    System.Console.Write("\nPlease, submit action: ");
                    string? input = System.Console.ReadLine()?.Trim().ToLower();

                    if (string.IsNullOrWhiteSpace(input))
                        continue;

                    string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    if (parts[0] == "exit")
                    {
                        System.Console.WriteLine("Thank you for playing! Hope to see you again soon.");
                        break;
                    }
                    else if (parts.Length == 2 && decimal.TryParse(parts[1], out decimal amount))
                    {
                        ProcessAction(parts[0], Math.Round(amount,2));
                    }
                    else
                    {
                        System.Console.WriteLine("Invalid input format. Use: deposit [amount], withdraw [amount], bet [amount], or exit.");
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("An unexpected error occurred. Please try again.");
                }
            }
        }

        private void ProcessAction(string action, decimal amount)
        {
            switch (action)
            {
                case "deposit":
                    HandleDeposit(amount);
                    break;
                case "withdraw":
                    HandleWithdrawal(amount);
                    break;
                case "bet":
                    HandleBet(amount);
                    break;
                default:
                    System.Console.WriteLine("Invalid command. Use deposit, withdraw, bet, or exit.");
                    break;
            }
        }

        private void HandleDeposit(decimal amount)
        {
            var result = _wallet.Deposit(amount);
            PrintResult(result, $"Your deposit of ${amount:F2} was successful. Your current balance is: ${_wallet.Balance:F2}");
        }

        private void HandleWithdrawal(decimal amount)
        {
            var result = _wallet.Withdraw(amount);
            PrintResult(result, $"Your withdrawal of ${amount:F2} was successful. Your current balance is: ${_wallet.Balance:F2}");
        }

        private void HandleBet(decimal betAmount)
        {
            if (betAmount > _wallet.Balance)
            {
                System.Console.WriteLine($"Insufficient funds: your balance is only ${_wallet.Balance:F2}. Please deposit more funds or lower your bet.");
                return;
            }

            var gameResult = _gameService.Play(betAmount);
            if (!gameResult.Succeeded)
            {
                System.Console.WriteLine(gameResult.ErrorMessage);
                return;
            }

            var gameOutcome = gameResult.Data;
            _wallet.RecalculateBalance(betAmount, Math.Round(gameOutcome.Amount, 2));
            PrintGameOutcome(gameOutcome);
        }

        private void PrintResult(Result result, string successMessage)
        {
            System.Console.WriteLine(result.Succeeded ? successMessage : result.ErrorMessage);
        }

        private void PrintGameOutcome(GameOutcome gameOutcome)
        {
            string message = gameOutcome.Type switch
            {
                GameOutcomeType.Loss => $"No luck this time! Your current balance is: ${_wallet.Balance:F2}",
                GameOutcomeType.StandardWin => $"Congrats - you won ${gameOutcome.Amount:F2}! Your current balance is: ${_wallet.Balance:F2}",
                GameOutcomeType.BigWin => $"BIG WIN! You won ${gameOutcome.Amount:F2}! Your current balance is: ${_wallet.Balance:F2}",
                _ => "Unexpected game outcome."
            };

            System.Console.WriteLine(message);
        }
    }
}
