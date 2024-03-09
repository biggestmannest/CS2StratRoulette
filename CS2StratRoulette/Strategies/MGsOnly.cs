using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public class MGsOnly : IStrategy
    {
        /// <inheritdoc cref="IStrategy.Name"/>
        public string Name => "MGs Only";

        /// <inheritdoc cref="IStrategy.Description"/>
        public string Description => "You're only allowed to buy machine guns.";

        /// <inheritdoc cref="IStrategy.Running"/>
        public bool Running { get; private set; }

        /// <inheritdoc cref="IStrategy.Start"/>
        public bool Start(ref CS2StratRoulettePlugin plugin)
        {
            if (this.Running)
            {
                return false;
            }

            Server.ExecuteCommand($"mp_buy_allow_guns {BuyAllow.Heavy.Str()}");

            this.Running = true;

            return true;
        }

        /// <inheritdoc cref="IStrategy.Stop"/>
        public bool Stop(ref CS2StratRoulettePlugin plugin)
        {
            if (!this.Running)
            {
                return false;
            }
            Server.ExecuteCommand($"mp_buy_allow_guns {BuyAllow.All.Str()}");

            this.Running = false;


            return true;
        }

    }
}
