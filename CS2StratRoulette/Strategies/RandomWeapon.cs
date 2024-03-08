using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public class RandomWeapon : IStrategy
	{
		private const int PistolMin = (int)CsItem.Deagle;
		private const int PistolMax = (int)CsItem.Revolver;

		private const int MidMin = (int)CsItem.Mac10;
		private const int MidMax = (int)CsItem.Negev;

		private const int RifleMin = (int)CsItem.AK47;
		private const int RifleMax = (int)CsItem.G3SG1;

		public string Name => "Random weapon";

		public string Description =>
			"You will receive a random weapon and may only use that weapon.";

		public bool Running { get; private set; }

		private CsItem item = CsItem.Frag;

		private readonly System.Random random = new();

		public bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (this.Running)
			{
				return false;
			}

			this.Running = true;

			Server.ExecuteCommand($"mp_buy_allow_guns {BuyAllow.None}");
			Server.ExecuteCommand("mp_buy_allow_grenades 0");
			Server.ExecuteCommand("mp_weapons_allow_zeus 0");

			if ((this.random.Next() & 1) == 0)
			{
				this.item = (CsItem)this.random.Next(RandomWeapon.PistolMin, RandomWeapon.PistolMax);
			}
			else if ((this.random.Next() & 1) == 0)
			{
				this.item = (CsItem)this.random.Next(RandomWeapon.MidMin, RandomWeapon.MidMax);
			}
			else
			{
				this.item = (CsItem)this.random.Next(RandomWeapon.RifleMin, RandomWeapon.RifleMax);
			}

			foreach (var player in Utilities.GetPlayers())
			{
				if (!player.TryGetPlayerController(out var controller))
				{
					continue;
				}

				if (!controller.TryGetPlayerPawn(out var pawn) || pawn.WeaponServices is null)
				{
					continue;
				}

				var weapons = pawn.WeaponServices.MyWeapons;

				foreach (var weapon in weapons)
				{
					if (!weapon.IsValid || weapon.Value is null || !weapon.Value.IsValid || weapon.Value.VData is null)
					{
						continue;
					}

					var data = new CCSWeaponBaseVData(weapon.Value.VData.Handle);

					if (data.WeaponType is CSWeaponType.WEAPONTYPE_KNIFE or
					                       CSWeaponType.WEAPONTYPE_C4 or
					                       CSWeaponType.WEAPONTYPE_EQUIPMENT)
					{
						pawn.RemovePlayerItem(weapon.Value);
					}
				}

				player.GiveNamedItem(this.item);
			}

			return true;
		}

		public bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!this.Running)
			{
				return false;
			}

			this.Running = false;

			Server.ExecuteCommand($"mp_buy_allow_guns {BuyAllow.All}");
			Server.ExecuteCommand("mp_buy_allow_grenades 1");
			Server.ExecuteCommand("mp_weapons_allow_zeus 1");

			return true;
		}
	}
}
