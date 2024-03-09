using CS2StratRoulette.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Strike : Strategy, IStrategyPostStop
	{
		private const string Enable = "mp_weapons_allow_map_placed 1";
		private const string Disable = "mp_weapons_allow_map_placed 0";

		public override string Name =>
			"Janitors on Strike";

		public override string Description =>
			"The janitors are on strike so dropped items won't disappear after this round.";

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			CounterStrikeSharp.API.Server.ExecuteCommand(Strike.Disable);

			return true;
		}

		public void PostStop()
		{
			CounterStrikeSharp.API.Server.ExecuteCommand(Strike.Enable);
		}
	}
}
