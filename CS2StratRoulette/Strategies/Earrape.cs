using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Utils;
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

            Server.ExecuteCommand("mp_buy_allow_guns 0");
            Server.ExecuteCommand("mp_buy_allow_grenades 0");
            Server.ExecuteCommand("mp_weapons_allow_zeus 0");

            var allPlayers = Utilities.GetPlayers();
            var randomPlayer = allPlayers[this.random.Next(0, allPlayers.Count)];


            foreach (var playerController in allPlayers)
            {
                //TODO: make it easier to remove weapons from inventory instead of removing all items, since c4 dies too
                playerController.GiveNamedItem("weapon_negev");
                playerController.GiveNamedItem("weapon_decoy");

            }

            Server.ExecuteCommand("sv_cheats 1");

            if (randomPlayer.IsValid && randomPlayer.Team is CsTeam.Terrorist)
            {
                randomPlayer.GiveNamedItem("weapon_c4");
            }

            Server.ExecuteCommand("sv_cheats 0");

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
            Server.ExecuteCommand("mp_buy_allow_grenades 1");
            Server.ExecuteCommand("mp_weapons_allow_zeus 1");

            this.Running = false;

            return true;
        }

    }
}
