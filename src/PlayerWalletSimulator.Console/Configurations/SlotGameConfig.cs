namespace PlayerWalletSimulator.Console.Configurations
{
    public class SlotGameConfig
    {
        public decimal MinBet { get; init; }

        public decimal MaxBet { get; init; }

        public double LoseProbability { get; init; }

        public double WinProbability { get; init; }

        public double BigWinProbability { get; init; }

        public int BigWinMinMultiplier { get; set; }

        public int BigWinMaxMultiplier { get; set; }
    }
}
