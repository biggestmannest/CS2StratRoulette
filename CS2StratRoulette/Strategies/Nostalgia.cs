using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Nostalgia : Strategy
	{
		public override string Name =>
			"Nostalgia";

		public override string Description =>
			"Plant and find out ;) (highly recommended that you type \"snd_toolvolume 0.05\" in console).";

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			foreach (var players in Utilities.GetPlayers())
			{
				players.ExecuteClientCommand("snd_toolvolume 0.3");
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
				players.ExecuteClientCommand("play sounds/music/theverkkars_01/bombplanted");
			}


			return HookResult.Continue;
		}
	}
}
