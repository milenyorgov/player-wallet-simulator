using PlayerWalletSimulator.Console.Services;
using PlayerWalletSimulator.Console.Shared;

namespace PlayerWalletSimulator.Console.Domain
{
    public class PlayerWallet : IPlayerWallet
    {
        public decimal Balance { get; private set; }

        public PlayerWallet()
        {
            Balance = 0;
        }

        public Result Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                return "Deposit failed. Please enter a positive amount.";
            }

            Balance += amount;

            return true;
        }

        public Result Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                return "Withdrawal failed. Please enter a positive amount.";
            }

            if (amount > Balance)
            {
                return $"Withdrawal failed. You attempted to withdraw ${amount}, but your balance is only ${Balance}.";
            }

            Balance -= amount;

            return true;
        }

        public void RecalculateBalance(decimal betAmount, decimal winAmount)
        {
            Balance = Balance - betAmount + winAmount;
        }
    }
}
