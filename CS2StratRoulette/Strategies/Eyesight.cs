using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using CS2StratRoulette.Enums;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Eyesight : Strategy
	{
		public override string Name =>
			"Eyesight";

		public override string Description =>
			"Look carefully, everyone is at 50% visibility";

		public override StrategyFlags Flags =>
			StrategyFlags.AlwaysVisible;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					continue;
				}

				pawn.Render = Color.FromArgb(byte.MaxValue / 2, pawn.Render);

				Utilities.SetStateChanged(pawn, "CBaseModelEntity", "m_clrRender");
			}

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					continue;
				}

				pawn.Render = Color.FromArgb(byte.MaxValue, pawn.Render);

				Utilities.SetStateChanged(pawn, "CBaseModelEntity", "m_clrRender");
			}

			return true;
		}
	}
}
