using System.Diagnostics.CodeAnalysis;
using CS2StratRoulette.Enums;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class PlantAnywhere : Strategy
	{
		private const string Enable = "mp_plant_c4_anywhere 1; mp_c4timer 60";
		private const string Disable = "mp_plant_c4_anywhere 0; mp_c4timer 40";

		public override string Name =>
			"Plant Anywhere";

		public override string Description =>
			"The bomb may be planted anywhere.";

		public override StrategyFlags Flags { get; protected set; } = StrategyFlags.Hidden;

		public override bool Start(ref Base plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			CounterStrikeSharp.API.Server.ExecuteCommand(PlantAnywhere.Enable);

			return true;
		}

		public override bool Stop(ref Base plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			CounterStrikeSharp.API.Server.ExecuteCommand(PlantAnywhere.Disable);

			return true;
		}
	}
}
