using System.Collections.Generic;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Utils;
using CS2StratRoulette.Constants;
using CS2StratRoulette.Extensions;

namespace CS2StratRoulette.Strategies
{
	// ReSharper disable once InconsistentNaming
	public sealed class VIP : Strategy
	{
		public override string Name =>
			"VIP";

		public override string Description =>
			"One player from each team has been made VIP. If the VIP dies you lose the round.";

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

			var cts = new List<CCSPlayerController>(10);
			var ts = new List<CCSPlayerController>(10);

			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.IsValid)
				{
					continue;
				}

				// ReSharper disable once ConvertIfStatementToSwitchStatement
				if (controller.Team is CsTeam.CounterTerrorist)
				{
					cts.Add(controller);
				}
				else if (controller.Team is CsTeam.Terrorist)
				{
					ts.Add(controller);
				}
			}

			if (cts.Count > 0)
			{
				var ct = cts[this.random.Next(cts.Count)];

				if (ct.IsValid)
				{
					this.ctVip = VIP.MakeVip(ct);
				}
			}

			if (ts.Count > 0)
			{
				var t = ts[this.random.Next(ts.Count)];

				if (t.IsValid)
				{
					this.tVip = VIP.MakeVip(t);
				}
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

			const string playerDeath = "player_death";

			plugin.DeregisterEventHandler(playerDeath, this.OnPlayerDeath, true);

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

			if (!controller.IsValid)
			{
				return HookResult.Continue;
			}

			if ((this.ctVip is null || controller.SteamID != this.ctVip.SteamID) &&
			    (this.tVip is null || controller.SteamID != this.tVip.SteamID))
			{
				return HookResult.Continue;
			}

			var game = new CCSGameRules(controller.Handle);

			if (game.Handle == System.IntPtr.Zero)
			{
				return HookResult.Continue;
			}

			var reason = controller.Team is CsTeam.CounterTerrorist
				             ? RoundEndReason.TerroristsWin
				             : RoundEndReason.CTsWin;

			game.TerminateRound(1.0f, reason);

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

				pawn.SetModel(controller.Team is CsTeam.CounterTerrorist ? Models.JuggernautCt : Models.JuggernautT);
			});

			return controller;
		}
	}
}
