using System.Collections.Generic;
using CS2StratRoulette.Constants;
using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Timers;
using CS2StratRoulette.Enums;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class PropHunt : Strategy
	{
		public override string Name =>
			"Prop Hunt";

		public override string Description =>
			"garry simon's modifications.";

		public override StrategyFlags Flags =>
			StrategyFlags.AlwaysVisible;

		private readonly System.Random random = new();

		private const string DisableRadar = "sv_disable_radar 1";
		private const string EnableRadar = "sv_disable_radar 0";

		private Timer timer;

		private float Interval = 40f;

		private float startTime;

		private const float freezeTime = 30f;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			this.startTime = (Server.CurrentTime + PropHunt.freezeTime);

			Server.ExecuteCommand(PropHunt.DisableRadar);

			Server.ExecuteCommand(ConsoleCommands.BuyAllowNone);
			Server.ExecuteCommand(ConsoleCommands.BuyAllowGrenadesDisable);

			Server.ExecuteCommand(ConsoleCommands.CheatsEnable);

			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					continue;
				}

				if (controller.Team is CsTeam.Terrorist)
				{
					controller.EquipKnife();

					pawn.KeepWeaponsByType(CSWeaponType.WEAPONTYPE_KNIFE);
					pawn.RemoveC4();

					Server.NextFrame(() =>
					{
						controller.GiveNamedItem(CsItem.HEGrenade);
						controller.GiveNamedItem(CsItem.Molotov);
					});
					Server.NextFrame(() => { this.BlindPlayer(controller); });

					continue;
				}

				if (controller.Team is not CsTeam.CounterTerrorist)
				{
					continue;
				}

				controller.RemoveWeapons();

				pawn.Health = 1;

				Server.NextFrame(() =>
				{
					pawn.SetModel(Models.Props[this.random.Next(Models.Props.Length)]);

					Utilities.SetStateChanged(controller, "CBaseEntity", "m_iHealth");
				});
			}

			this.timer = new Timer(1f, () =>
			{
				var time = Server.CurrentTime;
				if (time < this.startTime)
				{
					var message = $"Seekers will be released in: {(this.startTime - time).Str("F0")}";
					for (var i = 0; i < Server.MaxPlayers; i++)
					{
						var controller = Utilities.GetPlayerFromSlot(i);

						if (!controller.IsValid)
						{
							continue;
						}

						controller.PrintToCenter(message);
					}

					return;
				}
				else if ((int)time % 40 == 0)
				{
					for (var i = 0; i < Server.MaxPlayers; i++)
					{
						var controller = Utilities.GetPlayerFromSlot(i);

						if (controller.IsValid && controller.Team is CsTeam.CounterTerrorist)
						{
							if (!controller.TryGetPlayerPawn(out var pawn))
							{
								continue;
							}

							this.SpawnDecoy(pawn);
						}
					}
				}
			}, TimerFlags.REPEAT);

			plugin.RegisterEventHandler<EventPlayerDeath>(this.OnPlayerDeath);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(ConsoleCommands.BuyAllowAll);
			Server.ExecuteCommand(ConsoleCommands.BuyAllowGrenadesEnable);
			Server.ExecuteCommand(PropHunt.EnableRadar);

			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.IsValid)
				{
					continue;
				}

				controller.RemoveWeapons();
			}

			this.timer.Kill();

			plugin.DeregisterEventHandler<EventPlayerDeath>(this.OnPlayerDeath);

			return true;
		}

		private HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo _)
		{
			var controller = @event.Userid;

			if (controller is null || !controller.TryGetPlayerPawn(out var pawn))
			{
				return HookResult.Continue;
			}

			pawn.SetModel(controller.Team is CsTeam.CounterTerrorist ? Models.NormalCt : Models.NormalT);

			return HookResult.Continue;
		}

		private void BlindPlayer(CCSPlayerController controller)
		{
			if (!controller.TryGetPlayerPawn(out var pawn))
			{
				return;
			}

			this.SetMoveType(pawn, MoveType_t.MOVETYPE_NONE);

			new Timer(PropHunt.freezeTime, () => { this.SetMoveType(pawn, MoveType_t.MOVETYPE_WALK); });
		}

		private void SetMoveType(CCSPlayerPawn pawn, MoveType_t moveType)
		{
			pawn.MoveType = moveType;
			Schema.SetSchemaValue(pawn.Handle, "CBaseEntity", "m_nActualMoveType", (int)moveType);
			Utilities.SetStateChanged(pawn, "CBaseEntity", "m_MoveType");
		}

		private void SpawnDecoy(CCSPlayerPawn pawn)
		{
			var decoy = Utilities.CreateEntityByName<CDecoyGrenade>("decoy_projectile");

			var position = pawn.AbsOrigin ?? new Vector();
			position.Z += 30;

			Server.NextFrame(() =>
			{
				decoy.Collision.SolidType = SolidType_t.SOLID_NONE;
				decoy.Collision.CollisionGroup = (byte)CollisionGroup.COLLISION_GROUP_DEBRIS;
				decoy.Collision.CollisionAttribute.CollisionGroup = (byte)CollisionGroup.COLLISION_GROUP_DEBRIS;

				var collisionRulesChanged = new VirtualFunctionVoid<nint>(decoy.Handle, 172);

				collisionRulesChanged.Invoke(decoy.Handle);
			});

			Server.NextFrame(() => { decoy?.DispatchSpawn(); });

			decoy?.Teleport(position);

			new Timer(3f, () => { decoy?.Remove(); });
		}
	}
}
