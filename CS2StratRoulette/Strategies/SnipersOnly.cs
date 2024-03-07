using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public class SnipersOnly : IStrategy
    {
        /// <inheritdoc cref="IStrategy.Name"/>
        public string Name => "Snipers Only";

        /// <inheritdoc cref="IStrategy.Description"/>
        public string Description => "You're only allowed to buy snipers.";

        /// <inheritdoc cref="IStrategy.Running"/>
        public bool Running { get; private set; }

        /// <inheritdoc cref="IStrategy.Start"/>
        public bool Start(ref CS2StratRoulettePlugin plugin)
        {
            if (this.Running)
            {
                return false;
            }
            //TODO: put this in freezetime bool :d
            Server.ExecuteCommand("mp_buy_allow_guns 16");

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
            Server.ExecuteCommand("mp_buy_allow_guns 255");

            this.Running = false;


            return true;
        }

    }
}
