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

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			foreach (var player in Utilities.GetPlayers())
			{
				player.ExecuteClientCommand("play sounds/sfx/ence_roundstart");
				this.timer = new Timer(17.0f,
					() => { player.ExecuteClientCommand("play sounds/sfx/ence_actionstart"); });
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

			this.timer.Kill();

			Server.NextFrame(() => { Server.ExecuteCommand("sv_cheats 0"); });

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
				players.ExecuteClientCommand("play sounds/sfx/encething");
			}


			return HookResult.Continue;
		}
	}
}
