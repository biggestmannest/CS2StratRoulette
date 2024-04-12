using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Poor : Strategy
	{
		public override string Name =>
			"The Economy";

		public override string Description =>
			"You have no money this round.";

		/// <summary>
		/// Array storing the player's guessed number indexed by their <see cref="CCSPlayerController.Slot"/>
		/// </summary>
		private readonly int[] accounts = new int[Server.MaxPlayers];

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.IsValid)
				{
					continue;
				}

				var moneyServices = controller.InGameMoneyServices;

				if (moneyServices is null)
				{
					continue;
				}

				this.accounts[controller.Slot] = moneyServices.Account;

				moneyServices.Account = 0;

				Utilities.SetStateChanged(controller, "CCSPlayerController", "m_pInGameMoneyServices");
			}

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

				var moneyServices = controller.InGameMoneyServices;

				if (moneyServices is null)
				{
					continue;
				}

				moneyServices.Account += this.accounts[controller.Slot];

				Utilities.SetStateChanged(controller, "CCSPlayerController", "m_pInGameMoneyServices");
			}

			return true;
		}
	}
}
