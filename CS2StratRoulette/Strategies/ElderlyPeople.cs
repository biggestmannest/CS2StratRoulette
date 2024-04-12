using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CS2StratRoulette.Enums;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class ElderlyPeople : Strategy
	{
		public override string Name =>
			"Elderly People";

		public override string Description =>
			"guhhhhhhhhhh my backkkk";

		public override StrategyFlags Flags { get; protected set; } = StrategyFlags.Hidden;

		private const string Enable = "sv_maxspeed 170";
		private const string Disable = "sv_maxspeed 320";

		public override bool Start(ref Base plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(ElderlyPeople.Enable);

			return true;
		}

		public override bool Stop(ref Base plugin)
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
