using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;
using CS2StratRoulette.Extensions;
using CS2StratRoulette.Enums;
using CounterStrikeSharp.API.Modules.Utils;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

namespace CS2StratRoulette.Strategies
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public class Earrape : Strategy
    {

        private static string startCmds = $"sv_cheats 1; sv_infinite_ammo 2; sv_cheats 0; mp_buy_allow_guns {BuyAllow.None.Str()}; mp_buy_allow_grenades {BuyAllow.None.Str()}; mp_weapons_buy_allow_zeus {BuyAllow.None.Str()}";

        private static string stopCmds = $"sv_cheats 1; sv_infinite_ammo 0; sv_cheats 0; mp_buy_allow_guns {BuyAllow.All.Str()}; mp_buy_allow_grenades 1; mp_weapons_buy_allow_zeus 1";
        /// <inheritdoc cref="IStrategy.Name"/>
        public override string Name => "Earrape";

        /// <inheritdoc cref="IStrategy.Description"/>
        public override string Description => "Everyone gets a Negev and decoy grenades, succesfully ruining the enemies ears.";

        private readonly System.Random random = new();

        private readonly List<CCSPlayerController> allPlayers = Utilities.GetPlayers();

        /// <inheritdoc cref="IStrategy.Start"/>
        public override bool Start(ref CS2StratRoulettePlugin plugin)
        {
            if (!base.Start(ref plugin))
            {
                return false;
            }

            Server.ExecuteCommand(startCmds);

            var randomPlayer = allPlayers[this.random.Next(0, allPlayers.Count)];


            foreach (var playerController in allPlayers)
            {
                //TODO: make it easier to remove weapons from inventory instead of removing all items, since c4 dies too
                playerController.RemoveWeapons();
                playerController.GiveNamedItem("weapon_negev");
                playerController.GiveNamedItem("weapon_decoy");

            }

            Server.ExecuteCommand("sv_cheats 1");

            if (randomPlayer.IsValid && randomPlayer.Team is CsTeam.Terrorist)
            {
                randomPlayer.GiveNamedItem("weapon_c4");
            }

            Server.ExecuteCommand("sv_cheats 0");

            return true;
        }

        /// <inheritdoc cref="IStrategy.Stop"/>
        public override bool Stop(ref CS2StratRoulettePlugin plugin)
        {
            if (!base.Stop(ref plugin))
            {
                return false;
            }
            Server.ExecuteCommand(stopCmds);
            foreach (var playerController in allPlayers)
            {
                //TODO: make it easier to remove weapons from inventory instead of removing all items, since c4 dies too
                playerController.RemoveWeapons();
            }

            return true;
        }

    }
}
