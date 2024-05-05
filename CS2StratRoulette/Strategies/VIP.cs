using CS2StratRoulette.Constants;
using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CS2StratRoulette.Helpers;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	// ReSharper disable once InconsistentNaming
	public sealed class VIP : Strategy
	{
		public override string Name =>
			"VIP";

		public override string Description =>
			"One player from each team has been made VIP. If the VIP dies you lose the round.";

		public override StrategyFlags Flags =>
			StrategyFlags.AlwaysVisible;

		private readonly System.Random random = new();

		private CCSPlayerController? ctVip;
		private CCSPlayerController? tVip;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.PrecacheModel(Models.JuggernautCt);
			Server.PrecacheModel(Models.JuggernautT);

			var cts = new List<CCSPlayerController>(Server.MaxPlayers / 2);
			var ts = new List<CCSPlayerController>(Server.MaxPlayers / 2);

			Player.ForEach((controller) =>
			{
				if (!controller.IsValid || controller.IsBot)
				{
					return;
				}

				// ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
				switch (controller.Team)
				{
					case CsTeam.CounterTerrorist:
						cts.Add(controller);
						break;
					case CsTeam.Terrorist:
						ts.Add(controller);
						break;
				}
			});

			if (cts.Count > 0)
			{
				this.ctVip = VIP.MakeVip(cts[this.random.Next(cts.Count)]);
			}

			if (ts.Count > 0)
			{
				this.tVip = VIP.MakeVip(ts[this.random.Next(ts.Count)]);
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

			plugin.DeregisterEventHandler<EventPlayerDeath>(this.OnPlayerDeath);

			return true;
		}

		// ReSharper disable once InconsistentNaming
		private HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo _)
		{
			if (!this.Running)
			{
				return HookResult.Continue;
			}

			var controller = @event.Userid;

			if (controller is null || !controller.IsValid)
			{
				return HookResult.Continue;
			}

			if ((this.ctVip is null || controller.Slot != this.ctVip.Slot) &&
				(this.tVip is null || controller.Slot != this.tVip.Slot))
			{
				return HookResult.Continue;
			}

			var team = controller.Team;

			Player.ForEach((c) =>
			{
				if (c.Team == team)
				{
					c.CommitSuicide(false, true);
				}
			});

			return HookResult.Continue;
		}

		private static CCSPlayerController? MakeVip(CCSPlayerController controller)
		{
			if (!controller.TryGetPlayerPawn(out var pawn))
			{
				return null;
			}

			Server.NextFrame(() =>
			{
				pawn.KeepWeaponsByType(
					CSWeaponType.WEAPONTYPE_KNIFE,
					CSWeaponType.WEAPONTYPE_C4
				);
			});

			Server.NextFrame(() =>
			{
				controller.GiveNamedItem(CsItem.CZ);
				controller.EquipSecondary();

				pawn.SetModel(controller.Team is CsTeam.CounterTerrorist ? Models.JuggernautCt : Models.JuggernautT);
			});

			return controller;
		}
	}
}
