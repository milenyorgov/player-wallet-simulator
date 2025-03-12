using PlayerWalletSimulator.Console.Shared;

namespace PlayerWalletSimulator.Console.Services
{
    public interface IPlayerWallet
    {
        decimal Balance { get; }
        Result Deposit(decimal amount);
        Result Withdraw(decimal amount);
        void RecalculateBalance(decimal betAmount, decimal winAmount);
    }
}
