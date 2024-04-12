using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

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

		public override bool Start(ref Base plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(Toxicity.EnableAllTalk);

			return true;
		}

		public override bool Stop(ref Base plugin)
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
