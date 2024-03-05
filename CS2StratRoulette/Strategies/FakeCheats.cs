using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public class FakeCheats : IStrategy
	{
		private const float Interval = 3.0f;

		private static readonly string[] Messages =
		{
			"NEVER VAC AND YOU KNOW! ONLY @ GOGlobal.cheat",
			"AIM, WALLS & MORE @ GOGlobal.cheat",
			"50% OFF ONLY ON GOGlobal.cheat",
			"FREE 14 DAY SPINBOT TRIAL @ GOGlobal.cheat",
			"VISIT GOGlobal.cheat TODAY TO DOWNLOAD BEST HACKS",
			"VAC UNDEDECTED SOFT AIM @ GOGlobal.cheat",
			"UNDETECTED AIMBOT & WALLHACKS @ GOGlobal.cheat",
			"EXCLUSIVE ESP & TRIGGERBOT @ GOGlobal.cheat",
			"LIMITED TIME OFFER: 60% DISCOUNT @ GOGlobal.cheat",
			"TRY OUR AIM ASSISTANCE TODAY @ GOGlobal.cheat",
			"INSTANT HEADSHOTS GUARANTEED @ GOGlobal.cheat",
			"UNDISTURBED BY VAC BANS @ GOGlobal.cheat",
			"EXPLORE OUR ESP FEATURES @ GOGlobal.cheat",
			"UNTRACEABLE AIMBOT TECHNOLOGY @ GOGlobal.cheat",
			"ENHANCE YOUR GAMING EXPERIENCE @ GOGlobal.cheat",
			"UNDISCOVERED CHEAT METHODS @ GOGlobal.cheat",
		};

		/// <inheritdoc cref="IStrategy.Name"/>
		public string Name => "Fake Cheats";

		/// <inheritdoc cref="IStrategy.Description"/>
		public string Description => "Spams fake cheat advertisements in the chat.";

		/// <inheritdoc cref="IStrategy.Running"/>
		public bool Running { get; private set; }

		private readonly System.Random random = new();

		private Timer? timer;

		/// <inheritdoc cref="IStrategy.Start"/>
		public bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (this.Running)
			{
				return false;
			}

			var players = Utilities.GetPlayers();

			this.timer = new Timer(FakeCheats.Interval, () =>
			{
				var player = players[this.random.Next(0, players.Count)];

				if (player.IsValid)
				{
					var message = FakeCheats.Messages[this.random.Next(FakeCheats.Messages.Length)];

					player.ExecuteClientCommandFromServer($"say {message}");
				}
			}, TimerFlags.REPEAT);

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

			this.timer?.Kill();

			return true;
		}
	}
}
