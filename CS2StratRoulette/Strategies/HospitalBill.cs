using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class HospitalBill : IStrategy
	{
		/// <inheritdoc cref="IStrategy.Name"/>
		public string Name => "Hospital bills";

		/// <inheritdoc cref="IStrategy.Description"/>
		public string Description =>
			"When you die all other players in your team will lose $500 to pay for your hospital bills.";

		/// <inheritdoc cref="IStrategy.Running"/>
		public bool Running { get; private set; }

		/// <inheritdoc cref="IStrategy.Start"/>
		public bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (this.Running)
			{
				return false;
			}

			plugin.RegisterEventHandler<EventPlayerDeath>(this.OnPlayerDeath);

			this.Running = true;

			return true;
		}

		/// <inheritdoc cref="IStrategy.Stop"/>
		public bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!this.Running)
			{
				return false;
			}

			this.Running = false;

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

				Utilities.SetStateChanged(player, "CPlayerControllerComponent", "m_iAccount");
			}

			return HookResult.Continue;
		}
	}
}
