using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;
using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Gambling : Strategy
	{
		private const float GuessingDuration = 15f;

		private static readonly System.Random Random = new();

		public override string Name =>
			"Roulette";

		public override string Description =>
			$"{ChatColors.LightRed}Pick a number between 1 and 10 in chat. You have 10 seconds. Winners will be announced after the 10 seconds have passed";

		public override StrategyFlags Flags =>
			StrategyFlags.AlwaysVisible;

		/// <summary>
		/// Array storing the player's guessed number indexed by their <see cref="CCSPlayerController.Slot"/>
		/// </summary>
		private readonly int[] players = new int[Server.MaxPlayers];

		private int number;
		private float endTime;
		private Timer? timer;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			this.number = (Gambling.Random.Next(10) + 1);
			this.endTime = Server.CurrentTime + Gambling.GuessingDuration;

			plugin.RegisterEventHandler<EventPlayerChat>(this.OnNumberPicked);

			this.timer = new Timer(1f, this.OnInterval, 0);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			this.timer?.Kill();

			plugin.DeregisterEventHandler<EventPlayerChat>(this.OnNumberPicked);

			return true;
		}

		private HookResult OnNumberPicked(EventPlayerChat @event, GameEventInfo _)
		{
			if (!this.Running)
			{
				return HookResult.Continue;
			}

			if (!int.TryParse(@event.Text, CultureInfo.InvariantCulture, out var num))
			{
				return HookResult.Continue;
			}

			var controller = Utilities.GetPlayerFromUserid(@event.Userid);

			if (controller is null || this.players[controller.Slot] != default)
			{
				return HookResult.Continue;
			}

			this.players[controller.Slot] = num;

			return HookResult.Continue;
		}

		private void OnInterval()
		{
			var time = Server.CurrentTime;
			var ended = time >= this.endTime;

			if (ended)
			{
				this.End();
				this.timer?.Kill();

				return;
			}

			var message = $"Time left to guess: {(this.endTime - time).Str("F0")}";

			for (var slot = 0; slot < Server.MaxPlayers; slot++)
			{
				var controller = Utilities.GetPlayerFromSlot(slot);

				if (controller is null || !controller.IsValid)
				{
					continue;
				}

				controller.PrintToCenter(message);
			}
		}

		private void End()
		{
			var n = this.number.Str();

			var correct = $"You got it! The correct number was {n}. Enjoy the 200HP.";
			var wrong = $"You got it wrong. The correct number was {n}. L (-40 HP)";

			for (var slot = 0; slot < Server.MaxPlayers; slot++)
			{
				var controller = Utilities.GetPlayerFromSlot(slot);

				if (controller is null || !controller.IsValid)
				{
					continue;
				}

				var guess = this.players[slot];
				var won = (guess == this.number);
				var msg = wrong;

				if (won)
				{
					msg = correct;

					Gambling.GiveUpgrade(controller);
				}
				else
				{
					Gambling.Punish(controller);
				}

				controller.PrintToCenter(msg);
			}
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
				Utilities.SetStateChanged(pawn, "CBaseEntity", "m_iMaxHealth");
				Utilities.SetStateChanged(pawn, "CBaseEntity", "m_iHealth");
			});
		}

		private static void Punish(CCSPlayerController controller)
		{
			if (!controller.TryGetPlayerPawn(out var pawn))
			{
				return;
			}

			Server.NextFrame(() => { pawn.Health -= 40; });

			Server.NextFrame(() => { Utilities.SetStateChanged(pawn, "CBaseEntity", "m_iHealth"); });
		}
	}
}
