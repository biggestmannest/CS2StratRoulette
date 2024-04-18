using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;
using CS2StratRoulette.Enums;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Phoon : Strategy
	{
		private const string EnablePh =
			"sv_cheats 1;sv_enablebunnyhopping 1;sv_maxvelocity 7000;sv_staminamax 0;sv_staminalandcost 0;sv_staminajumpcost 0;sv_accelerate_use_weapon_speed 0;sv_staminarecoveryrate 0;sv_autobunnyhopping 1;sv_airaccelerate 2000;sv_cheats 0";

		private const string DisablePh =
			"sv_cheats 1;sv_enablebunnyhopping 0;sv_maxvelocity 3500;sv_staminamax 80;sv_staminalandcost 0.05;sv_staminajumpcost 0.08;sv_accelerate_use_weapon_speed 1;sv_staminarecoveryrate 60;sv_autobunnyhopping 0;sv_airaccelerate 12;sv_cheats 0";

		public override string Name =>
			"Phoon";

		public override string Description =>
			"ADMIN HE'S DOING IT SIDEWAYS";

		public override StrategyFlags Flags =>
			StrategyFlags.AlwaysVisible;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(Phoon.EnablePh);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(Phoon.DisablePh);

			return true;
		}
	}
}
