using PlayerWalletSimulator.Console.Domain;
using PlayerWalletSimulator.Console.Shared;

namespace PlayerWalletSimulator.Console.Services
{
    public interface IGameService
    {
        Result<GameOutcome> Play(decimal betAmount);
    }
}
