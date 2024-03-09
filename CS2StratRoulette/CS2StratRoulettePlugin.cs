using CS2StratRoulette.Interfaces;
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
		public required Strategy? ActiveStrategy;

		/// <summary>
		/// Main entry point of plugin
		/// Get all classes of type <see cref="Strategy"/> and store for later use
		/// </summary>
		/// <param name="hotReload"></param>
		public override void Load(bool hotReload)
		{
			var types = typeof(Strategy).Assembly.GetTypes();

			foreach (var type in types)
			{
				if (type.IsClass || !type.IsAbstract || type.IsSubclassOf(typeof(Strategy)))
				{
					this.Strategies.Add(type);
				}
			}
		}

		/// <summary>
		/// Get all classes of type <see cref="Strategy"/> and store for later use
		/// </summary>
		/// <param name="hotReload"></param>
		public override void Unload(bool hotReload)
		{
			this.Strategies.Clear();

			this.StopActiveStrategy();
		}

		[GameEventHandler]
		public HookResult OnRoundStart(EventRoundStart _, GameEventInfo _2)
		{
			if (this.ActiveStrategy is IStrategyPostStop strategy)
			{
				strategy.PostStop();
			}

			this.CycleStrategy();

			if (this.StartActiveStrategy() && this.ActiveStrategy is not null)
			{
				this.AnnounceStrategy(this.ActiveStrategy);
			}

			return HookResult.Continue;
		}

		[GameEventHandler]
		public HookResult OnRoundEnd(EventRoundEnd _, GameEventInfo _2)
		{
			this.StopActiveStrategy();

			return HookResult.Continue;
		}

		[GameEventHandler(HookMode.Pre)]
		public HookResult OnRoundEndPre(EventRoundEnd _, GameEventInfo _2)
		{
			if (this.ActiveStrategy is not null && this.ActiveStrategy is IStrategyPreStop strategy)
			{
				strategy.PreStop();
			}

			return HookResult.Continue;
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

			var idx = System.Random.Shared.Next(0, this.Strategies.Count);
			var type = this.Strategies[idx];

			// Try to invoke a random chosen strategy
			if (!this.TryInvokeStrategy(type, out var strategy))
			{
				// If it fails don't use a strategy for this round and pretend as if nothing happened :)
				this.ActiveStrategy = null;

				System.Console.WriteLine("[CycleStrategy]: failed invoking {0} strategy", type.Name);

				return;
			}

			this.ActiveStrategy = strategy;

			System.Console.WriteLine("[CycleStrategy]: picked {0}", strategy.Name);
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

			if (!result)
			{
				System.Console.WriteLine(
					"[StopActiveStrategy]: failed stopping {0}",
					this.ActiveStrategy.GetType().Name
				);
			}

			return result;
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
		private void AnnounceStrategy(Strategy strategy)
		{
			CounterStrikeSharp.API.Server.PrintToChatAll(
				$"{ChatColors.Red}[StratRoulette]:{ChatColors.Default} the chosen strategy for this round will be {ChatColors.Blue}{strategy.Name}"
			);

			CounterStrikeSharp.API.Server.PrintToChatAll(strategy.Description);
		}
	}
}
