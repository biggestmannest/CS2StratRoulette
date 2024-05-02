using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Timers;
using CS2StratRoulette.Extensions;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class DecoyDodgeball : Strategy
	{
		public override string Name =>
			"Decoy Dodgeball";

		public override string Description =>
			"1 HP + Infinite Decoys";

		private Timer? timer;

		private const float Interval = 3f;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			foreach (var player in Utilities.GetPlayers())
			{
				if (!player.TryGetPlayerPawn(out var pawn))
				{
					continue;
				}

				Server.NextFrame(() =>
				{
					player.RemoveWeapons();
				});

				Server.NextFrame(() => { player.EquipKnife(); });

				player.GiveNamedItem(CsItem.Decoy);

				Server.NextFrame(() =>
				{
					pawn.Health = 1;
					pawn.MaxHealth = 1;
					pawn.ArmorValue = 0;
				});

				Server.NextFrame(() =>
				{
					Utilities.SetStateChanged(pawn, "CBaseEntity", "m_iMaxHealth");
					Utilities.SetStateChanged(pawn, "CBaseEntity", "m_iHealth");
					Utilities.SetStateChanged(pawn, "CCSPlayerPawnBase", "m_ArmorValue");
				});
			}

			this.timer = new Timer(DecoyDodgeball.Interval, DecoyDodgeball.OnInterval, TimerFlags.REPEAT);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			this.timer?.Kill();

			return true;
		}

		private static void OnInterval()
		{
			foreach (var player in Utilities.GetPlayers())
			{
				if (!player.TryGetPlayerPawn(out var pawn))
				{
					continue;
				}

				if (!pawn.HasWeapon("weapon_decoy"))
				{
					player.GiveNamedItem(CsItem.Decoy);
				}
			}
		}
	}
}
