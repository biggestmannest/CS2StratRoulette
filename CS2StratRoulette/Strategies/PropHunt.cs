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
using CS2StratRoulette.Helpers;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class PropHunt : Strategy
	{
		private const float Interval = 1f;
		private const float FreezeTime = 50f;

		private const string TasersInfinite = "mp_taser_recharge_time 99999";
		private const string TasersReset = "mp_taser_recharge_time 30";

		public override string Name =>
			"Prop Hunt";

		public override string Description =>
			"garry simon's modifications.";

		public override StrategyFlags Flags =>
			StrategyFlags.AlwaysVisible;

		private readonly System.Random random = new();

		private Timer? timer;
		private float startTime;
		private float emitSoundTime;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			var rulesProxy = Game.RulesProxy();
			var rules = rulesProxy?.GameRules;

			if (rulesProxy is not null && rules is not null)
			{
				rules.RoundTime += 200;
				rules.FreezeTime = 0;
				rules.FreezePeriod = false;

				Utilities.SetStateChanged(rulesProxy, "CCSGameRulesProxy", "m_pGameRules");
			}

			Server.ExecuteCommand(PropHunt.TasersInfinite);
			Server.ExecuteCommand(ConsoleCommands.DisableRadar);
			Server.ExecuteCommand(ConsoleCommands.BuyAllowNone);
			Server.ExecuteCommand(ConsoleCommands.BuyAllowGrenadesDisable);

			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					continue;
				}

				if (controller.Team is CsTeam.Terrorist)
				{
					Server.NextFrame(() =>
					{
						controller.EquipKnife();

						pawn.KeepWeaponsByType(CSWeaponType.WEAPONTYPE_KNIFE);
						pawn.RemoveC4();
					});

					Server.NextFrame(() =>
					{
						controller.GiveNamedItem(CsItem.HEGrenade);
						controller.GiveNamedItem(CsItem.Molotov);
						PropHunt.FreezePlayer(pawn);
					});

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

			this.startTime = (Server.CurrentTime + PropHunt.FreezeTime);
			this.emitSoundTime = this.startTime + 120f;

			this.timer = new Timer(PropHunt.Interval, this.OnInterval, TimerFlags.REPEAT);

			plugin.RegisterEventHandler<EventPlayerDeath>(this.OnPlayerDeath);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(PropHunt.TasersReset);
			Server.ExecuteCommand(ConsoleCommands.BuyAllowAll);
			Server.ExecuteCommand(ConsoleCommands.BuyAllowGrenadesEnable);
			Server.ExecuteCommand(ConsoleCommands.EnableRadar);

			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.IsValid)
				{
					continue;
				}

				controller.RemoveWeapons();
			}

			this.timer?.Kill();

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

		private void OnInterval()
		{
			var time = Server.CurrentTime;
			var freeze = time < this.startTime;
			var timeLeft = this.startTime - time;
			var emitSound = time > this.emitSoundTime && (int)(this.startTime - time) % 30 == 0;

			string? message = null;

			for (var i = 0; i < Server.MaxPlayers; i++)
			{
				var controller = Utilities.GetPlayerFromSlot(i);

				if (controller is null || !controller.IsValid)
				{
					continue;
				}

				if (freeze)
				{
					message ??= $"Seekers will be released in: {timeLeft.Str("F0")}";

					controller.PrintToCenter(message);

					return;
				}

				if (timeLeft >= -1.0f)
				{
					controller.PrintToCenter("The seekers have been released");
				}

				if (!emitSound ||
					controller.Team is not CsTeam.CounterTerrorist ||
					!controller.TryGetPlayerPawn(out var pawn))
				{
					continue;
				}

				PropHunt.SpawnDecoy(pawn);
			}
		}

		private static void FreezePlayer(CCSPlayerPawn pawn)
		{
			PropHunt.SetMoveType(pawn, MoveType_t.MOVETYPE_NONE);

			_ = new Timer(PropHunt.FreezeTime, () => PropHunt.SetMoveType(pawn, MoveType_t.MOVETYPE_WALK));
		}

		private static void SetMoveType(CCSPlayerPawn pawn, MoveType_t moveType)
		{
			pawn.MoveType = moveType;

			Schema.SetSchemaValue(pawn.Handle, "CBaseEntity", "m_nActualMoveType", (int)moveType);
			Utilities.SetStateChanged(pawn, "CBaseEntity", "m_MoveType");
		}

		private static void SpawnDecoy(CCSPlayerPawn pawn)
		{
			var decoy = Utilities.CreateEntityByName<CDecoyGrenade>("decoy_projectile");
			var position = pawn.AbsOrigin;

			if (decoy is null || position is null)
			{
				return;
			}

			Server.NextFrame(() =>
			{
				decoy.Collision.SolidType = SolidType_t.SOLID_NONE;
				decoy.Collision.CollisionGroup = (byte)CollisionGroup.COLLISION_GROUP_DEBRIS;
				decoy.Collision.CollisionAttribute.CollisionGroup = (byte)CollisionGroup.COLLISION_GROUP_DEBRIS;

				var collisionRulesChanged = new VirtualFunctionVoid<nint>(decoy.Handle, 172);

				collisionRulesChanged.Invoke(decoy.Handle);
			});

			Server.NextFrame(decoy.DispatchSpawn);

			position.Z += 30;

			decoy.Teleport(position);

			_ = new Timer(3f, decoy.Remove);
		}
	}
}
