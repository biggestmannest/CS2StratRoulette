using CounterStrikeSharp.API.Core;
using CS2StratRoulette.Constants;
using CS2StratRoulette.Managers;
using JetBrains.Annotations;

namespace CS2StratRoulette
{
	[UsedImplicitly(ImplicitUseTargetFlags.Members)]
	// ReSharper disable once InconsistentNaming
	public sealed partial class CS2StratRoulettePlugin : BasePlugin
	{
		// @todo MA0069
		public static CS2StratRoulettePlugin Instance = new();

		public override string ModuleName =>
			"CS2StratRoulette";

		public override string ModuleVersion =>
			"0.0.1";

		public override string ModuleAuthor =>
			"me, only me. i am the greatest! :)";

		public override void Load(bool hotReload)
		{
			CS2StratRoulettePlugin.Instance = this;

			base.Load(hotReload);

			this.RegisterListener<Listeners.OnServerPrecacheResources>((manifest) =>
			{
				foreach (var model in Models.Props)
				{
					manifest.AddResource(model);
				}
			});

			System.Console.WriteLine("[CS2StratRoulettePlugin] Initialized");
		}

		public override void Unload(bool hotReload)
		{
			StrategyManager.Unload();
		}
	}
}
