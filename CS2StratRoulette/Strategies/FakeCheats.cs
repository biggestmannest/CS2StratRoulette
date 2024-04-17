using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;
using CS2StratRoulette.Enums;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class FakeCheats : Strategy
	{
		private const float Interval = 1.0f;

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

		public override string Name =>
			"Fake Cheats";

		public override string Description =>
			"Spams fake cheat advertisements in the chat.";

		public override StrategyFlags Flags { get; protected set; } = StrategyFlags.Hidden;

		private readonly System.Random random = new();

		private Timer? timer;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			var players = Utilities.GetPlayers();

			this.timer = new Timer(FakeCheats.Interval, () =>
			{
				if (this.random.Next(2) == 0)
				{
					return;
				}

				var player = players[this.random.Next(players.Count)];

				if (!player.IsValid)
				{
					return;
				}

				var message = FakeCheats.Messages[this.random.Next(FakeCheats.Messages.Length)];

				player.ExecuteClientCommandFromServer($"say {message}");
			}, TimerFlags.REPEAT);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			this.timer?.Kill();

			return true;
		}
	}
}
