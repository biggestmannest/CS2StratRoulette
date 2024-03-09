using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class StopHittingYourself : Strategy
	{
		private const string Half = "mp_weapon_self_inflict_amount 0.5";
		private const string Reset = "mp_weapon_self_inflict_amount 0";

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

			CounterStrikeSharp.API.Server.ExecuteCommand(StopHittingYourself.Half);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			CounterStrikeSharp.API.Server.ExecuteCommand(StopHittingYourself.Reset);

			return true;
		}
	}
}