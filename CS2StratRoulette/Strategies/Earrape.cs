using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public class Earrape : IStrategy
    {
        /// <inheritdoc cref="IStrategy.Name"/>
        public string Name => "Earrape";
        private readonly System.Random random = new();

        /// <inheritdoc cref="IStrategy.Description"/>
        public string Description => "Everyone gets a Negev and decoy grenades, succesfully ruining the enemies ears.";

        /// <inheritdoc cref="IStrategy.Running"/>
        public bool Running { get; private set; }

        /// <inheritdoc cref="IStrategy.Start"/>
        public bool Start(ref CS2StratRoulettePlugin plugin)
        {
            if (this.Running)
            {
                return false;
            }

            var allPlayers = Utilities.GetPlayers();
            var randomPlayer = allPlayers[this.random.Next(0, allPlayers.Count)];

            foreach (var playerController in allPlayers)
            {
                playerController.RemoveWeapons();
                playerController.GiveNamedItem("weapon_negev");
                playerController.GiveNamedItem("weapon_decoy");

            }
            Server.ExecuteCommand("mp_buytime 0");
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
            Server.ExecuteCommand("mp_buytime 20");

            this.Running = false;

            return true;
        }

    }
}
