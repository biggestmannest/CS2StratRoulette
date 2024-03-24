using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Utils;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Memory;
using CS2StratRoulette.Constants;
using CS2StratRoulette.Extensions;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Gladiator : Strategy
	{
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
			if ((points.Length & 1) != 0)
			{
				throw new System.Exception("[PrepareArena] uneven array");
			}

			for (var i = 0; i < points.Length; i += 2)
			{
				if (i == points.Length - 1)
				{
					continue;
				}

				var point = points[i];
				var direction = points[i + 1];
				var diff = point - direction;
				var step = diff.Unit2D() * Models.FenceWidth;
				var angle = point.Angle2(direction);

				var fences = (int)float.Ceiling(float.Abs(diff.Length2D()) / Models.FenceWidth);

				for (var j = 1; j <= fences; j++)
				{
					Gladiator.CreateFence(point + (step * j), angle);
				}
			}
		}

		private static CDynamicProp? CreateFence(Vector position, QAngle angle)
		{
			var entity = Utilities.CreateEntityByName<CDynamicProp>("prop_dynamic");

			if (entity is null || !entity.IsValid)
			{
				return null;
			}

			Server.NextFrame(() =>
			{
				entity.Collision.SolidType = SolidType_t.SOLID_VPHYSICS;
				entity.Collision.CollisionGroup = (byte)CollisionGroup.COLLISION_GROUP_NONE;
				entity.Collision.CollisionAttribute.CollisionGroup = (byte)CollisionGroup.COLLISION_GROUP_NONE;

				var collisionRulesChanged = new
					VirtualFunctionVoid<nint>(entity.Handle, 172);

				collisionRulesChanged.Invoke(entity.Handle);
			});

			Server.NextFrame(() =>
			{
				entity.DispatchSpawn();
				entity.SetModel(Models.Fence);
				entity.Teleport(position, angle, VectorExtensions.Zero);
			});

			return entity;
		}
	}
}
