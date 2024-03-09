using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public sealed class FlyingScoutsman : Strategy
    {
        /// <inheritdoc cref="IStrategy.Name"/>
        public override string Name => "Flying Scoutsman";

        /// <inheritdoc cref="IStrategy.Description"/>
        public override string Description =>
            "Low gravity + Scouts";

        /// <inheritdoc cref="IStrategy.Start"/>
        public override bool Start(ref CS2StratRoulettePlugin plugin)
        {
            if (!base.Start(ref plugin))
            {
                return false;
            }

            foreach (var playerController in Utilities.GetPlayers())
            {
                //todo: remove only primary and secondary because of C4
                playerController.RemoveWeapons();
                playerController.GiveNamedItem("weapon_ssg08");
            }
            Server.ExecuteCommand("sv_cheats 1;sv_gravity 230;sv_airaccelerate 20000; sv_maxspeed 420; sv_friction 4; sv_cheats 0");

            return true;
        }

        /// <inheritdoc cref="IStrategy.Stop"/>
        public override bool Stop(ref CS2StratRoulettePlugin plugin)
        {
            if (!base.Stop(ref plugin))
            {
                return false;
            }
            foreach (var playerController in Utilities.GetPlayers())
            {
                playerController.RemoveWeapons();
            }
            Server.ExecuteCommand("sv_cheats1;sv_gravity 800;sv_airaccelerate 12;sv_maxspeed 320;sv_friction 5.2;sv_cheats 0");

            return true;
        }

    }
}
