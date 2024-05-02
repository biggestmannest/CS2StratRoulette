using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API.Modules.Entities.Constants;

namespace CS2StratRoulette.Constants
{
	[SuppressMessage("Design", "MA0048")]
	public static class RetakeBuys
	{
		public static readonly RetakeWeapons CT = new(
			[CsItem.USP, CsItem.Elite, CsItem.FiveSeven, CsItem.Deagle],
			[CsItem.Famas, CsItem.MP9, CsItem.UMP, CsItem.Bizon],
			[CsItem.M4A4, CsItem.M4A1S, CsItem.AWP, CsItem.AUG]
			);

		public static readonly RetakeWeapons T = new(
			[CsItem.Glock, CsItem.Deagle, CsItem.P250, CsItem.Elite],
			[CsItem.Bizon, CsItem.XM1014, CsItem.Mac10, CsItem.Galil],
			[CsItem.AK47, CsItem.AWP, CsItem.SG553]
			);
	}

	public readonly struct RetakeWeapons
	{
		public readonly CsItem[] Eco;
		public readonly CsItem[] Force;
		public readonly CsItem[] Full;

		public RetakeWeapons(CsItem[] eco, CsItem[] force, CsItem[] full)
		{
			this.Eco = eco;
			this.Force = force;
			this.Full = full;
		}
	}
}
