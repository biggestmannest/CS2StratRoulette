using CounterStrikeSharp.API.Core;
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

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			foreach (var player in Utilities.GetPlayers())
			{
				if (!player.IsValid)
				{
					continue;
				}

				var moneyServices = player.InGameMoneyServices;

				if (moneyServices is null)
				{
					continue;
				}

				moneyServices.Account = 0;
				
				Utilities.SetStateChanged(player, "CCSPlayerController", "m_pInGameMoneyServices");
			}
			
			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			return true;
		}
	}
}
