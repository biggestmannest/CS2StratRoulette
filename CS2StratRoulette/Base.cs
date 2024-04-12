using CS2StratRoulette.Strategies;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace CS2StratRoulette
{
	public abstract class Base : BasePlugin
	{
		public readonly List<System.Type> Strategies = new(50);
		public required Strategy? ActiveStrategy;

		private readonly StringBuilder builder = new();

		public override void Load(bool hotReload)
		{
			var types = typeof(Strategy).Assembly.GetTypes();

			foreach (var type in types)
			{
				if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(Strategy)))
				{
					this.Strategies.Add(type);
				}
			}
		}

		public void CycleStrategy()
		{
			// We stop the strategy again just in case it didn't stop before.
			this.StopActiveStrategy();

			if (this.Strategies.Count == 0)
			{
				System.Console.WriteLine("[CycleStrategy]: there no strategies");

				return;
			}

			var idx = System.Random.Shared.Next(this.Strategies.Count);
			this.SetActiveStrategy(this.Strategies[idx]);
		}

		public bool StartActiveStrategy()
		{
			if (this.ActiveStrategy is null)
			{
				System.Console.WriteLine("[StartActiveStrategy]: ActiveStrategy is null");

				return false;
			}

			var plugin = this;
			var result = this.ActiveStrategy.Start(ref plugin);

			if (!result)
			{
				System.Console.WriteLine(
					"[StartActiveStrategy]: failed starting {0}",
					this.ActiveStrategy.GetType().Name
				);
			}

			return result;
		}

		public bool StopActiveStrategy()
		{
			if (this.ActiveStrategy is null)
			{
				return true;
			}

			var plugin = this;
			var result = this.ActiveStrategy.Stop(ref plugin);

			if (!result && !this.ActiveStrategy.Running)
			{
				System.Console.WriteLine(
					"[StopActiveStrategy]: failed stopping {0}",
					this.ActiveStrategy.GetType().Name
				);
			}

			return result;
		}

		public bool SetActiveStrategy(System.Type type)
		{
			// Try to invoke a random chosen strategy
			if (!Base.TryInvokeStrategy(type, out var strategy))
			{
				// If it fails don't use a strategy for this round and pretend as if nothing happened :)
				this.ActiveStrategy = null;

				System.Console.WriteLine("[CycleStrategy]: failed invoking {0} strategy", type.Name);

				return false;
			}

			this.ActiveStrategy = strategy;

			System.Console.WriteLine("[CycleStrategy]: picked {0}", strategy.Name);

			return true;
		}

		/// <summary>
		/// Try to invoke a class of subtype <see cref="Strategy"/>
		/// </summary>
		/// <param name="type">The class inheriting from <see cref="Strategy"/></param>
		/// <param name="strategy">Only <see langword="null"/> if invoking failed</param>
		/// <returns><see langword="false"/> when invoking failed and <see langword="true"/> when successful</returns>
		/// <example>
		/// <code>
		/// if (!this.TryInvokeStrat(type, out var strat)) {
		///		// invoking failed, strat will be null
		///		// exit or continue...
		/// }
		/// // invoking worked, you can use strat
		/// </code>
		/// </example>
		public static bool TryInvokeStrategy(System.Type type, [NotNullWhen(true)] out Strategy? strategy)
		{
			strategy = null;

			try
			{
				var @object = System.Activator.CreateInstance(type);

				if (@object is Strategy strat2)
				{
					strategy = strat2;

					return true;
				}
			}
			catch (System.Exception e)
			{
				System.Console.Write(e);
			}

			System.Console.WriteLine("[TryInvokeStrategy]: failed invoking Strategy");

			return false;
		}

		/// <summary>
		/// Announces a strategy in global chat
		/// </summary>
		public void AnnounceStrategy(Strategy strategy)
		{
			const char newLine = '\u2029';

			this.builder.Clear();

			this.builder.Append(' ');
			this.builder.Append(ChatColors.Blue);
			this.builder.Append('-', 80);
			this.builder.Append(newLine);
			this.builder.Append(ChatColors.Green);
			this.builder.Append(strategy.Name);
			this.builder.Append(newLine);
			this.builder.Append(ChatColors.Silver);
			this.builder.Append(strategy.Description);
			this.builder.Append(newLine);
			this.builder.Append(ChatColors.Blue);
			this.builder.Append('-', 80);

			Server.PrintToChatAll(this.builder.ToString());

			this.builder.Clear();
		}
	}
}
