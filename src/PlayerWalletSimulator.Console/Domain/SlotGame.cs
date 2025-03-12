using Microsoft.Extensions.Options;
using PlayerWalletSimulator.Console.Configurations;
using PlayerWalletSimulator.Console.Services;
using PlayerWalletSimulator.Console.Shared;

namespace PlayerWalletSimulator.Console.Domain
{
    public class SlotGame : IGameService
    {
        private readonly Random _random;
        private readonly SlotGameConfig _config;

        public SlotGame(IOptions<SlotGameConfig> config, Random random)
        {
            _random = random;
            _config = config.Value;
        }

        public Result<GameOutcome> Play(decimal betAmount)
        {
            if (betAmount < _config.MinBet || betAmount > _config.MaxBet)
            {
                return $"Invalid bet amount: ${betAmount}. Please place a bet between ${_config.MinBet} and ${_config.MaxBet}.";
            }

            decimal winAmount = 0;
            double outcome = _random.NextDouble();

            if (outcome < _config.LoseProbability)
            {
                return GameOutcome.Loss();
            }
            else if (outcome < _config.LoseProbability + _config.WinProbability)
            {
                decimal multiplier = (decimal)(1.0 + _random.NextDouble());
                winAmount = betAmount * multiplier;
                return GameOutcome.StandardWin(winAmount);
            }
            else
            {
                decimal multiplier = (decimal)(_config.BigWinMinMultiplier + _random.NextDouble() * (_config.BigWinMaxMultiplier - _config.BigWinMinMultiplier));
                winAmount = betAmount * multiplier;
                return GameOutcome.BigWin(winAmount);
            }
        }
    }
}
