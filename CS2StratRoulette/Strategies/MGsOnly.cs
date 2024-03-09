using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public class MGsOnly : Strategy
    {
        /// <inheritdoc cref="IStrategy.Name"/>
        public override string Name => "MGs Only";

        /// <inheritdoc cref="IStrategy.Description"/>
        public override string Description => "You're only allowed to buy machine guns.";

        /// <inheritdoc cref="IStrategy.Start"/>
        public override bool Start(ref CS2StratRoulettePlugin plugin)
        {
            if (!base.Start(ref plugin))
            {
                return false;
            }

            Server.ExecuteCommand($"mp_buy_allow_guns {BuyAllow.Heavy.Str()}");

            return true;
        }

        /// <inheritdoc cref="IStrategy.Stop"/>
        public override bool Stop(ref CS2StratRoulettePlugin plugin)
        {
            if (!base.Start(ref plugin))
            {
                return false;
            }

            Server.ExecuteCommand($"mp_buy_allow_guns {BuyAllow.All.Str()}");

            return true;
        }

    }
}
