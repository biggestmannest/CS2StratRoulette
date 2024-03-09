using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class HospitalBill : Strategy
	{
		public override string Name =>
			"Hospital bills";

		public override string Description =>
			"When you die all other players in your team will lose $500 to pay for your hospital bills.";

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
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

		private HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo _)
		{
			if (!this.Running)
			{
				return HookResult.Continue;
			}

			if (!@event.Userid.TryGetPlayerController(out var target))
			{
				return HookResult.Continue;
			}

			var team = target.Team;

			foreach (var player in Utilities.GetPlayers())
			{
				if (!player.IsValid || player.Team != team || player.UserId == target.UserId)
				{
					continue;
				}

				var moneyServices = player.InGameMoneyServices;

				if (moneyServices is null)
				{
					continue;
				}

				moneyServices.Account -= 500;

				Utilities.SetStateChanged(player, "CCSPlayerController", "m_pInGameMoneyServices");
			}

			return HookResult.Continue;
		}
	}
}
