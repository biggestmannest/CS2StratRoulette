namespace CS2StratRoulette.Strategies
{
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

		/// <summary>
		/// Is the strategy active?
		/// </summary>
		public bool Running { get; private set; }

		/// <summary>
		/// Register required event listeners in order to enforce the strategy
		/// </summary>
		/// <param name="plugin">Reference to <see cref="CS2StratRoulettePlugin"/></param>
		/// <returns><see langword="true"/> if all events register successfully</returns>
		public virtual bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (this.Running)
			{
				return false;
			}

			this.Running = true;

			return true;
		}

		/// <summary>
		/// Register required event listeners in order to enforce the strategy
		/// </summary>
		/// <param name="plugin">Reference to <see cref="CS2StratRoulettePlugin"/></param>
		/// <returns><see langword="true"/> if all events deregister successfully</returns>
		public virtual bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!this.Running)
			{
				return false;
			}

			this.Running = false;

			return true;
		}
	}
}
