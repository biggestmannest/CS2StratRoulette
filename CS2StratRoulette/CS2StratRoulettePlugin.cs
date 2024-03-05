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
	public sealed class CS2StratRoulettePlugin : BasePlugin
	{
		public override string ModuleName => "CS2StratRoulette";
		public override string ModuleVersion => "0.0.1";
		public override string ModuleAuthor => "the guys";

		public required List<System.Type> Strategies = new();
		public required IStrategy? ActiveStrategy;

		/// <summary>
		/// Main entry point of plugin
		/// Get all classes of type <see cref="IStrategy"/> and store for later use
		/// </summary>
		/// <param name="hotReload"></param>
		public override void Load(bool hotReload)
		{
			this.Strategies.Clear();

			if (this.ActiveStrategy is not null)
			{
				var plugin = this;

				this.ActiveStrategy.Stop(ref plugin);

				this.ActiveStrategy = null;
			}

			foreach (var strat in typeof(IStrategy).Assembly.GetTypes())
			{
				if (strat.IsClass && !strat.IsAbstract && strat.IsSubclassOf(typeof(IStrategy)))
				{
					this.Strategies.Add(strat);
				}
			}
		}

		public void CycleStrategy()
		{
			var idx = System.Random.Shared.Next(0, this.Strategies.Count);

			this.StopActiveStrategy();

			// Try to invoke a random chosen strategy
			if (!this.TryInvokeStrategy(this.Strategies[idx], out var strategy))
			{
				// If it fails don't use a strategy for this round and pretend as if nothing happened :)
				this.ActiveStrategy = null;

				System.Console.WriteLine("[CycleStrategy]: failed invoking strategy");

				return;
			}

			this.ActiveStrategy = strategy;

			if (!this.StartActiveStrategy())
			{
				return;
			}

			this.AnnounceStrategy(this.ActiveStrategy);
			System.Console.WriteLine("[CycleStrategy]: picked {0}", strategy.Name);
		}

		public bool StartActiveStrategy()
		{
			if (this.ActiveStrategy is null)
			{
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

			if (!result)
			{
				System.Console.WriteLine(
					"[StopActiveStrategy]: failed stopping {0}",
					this.ActiveStrategy.GetType().Name
				);
			}

			return result;
		}

		[GameEventHandler]
		public HookResult OnRoundStart(EventRoundAnnounceMatchStart @event, GameEventInfo _2)
		{
			this.CycleStrategy();

			return HookResult.Continue;
		}

		/// <summary>
		/// Try to invoke a class of subtype <see cref="IStrategy"/>
		/// </summary>
		/// <param name="type">The class inheriting from <see cref="IStrategy"/></param>
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
		public bool TryInvokeStrategy(System.Type type, [NotNullWhen(true)] out IStrategy? strategy)
		{
			strategy = null;

			try
			{
				var @object = System.Activator.CreateInstance(type);

				if (@object is IStrategy strat2)
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
		private void AnnounceStrategy(IStrategy strategy)
		{
			CounterStrikeSharp.API.Server.PrintToChatAll(
				$"{ChatColors.Red}[StratRoulette]:{ChatColors.Default} the chosen strategy for this round will be {ChatColors.Blue}{strategy.Name}"
			);

			CounterStrikeSharp.API.Server.PrintToChatAll(strategy.Description);
		}
	}
}
