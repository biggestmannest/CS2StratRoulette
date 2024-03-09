using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class PlantAnywhere : Strategy
	{
		public override string Name =>
			"Plant Anywhere";

		public override string Description =>
			"The bomb may be planted anywhere.";

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			CounterStrikeSharp.API.Server.ExecuteCommand("mp_plant_c4_anywhere 1");

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			CounterStrikeSharp.API.Server.ExecuteCommand("mp_plant_c4_anywhere 0");

			return true;
		}
	}
}
