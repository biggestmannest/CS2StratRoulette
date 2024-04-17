using CS2StratRoulette.Interfaces;
using CS2StratRoulette.Strategies;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace CS2StratRoulette.Managers
{
	public static class StrategyManager
	{
		private static readonly List<System.Type> strategies = new(50);
		private static readonly StringBuilder builder = new();

		public static Strategy? ActiveStrategy;

		public static bool Running =>
			(StrategyManager.ActiveStrategy is not null && StrategyManager.ActiveStrategy.Running);

		public static void Load()
		{
			var types = typeof(Strategy).Assembly.GetTypes();

			foreach (var type in types)
			{
				if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(Strategy)))
				{
					StrategyManager.strategies.Add(type);
				}
			}
		}

		public static void Unload()
		{
			StrategyManager.StopActiveStrategy();

			StrategyManager.strategies.Clear();
		}

		public static void CycleStrategy()
		{
			if (StrategyManager.strategies.Count == 0)
			{
				System.Console.WriteLine("[CycleStrategy]: there no strategies");

				return;
			}

			var idx = System.Random.Shared.Next(StrategyManager.strategies.Count);

			StrategyManager.SetActiveStrategy(StrategyManager.strategies[idx]);
		}

		public static bool StartActiveStrategy()
		{
			if (StrategyManager.ActiveStrategy is null)
			{
				System.Console.WriteLine("[StartActiveStrategy]: ActiveStrategy is null");

				return false;
			}

			var plugin = CS2StratRoulettePlugin.Instance;
			var result = StrategyManager.ActiveStrategy.Start(ref plugin);

			if (!result)
			{
				System.Console.WriteLine(
					"[StartActiveStrategy]: failed starting {0}",
					StrategyManager.ActiveStrategy.GetType().Name
				);
			}

			StrategyManager.AnnounceStrategy(StrategyManager.ActiveStrategy);

			return result;
		}

		public static bool StopActiveStrategy()
		{
			if (StrategyManager.ActiveStrategy is null)
			{
				return true;
			}

			if (StrategyManager.ActiveStrategy is IStrategyPreStop preStrat)
			{
				preStrat.PreStop();
			}

			var plugin = CS2StratRoulettePlugin.Instance;
			var result = StrategyManager.ActiveStrategy.Stop(ref plugin);

			if (StrategyManager.ActiveStrategy is IStrategyPostStop postStrat)
			{
				postStrat.PostStop();
			}

			if (!result && !StrategyManager.ActiveStrategy.Running)
			{
				System.Console.WriteLine(
					"[StopActiveStrategy]: failed stopping {0}",
					StrategyManager.ActiveStrategy.GetType().Name
				);
			}

			return result;
		}

		public static bool SetActiveStrategy(string name)
		{
			foreach (var strategy in StrategyManager.strategies)
			{
				if (strategy.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase))
				{
					return StrategyManager.SetActiveStrategy(strategy);
				}
			}

			return false;
		}

		public static bool SetActiveStrategy(System.Type type)
		{
			StrategyManager.StopActiveStrategy();

			// Try to invoke a random chosen strategy
			if (!StrategyManager.TryInvokeStrategy(type, out var strategy))
			{
				// If it fails don't use a strategy for this round and pretend as if nothing happened :)
				StrategyManager.ActiveStrategy = null;

				System.Console.WriteLine("[CycleStrategy]: failed invoking {0} strategy", type.Name);

				return false;
			}

			StrategyManager.ActiveStrategy = strategy;

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
		/// if (!StrategyManager.TryInvokeStrat(type, out var strat)) {
		///		// invoking failed, strat will be null
		///		// exit or continue...
		/// }
		/// // invoking worked, you can use strat
		/// </code>
		/// </example>
		private static bool TryInvokeStrategy(System.Type type, [NotNullWhen(true)] out Strategy? strategy)
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
		public static void AnnounceStrategy(Strategy strategy)
		{
			const char newLine = '\u2029';

			StrategyManager.builder.Clear();

			StrategyManager.builder.Append(' ');
			StrategyManager.builder.Append(ChatColors.Blue);
			StrategyManager.builder.Append('-', 80);
			StrategyManager.builder.Append(newLine);
			StrategyManager.builder.Append(ChatColors.Green);
			StrategyManager.builder.Append(strategy.Name);
			StrategyManager.builder.Append(newLine);
			StrategyManager.builder.Append(ChatColors.Silver);
			StrategyManager.builder.Append(strategy.Description);
			StrategyManager.builder.Append(newLine);
			StrategyManager.builder.Append(ChatColors.Blue);
			StrategyManager.builder.Append('-', 80);

			Server.PrintToChatAll(StrategyManager.builder.ToString());

			StrategyManager.builder.Clear();
		}
	}
}
