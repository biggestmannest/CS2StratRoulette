using CS2StratRoulette.Constants;
using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;
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

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(PropHunt.DisableRadar);

			Server.PrecacheModel(Models.JuggernautCt);

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
	}
}
