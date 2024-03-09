using CS2StratRoulette.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Strike : Strategy, IStrategyPreStop, IStrategyPostStop
	{
		public override string Name =>
			"Janitors on Strike";

		public override string Description =>
			"The janitors are on strike so dropped items won't disappear after this round.";

		public void PreStop()
		{
			CounterStrikeSharp.API.Server.ExecuteCommand("mp_weapons_allow_map_placed 1");
		}

		public void PostStop()
		{
			CounterStrikeSharp.API.Server.ExecuteCommand("mp_weapons_allow_map_placed 0");
		}
	}
}
