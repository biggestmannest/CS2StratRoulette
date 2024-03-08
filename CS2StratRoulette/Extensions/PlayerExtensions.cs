using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Extensions
{
	public static class PlayerExtensions
	{
		public static bool TryGetPlayerController(this CCSPlayerController? ccsPlayerController,
		                                          [NotNullWhen(true)] out CCSPlayerController? playerController)
		{
			playerController = null;

			if (ccsPlayerController is null || !ccsPlayerController.IsValid)
			{
				return false;
			}

			playerController = ccsPlayerController;

			return true;
		}

		public static bool TryGetPlayerPawn(this CCSPlayerController controller,
		                                    [NotNullWhen(true)] out CCSPlayerPawn? pawn)
		{
			pawn = null;

			if (!controller.PlayerPawn.IsValid || controller.PlayerPawn.Value is null)
			{
				return false;
			}

			var tmp = controller.PlayerPawn.Value!;

			if (!tmp.IsValid)
			{
				return false;
			}

			pawn = tmp;

			return true;
		}
	}
}
