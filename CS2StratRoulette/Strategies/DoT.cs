using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;
using CS2StratRoulette.Enums;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class DoT : Strategy
	{
		private const string Activate = "mp_global_damage_per_second 1";
		private const string Deactivate = "mp_global_damage_per_second 0";

		public override string Name =>
			"Damage over time";

		public override string Description =>
			"Kill or be killed by The Council.";

		public override StrategyFlags Flags { get; protected set; } = StrategyFlags.Hidden;

		public override bool Start(ref Base plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(DoT.Activate);

			return true;
		}

		public override bool Stop(ref Base plugin)
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
