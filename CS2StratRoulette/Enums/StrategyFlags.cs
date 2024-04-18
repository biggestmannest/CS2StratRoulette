namespace CS2StratRoulette.Enums
{
	[System.Flags]
	public enum StrategyFlags
	{
		None = 0,
		AlwaysVisible = (1 << 0),
		TeamOnly = (1 << 1),

		All = StrategyFlags.AlwaysVisible |
			  StrategyFlags.TeamOnly,
	}
}
