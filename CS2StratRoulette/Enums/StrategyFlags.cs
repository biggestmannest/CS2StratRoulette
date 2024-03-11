using System;

namespace CS2StratRoulette.Enums
{
	[Flags]
	public enum StrategyFlags
	{
		None = 0,
		Hidden = (1 << 0),
		TeamOnly = (1 << 1),

		All = StrategyFlags.Hidden |
			  StrategyFlags.TeamOnly,
	}
}
