using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API;
using CS2StratRoulette.Managers;
#if DEBUG
using CounterStrikeSharp.API.Modules.Utils;
using CS2StratRoulette.Extensions;
#endif

namespace CS2StratRoulette
{
	[SuppressMessage("Design", "MA0048")]
	[SuppressMessage("Design", "CA1822")]
	// ReSharper disable once InconsistentNaming
	public sealed partial class CS2StratRoulettePlugin
	{
		[ConsoleCommand("set_strat", "Sets the active strategy")]
		[CommandHelper(1, "[strat]")]
		[RequiresPermissions("@css/root")]
		public void OnStratCommand(CCSPlayerController? _, CommandInfo commandInfo)
		{
			var name = commandInfo.GetArg(1);

			if (!StrategyManager.SetActiveStrategy(name))
			{
				commandInfo.ReplyToCommand("[set_strat] Could not find or instantiate strategy");

				return;
			}

			var result = StrategyManager.Start();

			if (result)
			{
				StrategyManager.Announce();
			}

			commandInfo.ReplyToCommand(
				(result)
					? $"[set_strat] set active strategy to {StrategyManager.Name}"
					: "[set_strat] strategy not found"
			);
		}

		[ConsoleCommand("css_map", "Change map")]
		[CommandHelper(1, "[map]")]
		[RequiresPermissions("@css/changemap")]
		public void OnMapCommand(CCSPlayerController? _, CommandInfo info)
		{
			var name = info.GetArg(1);

			Server.ExecuteCommand($"map {name}");
		}

#if DEBUG
		[ConsoleCommand("props", "")]
		[CommandHelper(1, "[type]")]
		[RequiresPermissions("@css/root")]
		public void OnPropsCommand(CCSPlayerController? player, CommandInfo commandInfo)
		{
			var name = commandInfo.GetArg(1);

			foreach (var entity in Utilities.GetAllEntities())
			{
				if (entity.DesignerName.Contains("fence", System.StringComparison.OrdinalIgnoreCase) ||
					entity.DesignerName.Contains("gate", System.StringComparison.OrdinalIgnoreCase))
				{
					Server.ExecuteCommand($"say {entity.DesignerName}");
				}
			}

			foreach (var entity in Utilities.FindAllEntitiesByDesignerName<CEntityInstance>(name))
			{
				var prop = new CEntityInstance(entity.Handle);

				var model = Constants.Signatures.GetModel.Invoke(prop.Handle);
				Server.ExecuteCommand($"say {model}");
			}
		}

		[ConsoleCommand("roll", "")]
		[RequiresPermissions("@css/root")]
		public void OnRollCommand(CCSPlayerController? player, CommandInfo commandInfo)
		{
			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					continue;
				}

				if (controller.Team is not (CsTeam.Terrorist or CsTeam.CounterTerrorist))
				{
					continue;
				}

				if (pawn.AbsOrigin is null ||
					pawn.AbsRotation is null)
				{
					continue;
				}

				pawn.AbsRotation.Z = float.Abs(pawn.AbsRotation.Z - 45f) < 0.1f ? 0f : 45f;

				pawn.Teleport(pawn.AbsOrigin, pawn.AbsRotation, pawn.AbsVelocity);
			}
		}
#endif
	}
}
