using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CS2StratRoulette.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Quake : Strategy
	{
		private const string Enable =
			"sv_cheats 1;sv_enablebunnyhopping 1;sv_maxvelocity 4000;sv_staminamax 0;sv_staminalandcost 0;sv_staminajumpcost 0;sv_accelerate_use_weapon_speed 0;sv_staminarecoveryrate 0;sv_autobunnyhopping 1;sv_airaccelerate 24;sv_cheats 0";

		private const string Disabled =
			"sv_cheats 1;sv_enablebunnyhopping 0;sv_maxvelocity 3500;sv_staminamax 80;sv_staminalandcost 0.05;sv_staminajumpcost 0.08;sv_accelerate_use_weapon_speed 1;sv_staminarecoveryrate 60;sv_autobunnyhopping 0;sv_airaccelerate 12;sv_cheats 0";

		public override string Name =>
			"Quake";

		public override string Description =>
			"It's Quake.";

		// ReSharper disable once InconsistentNaming
		private uint? FOV;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(Quake.Enable);

			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.TryGetPlayerPawn(out var pawn) || controller.IsBot)
				{
					continue;
				}

				if (pawn.CameraServices is null)
				{
					continue;
				}

				var camera = new CCSPlayer_CameraServices(pawn.CameraServices.Handle);

				System.Console.WriteLine($"[Quake]: {camera.FOV}");

				this.FOV ??= camera.FOV;

				Server.NextFrame(() =>
				{
					camera.FOV = 90;
					Utilities.SetStateChanged(controller, "CBasePlayerPawn", "m_pCameraServices");
				});
			}

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(Quake.Disabled);

			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.TryGetPlayerPawn(out var pawn) || controller.IsBot)
				{
					continue;
				}

				if (pawn.CameraServices is null)
				{
					continue;
				}

				var camera = new CCSPlayer_CameraServices(pawn.CameraServices.Handle);

				System.Console.WriteLine($"[Quake]: {camera.FOV}");

				Server.NextFrame(() =>
				{
					camera.FOV = this.FOV ?? 60U;
					Utilities.SetStateChanged(controller, "CBasePlayerPawn", "m_pCameraServices");
				});
			}

			return true;
		}
	}
}
