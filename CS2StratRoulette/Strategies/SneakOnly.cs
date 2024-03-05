using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public class SneakOnly : Strategy
	{
		/// <inheritdoc cref="Strategy.Name"/>
		public override string Name => "Sneak Only";

		/// <inheritdoc cref="Strategy.Description"/>
		public override string Description => "You're not allowed to run. Any footstep noises will kill you.";

		/// <summary>
		/// Register required event listeners in order to enforce the strategy
		/// TODO: dispose of the event listeners!
		/// </summary>
		/// <param name="plugin">Reference to <see cref="CS2StratRoulettePlugin"/></param>
		public SneakOnly(ref CS2StratRoulettePlugin plugin) : base(ref plugin)
		{
			plugin.RegisterEventHandler<EventPlayerSound>((@event, _) =>
			{
				if (@event.Userid.IsValid && @event.Step)
				{
					@event.Userid.CommitSuicide(false, true);
				}

				return HookResult.Continue;
			});
		}
	}
}
