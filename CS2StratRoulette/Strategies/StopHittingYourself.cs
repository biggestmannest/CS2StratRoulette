using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class StopHittingYourself : Strategy
	{
		private const string Half = "mp_weapon_self_inflict_amount 0.5";
		private const string Reset = "mp_weapon_self_inflict_amount 0";

		private const string KickDisable = "mp_autokick 0";
		private const string KickEnable = "mp_autokick 1";

		public override string Name =>
			"Stop Hitting Yourself!";

		public override string Description =>
			"If you miss you will receive half damage from your shot.";

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(StopHittingYourself.Half);
			Server.ExecuteCommand(StopHittingYourself.KickDisable);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(StopHittingYourself.Reset);
			Server.ExecuteCommand(StopHittingYourself.KickEnable);

			return true;
		}
	}
}
