using CS2StratRoulette.Constants;
using CS2StratRoulette.Helpers;
using CS2StratRoulette.Interfaces;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core;
using JetBrains.Annotations;

namespace CS2StratRoulette
{
	[UsedImplicitly(ImplicitUseTargetFlags.Members)]
	public abstract class Hooks : Commands
	{
		public override void Load(bool hotReload)
		{
			base.Load(hotReload);

			this.RegisterListener<Listeners.OnServerPrecacheResources>((manifest) =>
			{
				foreach (var model in Models.Props)
				{
					manifest.AddResource(model);
				}
			});
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
			if (this.ActiveStrategy is IStrategyPreStop strategy)
			{
				strategy.PreStop();
			}

			return HookResult.Continue;
		}
	}
}
