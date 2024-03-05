using CounterStrikeSharp.API.Modules.Utils;

namespace CS2StratRoulette.Strategies
{
	// TODO: Add a state to discern whether the strategy should be enforced
	public abstract class Strategy
	{
		/// <summary>
		/// Human readable strategy name
		/// </summary>
		public abstract string Name { get; }

		/// <summary>
		/// Short description explaining the strategy
		/// </summary>
		public abstract string Description { get; }

		/// <param name="plugin">Reference to <see cref="CS2StratRoulette"/></param>
		protected Strategy(ref CS2StratRoulette plugin)
		{
			System.Console.WriteLine("[Strategy]: {0} initializing...", this.GetType().Name);
		}
	}
}
