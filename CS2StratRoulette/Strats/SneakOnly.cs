using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strats
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public class SneakOnly : Strat
	{
		public SneakOnly(ref CS2StratRoulette plugin) : base(ref plugin, "Sneak Only")
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
