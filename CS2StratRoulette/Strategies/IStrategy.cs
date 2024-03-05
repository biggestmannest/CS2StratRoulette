namespace CS2StratRoulette.Strategies
{
	public interface IStrategy
	{
		/// <summary>
		/// Human readable strategy name
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Short description explaining the strategy
		/// </summary>
		public string Description { get; }

		/// <summary>
		/// Is the strategy active?
		/// </summary>
		protected bool Running { get; }

		/// <summary>
		/// Register required event listeners in order to enforce the strategy
		/// </summary>
		/// <param name="plugin">Reference to <see cref="CS2StratRoulettePlugin"/></param>
		/// <returns><see langword="true"/> if all events register successfully</returns>
		public bool Start(ref CS2StratRoulettePlugin plugin);

		/// <summary>
		/// Register required event listeners in order to enforce the strategy
		/// </summary>
		/// <param name="plugin">Reference to <see cref="CS2StratRoulettePlugin"/></param>
		/// <returns><see langword="true"/> if all events deregister successfully</returns>
		public bool Stop(ref CS2StratRoulettePlugin plugin);
	}
}
