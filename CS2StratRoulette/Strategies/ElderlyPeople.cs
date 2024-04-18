using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class ElderlyPeople : Strategy
	{
		private const string Enable = "sv_maxspeed 170";
		private const string Disable = "sv_maxspeed 320";

		public override string Name =>
			"Elderly People";

		public override string Description =>
			"guhhhhhhhhhh my backkkk";

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(ElderlyPeople.Enable);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(ElderlyPeople.Disable);

			return true;
		}
	}
}
