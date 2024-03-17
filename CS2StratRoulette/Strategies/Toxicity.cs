using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;
using CS2StratRoulette.Enums;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Toxicity : Strategy
	{
		private const string EnableAllTalk = "sv_full_alltalk 1";
		private const string DisableAllTalk = "sv_full_alltalk 0";

		public override string Name =>
			"Toxicity";

		public override string Description =>
			"Global voice chat has been turned on.";
		

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(Toxicity.EnableAllTalk);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(Toxicity.DisableAllTalk);

			return true;
		}
	}
}
