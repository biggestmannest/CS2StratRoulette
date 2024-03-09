
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;

namespace CS2StratRoulette.Strategies
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public class Phoon : Strategy
    {
        private const string enablePh = "sv_cheats 1;sv_enablebunnyhopping 1;sv_maxvelocity 7000;sv_staminamax 0;sv_staminalandcost 0;sv_staminajumpcost 0;sv_accelerate_use_weapon_speed 0;sv_staminarecoveryrate 0;sv_autobunnyhopping 1;sv_airaccelerate 2000;sv_cheats 0";

        private const string disablePh = "sv_cheats 1;sv_enablebunnyhopping 0;sv_maxvelocity 3500;sv_staminamax 80;sv_staminalandcost 0.05;sv_staminajumpcost 0.08;sv_accelerate_use_weapon_speed 1;sv_staminarecoveryrate 60;sv_autobunnyhopping 0;sv_airaccelerate 12;sv_cheats 0";

        /// <inheritdoc cref="IStrategy.Name"/>
        public override string Name => "Phoon";

        /// <inheritdoc cref="IStrategy.Description"/>
        public override string Description =>
            "ADMIN HE'S DOING IT SIDEWAYS";


        /// <inheritdoc cref="IStrategy.Start"/>
        public override bool Start(ref CS2StratRoulettePlugin plugin)
        {
            if (!base.Start(ref plugin))
            {
                return false;
            }
            Server.ExecuteCommand(enablePh);

            return true;
        }

        /// <inheritdoc cref="IStrategy.Stop"/>
        public override bool Stop(ref CS2StratRoulettePlugin plugin)
        {
            if (!base.Stop(ref plugin))
            {
                return false;
            }

            Server.ExecuteCommand(disablePh);


            return true;
        }
    }
}
