using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class HeadshotOnly : Strategy
	{
		private const string EnableHeadshotOnly = "mp_damage_headshot_only 1";
		private const string DisableHeadshotOnly = "mp_damage_headshot_only 0";

		public override string Name =>
			"Headshot Only";

		public override string Description =>
			"You can only kill players with a headshot.";

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(HeadshotOnly.EnableHeadshotOnly);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(HeadshotOnly.DisableHeadshotOnly);

			return true;
		}
	}
}
