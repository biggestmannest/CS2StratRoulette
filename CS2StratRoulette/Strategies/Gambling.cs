using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Utils;
using CS2StratRoulette.Extensions;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Gambling : Strategy
	{
		public override string Name =>
			"Roulette";

		public override string Description =>
			"If you guess the number between 1 and 10 correctly you get 200HP :) happy gambling!";

		private static readonly System.Random Random = new();

		private readonly int[] guessers = new int[Server.MaxPlayers];

		private float endTime;

		private int numPick;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.PrintToChatAll(
				$" {ChatColors.Red}Pick a number between 1 and 10 in chat. You have 10 seconds. Winners will be announced after the 10 seconds have passed.");

			plugin.RegisterListener<Listeners.OnTick>(this.OnTick);

			this.numPick = (Gambling.Random.Next(10) + 1);
			this.endTime = Server.CurrentTime + 10f;

			plugin.RegisterEventHandler<EventPlayerChat>(this.OnNumberPicked);
			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			const string playerChat = "player_chat";
			plugin.DeregisterEventHandler(playerChat, this.OnNumberPicked, true);

			const string onTick = "OnTick";
			plugin.RemoveListener(onTick, this.OnTick);

			return true;
		}

		private HookResult OnNumberPicked(EventPlayerChat @event, GameEventInfo _)
		{
			if (!this.Running)
			{
				return HookResult.Continue;
			}

			if (!int.TryParse(@event?.Text, CultureInfo.InvariantCulture, out var num))
			{
				return HookResult.Continue;
			}

			var controller = Utilities.GetPlayerFromIndex(@event.Userid);

			if (this.guessers[controller.Slot] != default)
			{
				return HookResult.Continue;
			}

			this.guessers[controller.Slot] = num;
			return HookResult.Continue;
		}

		private static void GiveUpgrade(CCSPlayerController controller)
		{
			if (!controller.TryGetPlayerPawn(out var pawn))
			{
				return;
			}

			Server.NextFrame(() =>
			{
				pawn.MaxHealth = 200;
				pawn.Health = 200;
			});

			Server.NextFrame(() =>
			{
				Utilities.SetStateChanged(controller, "CBaseEntity", "m_iMaxHealth");
				Utilities.SetStateChanged(controller, "CBaseEntity", "m_iHealth");
			});
		}

		private void OnTick()
		{
			if (Server.CurrentTime < this.endTime)
			{
				return;
			}

			this.endTime = 0f;

			var theNumber = this.numPick.Str();
			var correct = $"You got it! The correct number was {theNumber}. Enjoy the 200HP.";
			var wrong = $"You got it wrong. The correct number was {theNumber}. L";

			for (var index = 0; index < this.guessers.Length; index++)
			{
				var number = this.guessers[index];
				var controller = Utilities.GetPlayerFromSlot(index);
				var won = number == this.numPick;
				var msg = won ? correct : wrong;

				if (won)
				{
					Gambling.GiveUpgrade(controller);
				}

				controller.PrintToCenter(msg);
			}
		}
	}
}
