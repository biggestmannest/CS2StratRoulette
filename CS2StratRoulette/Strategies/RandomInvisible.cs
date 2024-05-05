using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using CS2StratRoulette.Helpers;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class RandomInvisible : Strategy
	{
		public override string Name =>
			"Where??? WHERE??????????";

		public override string Description =>
			"A random player has been made invisible.";

		private readonly System.Random random = new();

		private CCSPlayerController? randomPlayer;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			this.randomPlayer = Player.Get(this.random.Next(Player.Count));

			if (!this.randomPlayer.TryGetPlayerPawn(out var pawn))
			{
				return false;
			}

			pawn.Render = Color.FromArgb(0, pawn.Render);

			Utilities.SetStateChanged(pawn, "CBaseModelEntity", "m_clrRender");

			this.randomPlayer.PrintToCenter("You have been made invisible.");

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			if (this.randomPlayer is null)
			{
				return true;
			}

			if (!this.randomPlayer.TryGetPlayerPawn(out var pawn))
			{
				return false;
			}

			pawn.Render = Color.FromArgb(byte.MaxValue, pawn.Render);

			Utilities.SetStateChanged(this.randomPlayer, "CBaseModelEntity", "m_clrRender");

			return true;
		}
	}
}
