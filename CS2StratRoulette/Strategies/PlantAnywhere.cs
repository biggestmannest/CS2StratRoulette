using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class PlantAnywhere : Strategy
	{
		private const string Enable = "mp_plant_c4_anywhere 1";
		private const string Disabled = "mp_plant_c4_anywhere 0";

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

			CounterStrikeSharp.API.Server.ExecuteCommand(PlantAnywhere.Enable);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			CounterStrikeSharp.API.Server.ExecuteCommand(PlantAnywhere.Disabled);

			return true;
		}
	}
}