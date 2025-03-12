using NSubstitute;
using PlayerWalletSimulator.Console.Services;
using FluentAssertions;
using PlayerWalletSimulator.Console.Shared;
using PlayerWalletSimulator.Console.Domain;
using PlayerWalletSimulator.Console;

namespace PlayerWalletSimulator.Tests
{
    public class GameManagerTests
    {
        private readonly IPlayerWallet _wallet;
        private readonly IGameService _gameService;
        private readonly GameManager _gameManager;

        public GameManagerTests()
        {
            _wallet = Substitute.For<IPlayerWallet>();
            _gameService = Substitute.For<IGameService>();
            _gameManager = new GameManager(_wallet, _gameService);
        }

        [Fact]
        public void HandleDeposit_ShouldIncreaseBalance_WhenAmountIsValid()
        {
            // Arrange
            _wallet.Deposit(10m).Returns(Result.Success);
            _wallet.Balance.Returns(10m);

            // Act
            var result = _gameManager.HandleDeposit(10m);

            // Assert
            _wallet.Received(1).Deposit(10m);
            result.Should().Contain("Your deposit of $10.00 was successful");
        }

        [Fact]
        public void HandleDeposit_ShouldFail_WhenAmountIsInvalid()
        {
            // Arrange
            _wallet.Deposit(-5m).Returns(Result.Failure("Deposit failed. Please enter a positive amount."));

            // Act
            var result = _gameManager.HandleDeposit(-5m);

            // Assert
            result.Should().Contain("Deposit failed. Please enter a positive amount.");
        }

        [Fact]
        public void HandleWithdrawal_ShouldDecreaseBalance_WhenFundsAreSufficient()
        {
            // Arrange
            _wallet.Withdraw(5m).Returns(Result.Success);
            _wallet.Balance.Returns(5m);

            // Act
            var result = _gameManager.HandleWithdrawal(5m);

            // Assert
            _wallet.Received(1).Withdraw(5m);
            result.Should().Contain("Your withdrawal of $5.00 was successful");
        }

        [Fact]
        public void HandleWithdrawal_ShouldFail_WhenFundsAreInsufficient()
        {
            // Arrange
            _wallet.Withdraw(20m).Returns(Result.Failure("Insufficient funds."));
            _wallet.Balance.Returns(10m);

            // Act
            var result = _gameManager.HandleWithdrawal(20m);

            // Assert
            result.Should().Contain("Insufficient funds.");
        }

        [Fact]
        public void HandleBet_ShouldDeductBalance_AndReturnWinMessage()
        {
            // Arrange
            var gameOutcome = GameOutcome.StandardWin(15m);
            _wallet.Balance.Returns(20m);
            _gameService.Play(10m).Returns(Result<GameOutcome>.SuccessWith(gameOutcome));

            // Act
            var result = _gameManager.HandleBet(10m);

            // Assert
            _wallet.Received(1).RecalculateBalance(10m, 15m);
            result.Should().Contain("Congrats - you won $15.00!");
        }

        [Fact]
        public void HandleBet_ShouldFail_WhenInsufficientFunds()
        {
            // Arrange
            _wallet.Balance.Returns(5m);

            // Act
            var result = _gameManager.HandleBet(10m);

            // Assert
            result.Should().Contain("Insufficient funds: your balance is only $5.00.");
        }

        [Fact]
        public void HandleBet_ShouldFail_WhenGameFails()
        {
            // Arrange
            _wallet.Balance.Returns(20m);
            _gameService.Play(10m).Returns(Result<GameOutcome>.Failure("Invalid bet"));

            // Act
            var result = _gameManager.HandleBet(10m);

            // Assert
            result.Should().Contain("Invalid bet");
        }
    }
}
