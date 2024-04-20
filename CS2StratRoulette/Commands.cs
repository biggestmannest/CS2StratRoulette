using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API;
using CS2StratRoulette.Managers;
using CounterStrikeSharp.API.Modules.Utils;
using CS2StratRoulette.Extensions;

namespace CS2StratRoulette
{
	[SuppressMessage("Design", "MA0048")]
	[SuppressMessage("Design", "CA1822")]
	// ReSharper disable once InconsistentNaming
	public sealed partial class CS2StratRoulettePlugin
	{
		private const string Prefix = "[SRP]";

		[ConsoleCommand("css_srp_toggle", "Toggles the plugin off/on")]
		[RequiresPermissions("@css/root")]
		public void OnToggleCommand(CCSPlayerController? _, CommandInfo info)
		{
			var active = !this.Active;

			if (active)
			{
				StrategyManager.Start();
			}
			else
			{
				StrategyManager.Kill();
			}

			info.ReplyToCommand(
				(active)
					? $"{CS2StratRoulettePlugin.Prefix} {ChatColors.Green}Started SRP"
					: $"{CS2StratRoulettePlugin.Prefix} {ChatColors.LightRed}Stopped SRP"
			);

			this.Active = active;
		}

		[ConsoleCommand("css_srp_set", "Sets and starts given strategy")]
		[CommandHelper(1, "[strategy]")]
		[RequiresPermissions("@css/root")]
		public void OnSetStrategyCommand(CCSPlayerController? _, CommandInfo info)
		{
			if (!this.Active)
			{
				return;
			}

			var name = info.GetArg(1);

			if (!StrategyManager.SetActiveStrategy(name))
			{
				info.ReplyToCommand($"{CS2StratRoulettePlugin.Prefix} Could not find or instantiate strategy");

				return;
			}

			var result = StrategyManager.Start();

			if (result)
			{
				StrategyManager.Announce();
			}

			info.ReplyToCommand(
				(result)
					? $"{CS2StratRoulettePlugin.Prefix} Set strategy to {StrategyManager.Name}"
					: $"{CS2StratRoulettePlugin.Prefix} Failed starting strategy"
			);
		}

		[ConsoleCommand("css_srp_start", "Starts the current strategy")]
		[RequiresPermissions("@css/root")]
		public void OnStartStrategyCommand(CCSPlayerController? _, CommandInfo info)
		{
			if (!this.Active)
			{
				return;
			}

			info.ReplyToCommand(StrategyManager.Start()
									? $"{CS2StratRoulettePlugin.Prefix} {ChatColors.Green}Started"
									: $"{CS2StratRoulettePlugin.Prefix} {ChatColors.LightRed}Failed starting");
		}

		[ConsoleCommand("css_srp_stop", "Stops the current strategy")]
		[RequiresPermissions("@css/root")]
		public void OnStopStrategyCommand(CCSPlayerController? _, CommandInfo info)
		{
			if (!this.Active)
			{
				return;
			}

			info.ReplyToCommand(StrategyManager.Kill()
									? $"{CS2StratRoulettePlugin.Prefix} {ChatColors.Green}Stopped"
									: $"{CS2StratRoulettePlugin.Prefix} {ChatColors.LightRed}Failed stopping");
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
		[ConsoleCommand("srp_props", "")]
		[CommandHelper(1, "[type]")]
		[RequiresPermissions("@css/root")]
		public void OnPropsCommand(CCSPlayerController? _, CommandInfo info)
		{
			var name = info.GetArg(1);

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

		[ConsoleCommand("css_srp_sv")]
		[RequiresPermissions("@css/root")]
		public void OnSvCommand(CCSPlayerController? _, CommandInfo __)
		{
			const string cmd =
				"sv_cheats 1;mp_freezetime 0;mp_roundtime_defuse 60;mp_roundtime_hostage 60;mp_roundtime 60;mp_buy_anywhere 1;sv_infinite_ammo 1;mp_maxmoney 65535;mp_startmoney 65535;mp_afterroundmoney 65535;mp_buytime 60000;mp_restartgame 1";

			Server.ExecuteCommand(cmd);
		}

		[ConsoleCommand("srp_roll")]
		[RequiresPermissions("@css/root")]
		public void OnRollCommand(CCSPlayerController? _, CommandInfo __)
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
