using CS2StratRoulette.Interfaces;
using CS2StratRoulette.Strategies;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using CS2StratRoulette.Constants;
using CS2StratRoulette.Extensions;
using CS2StratRoulette.Helpers;

namespace CS2StratRoulette
{
	[UsedImplicitly(ImplicitUseTargetFlags.Members)]
	// ReSharper disable once InconsistentNaming
	public sealed class CS2StratRoulettePlugin : BasePlugin
	{
		private const char NewLine = '\u2029';

		public override string ModuleName => "CS2StratRoulette";
		public override string ModuleVersion => "0.0.1";
		public override string ModuleAuthor => "the guys";

		public required List<System.Type> Strategies = new();
		public required Strategy? ActiveStrategy;

		private readonly StringBuilder builder = new();

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
				if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(Strategy)))
				{
					this.Strategies.Add(type);
				}
			}

			this.RegisterListener<Listeners.OnServerPrecacheResources>((manifest) =>
			{
				foreach (var model in Models.Props)
				{
					manifest.AddResource(model);
				}
			});
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
			var rules = Game.Rules();

			if (rules is not null && rules.WarmupPeriod)
			{
				return HookResult.Continue;
			}

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

		[ConsoleCommand("set_strat", "Sets the active strategy")]
		[CommandHelper(1, "[strat]")]
		[RequiresPermissions("@css/root")]
		public void OnStratCommand(CCSPlayerController? player, CommandInfo commandInfo)
		{
			var name = commandInfo.GetArg(1);

			this.StopActiveStrategy();

			foreach (var strategy in this.Strategies)
			{
				if (!string.Equals(strategy.Name, name, System.StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}

				if (!this.SetActiveStrategy(strategy))
				{
					commandInfo.ReplyToCommand($"[OnStratCommand] failed setting {strategy.Name} as active strategy");

					return;
				}

				if (!this.StartActiveStrategy())
				{
					commandInfo.ReplyToCommand("[OnStratCommand] failed starting strategy");

					return;
				}

				commandInfo.ReplyToCommand($"[OnStratCommand] set active strategy to {strategy.Name}");

				if (this.ActiveStrategy is not null)
				{
					this.AnnounceStrategy(this.ActiveStrategy);
				}

				return;
			}

			commandInfo.ReplyToCommand("[OnStratCommand] strategy not found");
		}

		[ConsoleCommand("props", "")]
		[CommandHelper(1, "[type]")]
		[RequiresPermissions("@css/root")]
		public void OnPropsCommand(CCSPlayerController? player, CommandInfo commandInfo)
		{
			var name = commandInfo.GetArg(1);

			foreach (var entity in Utilities.GetAllEntities())
			{
				if (entity.DesignerName.Contains("fence", System.StringComparison.OrdinalIgnoreCase) ||
					entity.DesignerName.Contains("gate", System.StringComparison.OrdinalIgnoreCase))
				{
					Server.ExecuteCommand($"say {entity.DesignerName}");
				}
			}

			foreach (var entity in Utilities.FindAllEntitiesByDesignerName<CEntityInstance>(name))
			{
				var prop = new CEntityInstance(entity.Handle);

				var model = Constants.Signatures.GetModel.Invoke(prop.Handle);
				Server.ExecuteCommand($"say {model}");
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

		private bool SetActiveStrategy(System.Type type)
		{
			// Try to invoke a random chosen strategy
			if (!this.TryInvokeStrategy(type, out var strategy))
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
			this.builder.Clear();

			this.builder.Append(' ');
			this.builder.Append(ChatColors.Blue);
			this.builder.Append('-', 80);
			this.builder.Append(CS2StratRoulettePlugin.NewLine);
			this.builder.Append(ChatColors.Green);
			this.builder.Append(strategy.Name);
			this.builder.Append(CS2StratRoulettePlugin.NewLine);
			this.builder.Append(ChatColors.Silver);
			this.builder.Append(strategy.Description);
			this.builder.Append(CS2StratRoulettePlugin.NewLine);
			this.builder.Append(ChatColors.Blue);
			this.builder.Append('-', 80);

			Server.PrintToChatAll(this.builder.ToString());

			this.builder.Clear();
		}
	}
}
