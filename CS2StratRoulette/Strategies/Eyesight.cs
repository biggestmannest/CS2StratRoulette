﻿using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using CounterStrikeSharp.API;
using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Eyesight : Strategy
	{
		public override string Name =>
			"Eyesight";

		public override string Description =>
			"Look carefully, everyone is at 50% visibility";

		public override StrategyFlags Flags { get; protected set; } = StrategyFlags.Hidden;

		public override bool Start(ref Base plugin)
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
			}

			return true;
		}

		public override bool Stop(ref Base plugin)
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
			}

			return true;
		}
	}
}
