using System.Collections.Generic;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Utils;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API.Core;
using CS2StratRoulette.Constants;
using CS2StratRoulette.Extensions;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Gladiator : Strategy
	{
		// private const string Enable =
		// 	"sv_cheats 1;sv_gravity 230;sv_airaccelerate 20000;sv_maxspeed 420;sv_friction 4;sv_cheats 0";
		//
		// private const string Disable =
		// 	"sv_cheats 1;sv_gravity 800;sv_airaccelerate 12;sv_maxspeed 320;sv_friction 5.2;sv_cheats 0";

		public override string Name =>
			"Gladiator";

		public override string Description =>
			"Fuckin' Gladiator ya fuck!";

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.PrecacheModel(Models.Fence);

			Gladiator.PrepareArena(Points.Mirage);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			return true;
		}

		private static void PrepareArena(Vector[] points)
		{
			for (var i = 0; i < points.Length; i++)
			{
				// Last point is only used for direction purposes
				if (i == points.Length - 1)
				{
					continue;
				}

				var point = points[i];
				var direction = points[i + 1];

				var entity = Utilities.CreateEntityByName<CBaseProp>("prop_dynamic");

				if (entity is null || !entity.IsValid)
				{
					continue;
				}

				entity.Teleport(point, point.Angle2(direction), VectorExtensions.Zero);
				entity.DispatchSpawn();
			}
		}
	}
}
