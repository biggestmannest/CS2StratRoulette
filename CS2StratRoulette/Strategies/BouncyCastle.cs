using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CS2StratRoulette.Enums;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class BouncyCastle : Strategy
	{
		private const string Enable = "sv_bounce 6";
		private const string Disable = "sv_bounce 0";

		public override string Name =>
			"Bouncy Castle";

		public override string Description =>
			"You can bounce off of walls.";

		public override StrategyFlags Flags =>
			StrategyFlags.AlwaysVisible;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(BouncyCastle.Enable);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(BouncyCastle.Disable);

			return true;
		}
	}
}
