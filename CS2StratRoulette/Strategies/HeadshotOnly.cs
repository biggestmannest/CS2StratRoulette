using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public class HeadshotOnly : Strategy
    {
        /// <inheritdoc cref="IStrategy.Name"/>
        public override string Name => "Headshot Only";

        /// <inheritdoc cref="IStrategy.Description"/>
        public override string Description => "You can only kill players with a headshot.";

        /// <inheritdoc cref="IStrategy.Start"/>
        public override bool Start(ref CS2StratRoulettePlugin plugin)
        {
            if (!base.Start(ref plugin))
            {
                return false;
            }

            Server.ExecuteCommand("mp_damage_headshot_only 1");

            return true;
        }

        /// <inheritdoc cref="IStrategy.Stop"/>
        public override bool Stop(ref CS2StratRoulettePlugin plugin)
        {
            if (!base.Stop(ref plugin))
            {
                return false;
            }

            Server.ExecuteCommand("mp_damage_headshot_only 0");

            return true;
        }

    }
}
