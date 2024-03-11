using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class DoT : Strategy
	{
		private const string Activate = "mp_global_damage_per_second 0";
		private const string Deactivate = "mp_global_damage_per_second 1";

		public override string Name =>
			"Damage over time";

		public override string Description =>
			"Kill or be killed by The Council.";

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(DoT.Activate);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(DoT.Deactivate);

			return true;
		}
	}
}
