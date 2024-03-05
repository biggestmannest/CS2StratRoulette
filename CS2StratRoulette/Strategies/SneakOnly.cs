using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public class SneakOnly : IStrategy
	{
		/// <inheritdoc cref="IStrategy.Name"/>
		public string Name => "Sneak Only";

		/// <inheritdoc cref="IStrategy.Description"/>
		public string Description => "You're not allowed to run. Any footstep noises will kill you.";

		/// <inheritdoc cref="IStrategy.Running"/>
		public bool Running { get; private set; }

		/// <inheritdoc cref="IStrategy.Start"/>
		public bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (this.Running)
			{
				return false;
			}

			plugin.RegisterEventHandler<EventPlayerSound>(this.OnPlayerSound);

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

			const string playerSound = "player_sound";

			plugin.DeregisterEventHandler(playerSound, this.OnPlayerSound, true);

			return true;
		}

		private HookResult OnPlayerSound(EventPlayerSound @event, GameEventInfo _)
		{
			if (!this.Running)
			{
				return HookResult.Continue;
			}

			if (@event.Userid.IsValid && @event.Step)
			{
				@event.Userid.CommitSuicide(false, true);
			}

			return HookResult.Continue;
		}
	}
}
