using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Moon : Strategy
	{
		private const string Enable =
			"sv_cheats 1;sv_gravity 230;sv_airaccelerate 20000;sv_maxspeed 420;sv_friction 4;sv_cheats 0";

		private const string Disable =
			"sv_cheats 1;sv_gravity 800;sv_airaccelerate 12;sv_maxspeed 320;sv_friction 5.2;sv_cheats 0";

		public override string Name =>
			"Moon";

		public override string Description =>
			"One small step for man (or woman), one giant leap for (wo)mankind.";

		public override bool Start(ref Base plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(Moon.Enable);

			return true;
		}

		public override bool Stop(ref Base plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(Moon.Disable);

			return true;
		}
	}
}
