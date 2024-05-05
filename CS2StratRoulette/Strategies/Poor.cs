using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;
using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;
using CS2StratRoulette.Helpers;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Poor : Strategy
	{
		public override string Name =>
			"The Economy";

		public override string Description =>
			"You have no money this round.";

		public override StrategyFlags Flags =>
			StrategyFlags.AlwaysVisible;

		/// <summary>
		/// Array storing the player's money indexed by their <see cref="CCSPlayerController.Slot"/>
		/// </summary>
		private readonly int[] accounts = new int[Server.MaxPlayers];

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Player.ForEach((controller) =>
			{
				var moneyServices = controller.InGameMoneyServices;

				if (moneyServices is null)
				{
					return;
				}

				this.accounts[controller.Slot] = moneyServices.Account;

				Server.NextFrame(() =>
				{
					moneyServices.Account = 0;

					Utilities.SetStateChanged(controller, "CCSPlayerController", "m_pInGameMoneyServices");
				});
			});

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Player.ForEach((controller) =>
			{
				var moneyServices = controller.InGameMoneyServices;

				if (moneyServices is null)
				{
					return;
				}

				Server.NextFrame(() =>
				{
					moneyServices.Account += this.accounts[controller.Slot];

					Utilities.SetStateChanged(controller, "CCSPlayerController", "m_pInGameMoneyServices");
				});
			});

			return true;
		}
	}
}
