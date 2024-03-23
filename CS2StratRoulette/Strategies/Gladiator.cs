using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Utils;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
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
			foreach (var point in points)
			{
				var entity = Utilities.CreateEntityByName<CDynamicProp>("prop_dynamic");

				if (entity is null || !entity.IsValid)
				{
					continue;
				}

				entity.Collision.CollisionGroup = (byte)CollisionGroup.COLLISION_GROUP_PLAYER;

				entity.DispatchSpawn();
				entity.SetModel(Models.Fence);
				entity.Teleport(point, new(0f, 0f, 0f), VectorExtensions.Zero);
			}
		}
	}
}
