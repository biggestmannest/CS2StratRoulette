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

			Server.PrecacheModel(Models.JuggernautCt);

			Server.ExecuteCommand(Commands.BuyAllowNone);
			Server.ExecuteCommand(Commands.BuyAllowGrenadesDisable);

			Server.ExecuteCommand(Commands.CheatsEnable);

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

					controller.EquipKnife();

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

			plugin.RegisterEventHandler<EventPlayerDeath>(this.OnPlayerDeath);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(Commands.BuyAllowAll);
			Server.ExecuteCommand(Commands.BuyAllowGrenadesEnable);

			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.IsValid)
				{
					continue;
				}

				controller.RemoveWeapons();
			}

			const string playerDeath = "player_death";
			plugin.DeregisterEventHandler(playerDeath, this.OnPlayerDeath, true);

			return true;
		}

		private HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo _)
		{
			var controller = @event.Userid;

			if (!controller.TryGetPlayerPawn(out var pawn))
			{
				return HookResult.Continue;
			}

			pawn.SetModel(controller.Team is CsTeam.CounterTerrorist ? Models.NormalCt : Models.NormalT);

			return HookResult.Continue;
		}
	}
}
