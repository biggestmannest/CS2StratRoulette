using System.Collections.Generic;
using CounterStrikeSharp.API.Core;
using CS2StratRoulette.Strats;
using JetBrains.Annotations;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette
{
	[UsedImplicitly(ImplicitUseTargetFlags.Members)]
	// ReSharper disable once InconsistentNaming
	public sealed class CS2StratRoulette : BasePlugin
	{
		public override string ModuleName => "CS2StratRoulette";

		public override string ModuleVersion => "0.0.1";

		public required List<System.Type> Strats = new();

		public required Strat? ActiveStrat;

		public override void Load(bool hotReload)
		{
			foreach (var strat in typeof(Strat).Assembly.GetTypes())
			{
				if (strat.IsClass && !strat.IsAbstract && strat.IsSubclassOf(typeof(Strat)))
				{
					this.Strats.Add(strat);
				}
			}
		}

		public bool TryInstantiateStrat(System.Type type, [NotNullWhen(true)] out Strat? strat)
		{
			strat = null;

			var plugin = this;
			var @object = System.Activator.CreateInstance(type, new[] { plugin });

			if (@object is not Strat strat2)
			{
				System.Console.WriteLine("[TryInstantiateStrat]: failed creating Strat instance");

				return false;
			}

			strat = strat2;

			return true;
		}

		[GameEventHandler]
		public HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
		{
			var idx = System.Random.Shared.Next(0, this.Strats.Count);

			if (!this.TryInstantiateStrat(this.Strats[idx], out var strat))
			{
				this.ActiveStrat = null;

				System.Console.WriteLine("[OnRoundStart]: failed instantiating strat");

				return HookResult.Continue;
			}

			this.ActiveStrat = strat;

			System.Console.WriteLine("[OnRoundStart]: picked {0}", strat.Name);

			return HookResult.Continue;
		}
	}
}
