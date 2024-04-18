using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class IceSkating : Strategy
	{
		private const string RemoveFriction = "sv_friction 0";
		private const string ResetFriction = "sv_friction 5.2";

		public override string Name =>
			"Ice Skating";

		public override string Description =>
			"Skate like the wind.";

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(IceSkating.RemoveFriction);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(IceSkating.ResetFriction);

			return true;
		}
	}
}
