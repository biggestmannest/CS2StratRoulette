using CS2StratRoulette.Extensions;
using CS2StratRoulette.Interfaces;
using CS2StratRoulette.Strategies;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using CS2StratRoulette.Enums;

namespace CS2StratRoulette.Managers
{
	public static class StrategyManager
	{
		private const string Prefix = "[CS2StratRoulette::StrategyManager]";

		private static readonly List<System.Type> strategies = new(50);
		private static readonly StringBuilder builder = new();

		private static int index;
		private static Strategy? activeStrategy;

		public static string Name =>
			(StrategyManager.activeStrategy is null)
				? string.Empty
				: StrategyManager.activeStrategy.GetType().Name;

		public static bool Running =>
			(StrategyManager.activeStrategy is not null && StrategyManager.activeStrategy.Running);

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

			StrategyManager.Shuffle();

			System.Console.WriteLine(
				$"{StrategyManager.Prefix} Loaded {StrategyManager.strategies.Count} strategies"
			);
		}

		public static void Unload()
		{
			StrategyManager.Kill();

			StrategyManager.strategies.Clear();
		}

		public static void Shuffle()
		{
			if (StrategyManager.strategies.Count > 2)
			{
				System.Random.Shared.Shuffle(StrategyManager.strategies);
			}

			System.Console.WriteLine($"{StrategyManager.Prefix} Shuffled");
		}

		public static void Next()
		{
			if (StrategyManager.strategies.Count == 0)
			{
				System.Console.WriteLine($"{StrategyManager.Prefix} No strategies");

				return;
			}

			if (StrategyManager.index >= StrategyManager.strategies.Count)
			{
				StrategyManager.Shuffle();
				StrategyManager.index = 0;
			}

			StrategyManager.SetActiveStrategy(StrategyManager.strategies[StrategyManager.index++]);

			if (StrategyManager.activeStrategy is not null && !StrategyManager.activeStrategy.CanRun())
			{
				System.Console.WriteLine(
					$"{StrategyManager.Prefix} Can't run {StrategyManager.Name} in current round"
				);

				StrategyManager.Next();
			}
		}

		public static bool Start()
		{
			if (StrategyManager.activeStrategy is null || StrategyManager.Running)
			{
				return false;
			}

			var plugin = CS2StratRoulettePlugin.Instance;
			var name = StrategyManager.Name;

			var result = StrategyManager.activeStrategy.Start(ref plugin);

			System.Console.WriteLine(
				result
					? $"{StrategyManager.Prefix} Started {name}"
					: $"{StrategyManager.Prefix} Failed starting {name}"
			);

			return true;
		}

		public static bool Stop()
		{
			if (StrategyManager.activeStrategy is null || !StrategyManager.Running)
			{
				return false;
			}

			var plugin = CS2StratRoulettePlugin.Instance;
			var name = StrategyManager.Name;

			var result = StrategyManager.activeStrategy.Stop(ref plugin);

			System.Console.WriteLine(
				result
					? $"{StrategyManager.Prefix} Stopped {name}"
					: $"{StrategyManager.Prefix} Failed stopping {name}"
			);

			return result;
		}

		public static void PostStop()
		{
			if (StrategyManager.activeStrategy is IStrategyPostStop strategy)
			{
				strategy.PostStop();
			}
		}

		public static bool Kill()
		{
			var result = StrategyManager.Stop();

			StrategyManager.PostStop();

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
			StrategyManager.Kill();

			// Try to invoke a random chosen strategy
			if (!StrategyManager.TryInvokeStrategy(type, out var strategy))
			{
				// If it fails don't use a strategy for this round and pretend as if nothing happened :)
				StrategyManager.activeStrategy = null;

				System.Console.WriteLine($"{StrategyManager.Prefix} Failed invoking {type.Name} strategy");

				return false;
			}

			StrategyManager.activeStrategy = strategy;

			System.Console.WriteLine($"{StrategyManager.Prefix} Set {StrategyManager.Name}");

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

			System.Console.WriteLine($"{StrategyManager.Prefix} Failed invoking Strategy");

			return false;
		}

		/// <summary>
		/// Announces the current strategy in global chat
		/// </summary>
		public static void Announce()
		{
			if (StrategyManager.activeStrategy is null)
			{
				return;
			}

			var name = StrategyManager.activeStrategy.Name;
			var description = StrategyManager.activeStrategy.Description;

			if (!StrategyManager.activeStrategy.Flags.HasFlag(StrategyFlags.AlwaysVisible) &&
				System.Random.Shared.Next(10) < 3) // 40% to be hidden
			{
				const string hiddenTitle = "Hidden.";
				const string hiddenDescription = "Good luck :)";

				name = hiddenTitle;
				description = hiddenDescription;
			}

			const char newLine = '\u2029';

			StrategyManager.builder.Clear();

			StrategyManager.builder.Append(' ');
			StrategyManager.builder.Append(ChatColors.Blue);
			StrategyManager.builder.Append('-', 80);
			StrategyManager.builder.Append(newLine);
			StrategyManager.builder.Append(ChatColors.Green);
			StrategyManager.builder.Append(name);
			StrategyManager.builder.Append(newLine);
			StrategyManager.builder.Append(ChatColors.Silver);
			StrategyManager.builder.Append(description);
			StrategyManager.builder.Append(newLine);
			StrategyManager.builder.Append(ChatColors.Blue);
			StrategyManager.builder.Append('-', 80);

			Server.PrintToChatAll(StrategyManager.builder.ToString());

			StrategyManager.builder.Clear();
		}
	}
}
