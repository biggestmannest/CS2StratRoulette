using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization.Formatters;

namespace CS2StratRoulette.Strategies
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public class Earrape : IStrategy
    {
        /// <inheritdoc cref="IStrategy.Name"/>
        public string Name => "Earrape";
        private readonly System.Random random = new();

        /// <inheritdoc cref="IStrategy.Description"/>
        public string Description => "Everyone gets a Negev and decoy grenades with infinite ammo, succesfully ruining the enemies ears.";

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

            /* 
            TODO: fix the fact that it sometimes doesn't give the player the bomb 
            likely due to sv_cheats either not executing quickly enough, or too quick.
            */
            if (randomPlayer.IsValid)
            {
                if (randomPlayer.PlayerPawn.Value?.TeamNum is 2) // 2 = T, 3 = CT
                {
                    Server.ExecuteCommand("sv_cheats 1");
                    randomPlayer.GiveNamedItem("weapon_c4");
                    Server.ExecuteCommand("sv_cheats 0");
                }
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
            var allPlayers = Utilities.GetPlayers();
            foreach (var playerController in allPlayers)
            {
                playerController.RemoveItemByDesignerName("weapon_negev");
            }
            Server.ExecuteCommand("mp_buytime 20");

            this.Running = false;

            return true;
        }

    }
}
