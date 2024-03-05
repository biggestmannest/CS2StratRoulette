using CS2StratRoulette.Strategies;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette
{
	[UsedImplicitly(ImplicitUseTargetFlags.Members)]
	// ReSharper disable once InconsistentNaming
	public sealed class CS2StratRoulette : BasePlugin
	{
		public override string ModuleName => "CS2StratRoulette";
		public override string ModuleVersion => "0.0.1";
		public override string ModuleAuthor => "the guys";

		public required List<System.Type> Strategies = new();
		public required Strategy? ActiveStrategy;

		/// <summary>
		/// Main entry point of plugin
		/// Get all classes of type <see cref="Strategy"/> and store for later use
		/// </summary>
		/// <param name="hotReload"></param>
		public override void Load(bool hotReload)
		{
			foreach (var strat in typeof(Strategy).Assembly.GetTypes())
			{
				if (strat.IsClass && !strat.IsAbstract && strat.IsSubclassOf(typeof(Strategy)))
				{
					this.Strategies.Add(strat);
				}
			}
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
		public bool TryInvokeStrategy(System.Type type, [NotNullWhen(true)] out Strategy? strategy)
		{
			var plugin = this;

			strategy = null;

			try
			{
				var @object = System.Activator.CreateInstance(type, new object?[] { plugin });

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

		[GameEventHandler]
		public HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
		{
			var idx = System.Random.Shared.Next(0, this.Strategies.Count);

			// Try to invoke a random chosen strategy
			if (!this.TryInvokeStrategy(this.Strategies[idx], out var strategy))
			{
				// If it fails don't use a strategy for this round and pretend as if nothing happened :)
				this.ActiveStrategy = null;

				System.Console.WriteLine("[OnRoundStart]: failed invoking strategy");

				return HookResult.Continue;
			}

			this.ActiveStrategy = strategy;

			CS2StratRoulette.Announce(this.ActiveStrategy);

			System.Console.WriteLine("[OnRoundStart]: picked {0}", strategy.Name);

			return HookResult.Continue;
		}

		/// <summary>
		/// Announces a strategy in global chat
		/// </summary>
		private static void Announce(Strategy strategy)
		{
			CounterStrikeSharp.API.Server.PrintToChatAll(
				$"{ChatColors.Red}[StratRoulette]:{ChatColors.Default} the chosen strategy for this round will be {ChatColors.Blue}{strategy.Name}"
			);

			CounterStrikeSharp.API.Server.PrintToChatAll(strategy.Description);
		}
	}
}
