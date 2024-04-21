using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Timers;
using CS2StratRoulette.Extensions;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class ForceReload : Strategy
	{
		public override string Name =>
			"Full Ammo";

		public override string Description =>
			"You will be forced to reload your weapon randomly.";

		private Timer? timer;

		private readonly System.Random random = new();

		private const float Interval = 15f;

		private int Ammo;
        
		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}
			
			this.timer = new Timer(ForceReload.Interval, this.OnInterval, TimerFlags.REPEAT);

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

		private void OnInterval()
		{
			var randomNum = this.random.Next(9);
			// ReSharper disable once InvertIf
			if (randomNum < 3)
			{
				foreach (var controller in Utilities.GetPlayers())
				{
					if (!controller.TryGetPlayerPawn(out var pawn))
					{
						continue;
					}

					pawn.ForEachWeapon(weapon =>
					{
						weapon.SetAmmo(0, weapon.ReserveAmmo[0]);
					});

					Utilities.SetStateChanged(controller, "CBasePlayerWeapon", "m_iClip1");
					Utilities.SetStateChanged(controller, "CBasePlayerWeapon", "m_pReserveAmmo");
				}
			}
		}
	}
}
