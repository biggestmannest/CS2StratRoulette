using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Silvers : Strategy
	{
		public override string Name =>
			"Silvers";

		public override string Description =>
			"nice aim!!!!!!!!!!!!!!!!!!!!!!!!!!";

		private const string Enable = "sv_cheats 1;weapon_accuracy_forcespread 1;sv_cheats 0";
		private const string Disable = "sv_cheats 1;weapon_accuracy_forcespread 0;sv_cheats 0";

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(Silvers.Enable);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(Silvers.Disable);
            
			return true;
		}
	}
}
