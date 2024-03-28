using CS2StratRoulette.Constants;
using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Gladiator : Strategy
	{
		public override string Name =>
			"Gladiator";

		public override string Description =>
			"Fuckin' Gladiator ya fuck!";

		private GladiatorMapBounds bounds;

		private readonly List<CCSPlayerController> cts = new(Server.MaxPlayers / 2);
		private readonly List<CCSPlayerController> ts = new(Server.MaxPlayers / 2);

		private CCSPlayerController? ct;
		private CCSPlayerController? t;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			var map = Server.MapName;

			if (!GladiatorBounds.Maps.TryGetValue(map, out var mapBounds))
			{
				return false;
			}

			this.bounds = mapBounds;

			Server.ExecuteCommand(Commands.BuyAllowNone);
			Server.ExecuteCommand(Commands.BuyAllowGrenadesDisable);

			foreach (var (pos, angle) in this.bounds.Fences)
			{
				Gladiator.CreateFence(pos, angle);
			}

			var players = Utilities.GetPlayers();

			foreach (var controller in players)
			{
				if (!controller.IsValid)
				{
					continue;
				}

				controller.EquipKnife();
				controller.RemoveWeapons();
			}

			this.TeleportSpectators(players, this.bounds.Spectators.min, this.bounds.Spectators.max);
			this.PickGladiators();

			plugin.RegisterEventHandler<EventPlayerDeath>(this.OnPlayerDeath);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			const string playerDeath = "player_death";
			plugin.DeregisterEventHandler(playerDeath, this.OnPlayerDeath, true);

			return true;
		}

		private HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo _)
		{
			if (!this.Running || !@event.Userid.IsValid)
			{
				return HookResult.Continue;
			}

			this.PickGladiators();

			return HookResult.Continue;
		}

		private void PickGladiators()
		{
			if (!this.Running)
			{
				return;
			}

			this.ct = Gladiator.PickGladiator(this.ct, this.cts, this.bounds.Gladiators.ct);
			this.t = Gladiator.PickGladiator(this.t, this.ts, this.bounds.Gladiators.t);
		}

		private void TeleportSpectators(List<CCSPlayerController> players, Vector min, Vector max)
		{
			const float playerWidth = 32f;

			var playerCount = players.Count;

			var playersX = (int)float.Floor((max.X - min.X) / playerWidth);
			var playersY = (int)float.Floor((max.Y - min.Y) / playerWidth);

			for (var y = 0; y < playersY; y++)
			{
				for (var x = 0; x < playersX; x++)
				{
					if (playerCount < 0)
					{
						return;
					}

					var player = players[--playerCount];

					if (!player.IsValid || player.Team is not (CsTeam.Terrorist or CsTeam.CounterTerrorist))
					{
						continue;
					}

					if (player.Team is CsTeam.Terrorist)
					{
						this.ts.Add(player);
					}
					else
					{
						this.cts.Add(player);
					}

					player.Teleport(
						new(min.X + (playerWidth * x), min.Y + (playerWidth * y), 0f),
						new(0f, 0f, 0f),
						new(0f, 0f, 0f)
					);
				}
			}
		}

		private static CCSPlayerController? PickGladiator(CCSPlayerController? current,
														  List<CCSPlayerController> players,
														  Vector position)
		{
			if (current is not null && current.PawnIsAlive)
			{
				return null;
			}

			var next = players.Find(static (e) => (e.IsValid && e.PawnIsAlive));

			if (next?.AbsRotation is null)
			{
				return null;
			}

			next.Teleport(position, next.AbsRotation, VectorExtensions.Zero);

			return next;
		}

		private static void CreateFence(Vector position, QAngle angle)
		{
			var entity = Utilities.CreateEntityByName<CDynamicProp>("prop_dynamic");

			if (entity is null || !entity.IsValid)
			{
				return;
			}

			Server.NextFrame(() =>
			{
				entity.Collision.SolidType = SolidType_t.SOLID_VPHYSICS;
				entity.Collision.CollisionGroup = (byte)CollisionGroup.COLLISION_GROUP_NONE;
				entity.Collision.CollisionAttribute.CollisionGroup = (byte)CollisionGroup.COLLISION_GROUP_NONE;

				var collisionRulesChanged = new VirtualFunctionVoid<nint>(entity.Handle, 172);

				collisionRulesChanged.Invoke(entity.Handle);
			});

			Server.NextFrame(() =>
			{
				entity.DispatchSpawn();
				entity.SetModel(Models.Fence);
				entity.Teleport(position, angle, VectorExtensions.Zero);
			});
		}
	}
}
