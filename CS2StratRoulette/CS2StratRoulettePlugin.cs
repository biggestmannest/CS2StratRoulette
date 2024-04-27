using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CS2StratRoulette.Constants;
using CS2StratRoulette.Managers;
using JetBrains.Annotations;

namespace CS2StratRoulette
{
	[UsedImplicitly(ImplicitUseTargetFlags.Members)]
	[MinimumApiVersion(217)]
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
			"Snake, bigman & wsrvn (i guess)";

		public bool Active { get; private set; } = true;

		public override void Load(bool hotReload)
		{
			CS2StratRoulettePlugin.Instance = this;

			if (!hotReload)
			{
				this.RegisterListener<Listeners.OnServerPrecacheResources>((manifest) =>
				{
					foreach (var model in Models.Props)
					{
						manifest.AddResource(model);
					}

					foreach (var model2 in Models.Props2)
					{
						manifest.AddResource(model2);
					}
				});
			}
			else
			{
				foreach (var model in Models.Props)
				{
					Server.PrecacheModel(model);
				}

				foreach (var model2 in Models.Props2)
				{
					Server.PrecacheModel(model2);
				}
			}

			StrategyManager.Load();

			System.Console.WriteLine("[CS2StratRoulettePlugin] Initialized");
		}

		public override void Unload(bool hotReload)
		{
			StrategyManager.Unload();
		}
	}
}
