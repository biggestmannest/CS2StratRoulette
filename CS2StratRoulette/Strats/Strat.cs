using CounterStrikeSharp.API.Modules.Utils;

namespace CS2StratRoulette.Strats
{
	public abstract class Strat
	{
		public readonly string Name;

		protected Strat(ref CS2StratRoulette plugin, string name)
		{
			this.Name = name;

			System.Console.WriteLine("[Strat]: {0} initializing...", this.GetType().Name);

			CounterStrikeSharp.API.Server.PrintToChatAll(
				$"{ChatColors.Purple}[StratRoulette]:{ChatColors.Default} the chosen strategy for this round will be {ChatColors.Blue}{this.Name}"
			);
		}
	}
}
