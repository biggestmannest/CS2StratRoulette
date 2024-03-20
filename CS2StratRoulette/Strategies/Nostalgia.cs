using System.Collections.Generic;
using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Timers;
using CS2StratRoulette.Enums;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Nostalgia : Strategy
	{
		public override string Name =>
			"Nostalgia";

		public override string Description =>
			":')";

		public override StrategyFlags Flags { get; protected set; } = StrategyFlags.Hidden;

		private Timer? timer;

		private readonly System.Random random = new();

		private readonly Dictionary<int, string> song1 = new();

		private readonly Dictionary<int, string> song2 = new();

		private Dictionary<int, string> randomSong;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			//EZ4Ence
			this.song1.Add(1, "sounds/sfx/ence_roundstart");
			this.song1.Add(2, "sounds/sfx/ence_actionstart");
			this.song1.Add(3, "sounds/sfx/encething");
			//Flashbang Dance
			this.song2.Add(1, "sounds/music/flashbang_roundstart");
			this.song2.Add(2, "sounds/music/flashbang_actionstart");
			this.song2.Add(3, "sounds/music/flashbang_bombplanted");

			var randomNum = this.random.Next(2);
			this.randomSong = randomNum == 0 ? this.song1 : this.song2;

			foreach (var player in Utilities.GetPlayers())
			{
				player.ExecuteClientCommand($"play {this.randomSong[1]}");
				this.timer = new Timer(17.0f,
					() => { player.ExecuteClientCommand($"play {this.randomSong[2]}"); });
			}

			plugin.RegisterEventHandler<EventBombPlanted>(this.OnBombPlanted);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			this.timer?.Kill();

			const string bombPlanted = "bomb_planted";

			plugin.DeregisterEventHandler(bombPlanted, this.OnBombPlanted, true);

			return true;
		}

		private HookResult OnBombPlanted(EventBombPlanted @event, GameEventInfo _)
		{
			if (!this.Running)
			{
				return HookResult.Continue;
			}

			foreach (var players in Utilities.GetPlayers())
			{
				players.ExecuteClientCommand($"play {this.randomSong[3]}");
			}


			return HookResult.Continue;
		}
	}
}
