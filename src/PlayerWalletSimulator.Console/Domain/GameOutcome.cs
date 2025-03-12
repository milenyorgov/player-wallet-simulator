namespace PlayerWalletSimulator.Console.Domain
{
    public class GameOutcome
    {
        public GameOutcomeType Type { get; }
        public decimal Amount { get; }

        private GameOutcome(GameOutcomeType type, decimal amount)
        {
            Amount = amount;
            Type = type;
        }


        public static GameOutcome Loss()
        {
            return new GameOutcome(GameOutcomeType.Loss, 0);
        }

        public static GameOutcome StandardWin(decimal winAmount)
        {
            return new GameOutcome(GameOutcomeType.StandardWin, winAmount);
        }

        public static GameOutcome BigWin(decimal winAmount)
        {
            return new GameOutcome(GameOutcomeType.BigWin, winAmount);
        }
    }
}
