using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class FireRain : Strategy
	{
		private const float Interval = 5f;

		private static readonly System.Random Random = new();

		public override string Name =>
			"Fire Rain";

		public override string Description =>
			"AAAHHHHHHHHHH";

		private Timer? timer;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			plugin.RegisterEventHandler<EventRoundAnnounceMatchStart>(this.OnRoundAnnounce);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			plugin.DeregisterEventHandler<EventRoundAnnounceMatchStart>(this.OnRoundAnnounce);

			this.timer?.Kill();

			return true;
		}

		private HookResult OnRoundAnnounce(EventRoundAnnounceMatchStart @event, GameEventInfo _)
		{
			this.timer = new Timer(FireRain.Interval, FireRain.OnInterval, TimerFlags.REPEAT);

			return HookResult.Continue;
		}

		private static void OnInterval()
		{
			for (var slot = 0; slot < Server.MaxPlayers; ++slot)
			{
				if (FireRain.Random.FiftyFifty())
				{
					continue;
				}

				FireRain.SpawnFire(Utilities.GetPlayerFromSlot(slot));
			}
		}

		private static void SpawnFire(CCSPlayerController controller)
		{
			if (!controller.TryGetPlayerPawn(out var pawn))
			{
				return;
			}

			var entity = Utilities.CreateEntityByName<CMolotovProjectile>("molotov_projectile");

			if (entity is null || !entity.IsValid)
			{
				return;
			}

			Server.NextFrame(() =>
			{
				entity.Collision.SolidType = SolidType_t.SOLID_NONE;
				entity.Collision.CollisionGroup = (byte)CollisionGroup.COLLISION_GROUP_DEBRIS;
				entity.Collision.CollisionAttribute.CollisionGroup = (byte)CollisionGroup.COLLISION_GROUP_DEBRIS;

				var collisionRulesChanged = new VirtualFunctionVoid<nint>(entity.Handle, 172);

				collisionRulesChanged.Invoke(entity.Handle);
			});

			var position = pawn.AbsOrigin ?? Vector.Zero;

			Server.NextFrame(() =>
			{
				if (entity.AbsOrigin is not null)
				{
					entity.AbsOrigin.X = position.X;
					entity.AbsOrigin.Y = position.Y;
					entity.AbsOrigin.Z = position.Z + 60f;
				}

				entity.DispatchSpawn();
				entity.AcceptInput("InitializeSpawnFromWorld");
			});
		}
	}
}
