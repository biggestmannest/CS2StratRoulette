using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class NoRadar : Strategy
	{
		public override string Name =>
			"Where the hell are they???";

		public override string Description =>
			"You cannot see your radar.";

		private const string Enable = "sv_disable_radar 1";
		private const string Disable = "sv_disable_radar 0";

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(NoRadar.Enable);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(NoRadar.Disable);

			return true;
		}
	}
}
