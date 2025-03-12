using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using PlayerWalletSimulator.Console.Configurations;
using PlayerWalletSimulator.Console.Domain;

namespace PlayerWalletSimulator.Tests
{
    public class SlotGameTests
    {
        private readonly SlotGame _slotGame;
        private readonly SlotGameConfig _config;
        private readonly Random _random;

        public SlotGameTests()
        {
            _config = new SlotGameConfig
            {
                MinBet = 1.0m,
                MaxBet = 10.0m,
                LoseProbability = 0.50,  
                WinProbability = 0.40,  
                BigWinProbability = 0.10,
                BigWinMinMultiplier = 2,
                BigWinMaxMultiplier = 10
            };

            var optionsMock = Substitute.For<IOptions<SlotGameConfig>>();
            optionsMock.Value.Returns(_config);

            _random = new Random();

            _slotGame = new SlotGame(optionsMock, _random);
        }

        [Theory]
        [InlineData(0.5)]
        [InlineData(11.0)]
        public void Play_ShouldReturnError_WhenBetIsOutOfRange(decimal betAmount)
        {
            // Act
            var result = _slotGame.Play(betAmount);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Contain("Invalid bet amount");
        }


        [Theory]
        [InlineData(2)] 
        [InlineData(5.5)]
        public void Play_ShouldReturnSucceeded_WhenBetIsInTheRange(decimal betAmount)
        {
            // Act
            var result = _slotGame.Play(betAmount);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.ErrorMessage.Should().BeEmpty();
        }

        [Fact]
        public void Play_ShouldFollowProbabilityDistribution()
        {
            int totalGames = 100000;
            int losses = 0;
            int standardWins = 0;
            int bigWins = 0;

            for (int i = 0; i < totalGames; i++)
            {
                _random.NextDouble();

                var result = _slotGame.Play(5);

                if (result.Data.Type == GameOutcomeType.Loss)
                    losses++;
                else if (result.Data.Type == GameOutcomeType.StandardWin)
                    standardWins++;
                else if (result.Data.Type == GameOutcomeType.BigWin)
                    bigWins++;
            }

            double lossPercentage = (double)losses / totalGames;
            double standardWinPercentage = (double)standardWins / totalGames;
            double bigWinPercentage = (double)bigWins / totalGames;

            lossPercentage.Should().BeInRange(_config.LoseProbability - 0.01d, _config.LoseProbability + 0.01d);   // Allow 1% margin
            standardWinPercentage.Should().BeInRange(_config.WinProbability - 0.01d, _config.WinProbability + 0.01d);  // Allow 1% margin
            bigWinPercentage.Should().BeInRange(_config.BigWinProbability - 0.01d, _config.BigWinProbability + 0.01d);  // Allow 1% margin
        }
    }
}
