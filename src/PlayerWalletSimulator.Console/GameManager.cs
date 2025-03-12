using PlayerWalletSimulator.Console.Domain;
using PlayerWalletSimulator.Console.Services;
using PlayerWalletSimulator.Console.Shared;

namespace PlayerWalletSimulator.Console
{
    public class GameManager
    {
        private readonly IPlayerWallet _wallet;
        private readonly IGameService _gameService;

        public GameManager(IPlayerWallet playerWallet, IGameService gameService)
        {
            _wallet = playerWallet;
            _gameService = gameService;
        }

        public string HandleDeposit(decimal amount)
        {
            var result = _wallet.Deposit(amount);

            return result.Succeeded ? 
                $"Your deposit of ${amount:F2} was successful. Your current balance is: ${_wallet.Balance:F2}" : 
                result.ErrorMessage;
        }

        public string HandleWithdrawal(decimal amount)
        {
            var result = _wallet.Withdraw(amount);

            return result.Succeeded ? 
                $"Your withdrawal of ${amount:F2} was successful. Your current balance is: ${_wallet.Balance:F2}" : 
                result.ErrorMessage;
        }

        public string HandleBet(decimal betAmount)
        {
            if (betAmount > _wallet.Balance)
            {
                return $"Insufficient funds: your balance is only ${_wallet.Balance:F2}. Please deposit more funds or lower your bet.";
            }

            var gameResult = _gameService.Play(betAmount);
            if (!gameResult.Succeeded)
            {
                return gameResult.ErrorMessage;
            }

            var gameOutcome = gameResult.Data;
            _wallet.RecalculateBalance(betAmount, Math.Round(gameOutcome.Amount, 2));

            return gameOutcome.Type switch
            {
                GameOutcomeType.Loss => $"No luck this time! Your current balance is: ${_wallet.Balance:F2}",
                GameOutcomeType.StandardWin => $"Congrats - you won ${gameOutcome.Amount:F2}! Your current balance is: ${_wallet.Balance:F2}",
                GameOutcomeType.BigWin => $"BIG WIN! You won ${gameOutcome.Amount:F2}! Your current balance is: ${_wallet.Balance:F2}",
                _ => "Unexpected game outcome."
            };
        }
    }
}
