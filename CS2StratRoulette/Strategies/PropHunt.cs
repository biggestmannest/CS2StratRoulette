using CS2StratRoulette.Constants;
using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class PropHunt : Strategy
	{
		private const byte Tries = 4;
		private const byte DamageOnTry = (100 /*HP*/ / PropHunt.Tries);

		public override string Name =>
			"Prop Hunt";

		public override string Description =>
			"garry simon's modifications.";

		private readonly System.Random random = new();

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

				if (controller.Team is CsTeam.Terrorist)
				{
					pawn.KeepWeaponsByType(CSWeaponType.WEAPONTYPE_KNIFE);

					controller.GiveNamedItem(CsItem.HEGrenade);
					controller.GiveNamedItem(CsItem.Molotov);

					continue;
				}

				if (controller.Team is not CsTeam.CounterTerrorist)
				{
					continue;
				}

				controller.RemoveWeapons();
				pawn.Health = 1;

				controller.ExecuteClientCommandFromServer(Commands.ThirdPersonEnable);

				Server.NextFrame(() =>
				{
					pawn.SetModel(Models.Props[this.random.Next(Models.Props.Length)]);
					Utilities.SetStateChanged(controller, "CBaseEntity", "m_iHealth");
				});
			}

			plugin.RegisterEventHandler<EventPlayerShoot>(PropHunt.OnPlayerShoot);
			plugin.RegisterEventHandler<EventPlayerDeath>(PropHunt.OnPlayerDeath);

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
				if (!controller.IsValid)
				{
					continue;
				}

				controller.RemoveWeapons();

				controller.ExecuteClientCommandFromServer(Commands.ThirdPersonDisable);
			}

			const string playerShoot = "player_shoot";
			const string playerDeath = "player_death";

			plugin.DeregisterEventHandler(playerShoot, PropHunt.OnPlayerShoot, true);
			plugin.DeregisterEventHandler(playerDeath, PropHunt.OnPlayerDeath, true);

			return true;
		}

		private static HookResult OnPlayerShoot(EventPlayerShoot @event, GameEventInfo _)
		{
			var controller = @event.Userid;

			if (controller.Team is not CsTeam.Terrorist || !controller.TryGetPlayerPawn(out var pawn))
			{
				return HookResult.Continue;
			}

			pawn.Health -= PropHunt.DamageOnTry;

			if (pawn.Health <= 0)
			{
				pawn.CommitSuicide(false, true);
			}
			else
			{
				Utilities.SetStateChanged(controller, "CBaseEntity", "m_iHealth");
			}

			return HookResult.Continue;
		}

		private static HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo _)
		{
			var target = @event.Userid;
			var attacker = @event.Attacker;

			if ((attacker.Team is CsTeam.Terrorist &&
				 target.Team is CsTeam.CounterTerrorist) ||
				!attacker.TryGetPlayerPawn(out var pawn))
			{
				return HookResult.Continue;
			}

			pawn.Health += PropHunt.DamageOnTry;

			Utilities.SetStateChanged(attacker, "CBaseEntity", "m_iHealth");

			return HookResult.Continue;
		}
	}
}
