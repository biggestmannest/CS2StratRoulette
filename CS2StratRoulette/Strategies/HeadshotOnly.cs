using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public class HeadshotOnly : IStrategy
    {
        /// <inheritdoc cref="IStrategy.Name"/>
        public string Name => "Headshot Only";

        /// <inheritdoc cref="IStrategy.Description"/>
        public string Description => "You can only kill players with a headshot.";

        /// <inheritdoc cref="IStrategy.Running"/>
        public bool Running { get; private set; }

        /// <inheritdoc cref="IStrategy.Start"/>
        public bool Start(ref CS2StratRoulettePlugin plugin)
        {
            if (this.Running)
            {
                return false;
            }

            Server.ExecuteCommand("mp_damage_headshot_only 1");


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

            Server.ExecuteCommand("mp_damage_headshot_only 0");

            this.Running = false;


            return true;
        }

    }
}
