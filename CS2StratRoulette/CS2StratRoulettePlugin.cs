using JetBrains.Annotations;

namespace CS2StratRoulette
{
	[UsedImplicitly(ImplicitUseTargetFlags.Members)]
	// ReSharper disable once InconsistentNaming
	public sealed class CS2StratRoulettePlugin : Hooks
	{
		public override string ModuleName =>
			"CS2StratRoulette";

		public override string ModuleVersion =>
			"0.0.1";

		public override string ModuleAuthor =>
			"me, only me. i am the greatest! :)";

		public override void Load(bool hotReload)
		{
			base.Load(hotReload);

			System.Console.WriteLine("[CS2StratRoulettePlugin] Initialized");
		}

		public override void Unload(bool hotReload)
		{
			this.Strategies.Clear();

			this.StopActiveStrategy();
		}
	}
}
