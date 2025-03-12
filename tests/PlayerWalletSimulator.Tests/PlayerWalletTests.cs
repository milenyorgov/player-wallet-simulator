using FluentAssertions;
using PlayerWalletSimulator.Console.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerWalletSimulator.Tests
{
    public class PlayerWalletTests
    {
        private readonly PlayerWallet _wallet;

        public PlayerWalletTests()
        {
            _wallet = new PlayerWallet();
        }

        [Fact]
        public void PlayerWallet_ShouldInitializeWithZeroBalance()
        {
            // Assert
            _wallet.Balance.Should().Be(0);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(50.75)]
        [InlineData(100)]
        public void Deposit_ShouldIncreaseBalance_WhenAmountIsPositive(decimal amount)
        {
            // Act
            var result = _wallet.Deposit(amount);

            // Assert
            result.Succeeded.Should().BeTrue();
            _wallet.Balance.Should().Be(amount);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void Deposit_ShouldFail_WhenAmountIsNonPositive(decimal amount)
        {
            // Act
            var result = _wallet.Deposit(amount);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Contain("Deposit failed.");
            _wallet.Balance.Should().Be(0);
        }

        [Theory]
        [InlineData(10, 5)]
        [InlineData(50.75, 25.50)]
        [InlineData(100, 99.99)]
        public void Withdraw_ShouldDecreaseBalance_WhenFundsAreSufficient(decimal depositAmount, decimal withdrawAmount)
        {
            // Arrange
            _wallet.Deposit(depositAmount);

            // Act
            var result = _wallet.Withdraw(withdrawAmount);

            // Assert
            result.Succeeded.Should().BeTrue();
            _wallet.Balance.Should().Be(depositAmount - withdrawAmount);
        }

        [Theory]
        [InlineData(10, 15)]
        [InlineData(100, 101)]
        [InlineData(10, 0)]
        [InlineData(50, -5)]
        public void Withdraw_ShouldFail_WhenFundsAreInsufficientOrNotPositive(decimal depositAmount, decimal withdrawAmount)
        {
            // Arrange
            _wallet.Deposit(depositAmount);

            // Act
            var result = _wallet.Withdraw(withdrawAmount);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Contain("Withdrawal failed.");
            _wallet.Balance.Should().Be(depositAmount);
        }


        [Theory]
        [InlineData(50, 10, 20)]
        [InlineData(100, 50, 0)]
        [InlineData(200, 100, 200)]
        public void RecalculateBalance_ShouldUpdateBalanceCorrectly(decimal initialBalance, decimal betAmount, decimal winAmount)
        {
            // Arrange
            _wallet.Deposit(initialBalance);

            // Act
            _wallet.RecalculateBalance(betAmount, winAmount);

            // Assert
            _wallet.Balance.Should().Be(initialBalance - betAmount + winAmount);
        }
    }
}
