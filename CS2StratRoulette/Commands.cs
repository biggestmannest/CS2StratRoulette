using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using JetBrains.Annotations;

namespace CS2StratRoulette
{
	[UsedImplicitly(ImplicitUseTargetFlags.Members)]
	public abstract class Commands : Base
	{
		[ConsoleCommand("set_strat", "Sets the active strategy")]
		[CommandHelper(1, "[strat]")]
		[RequiresPermissions("@css/root")]
		public void OnStratCommand(CCSPlayerController? player, CommandInfo commandInfo)
		{
			var name = commandInfo.GetArg(1);

			this.StopActiveStrategy();

			foreach (var strategy in this.Strategies)
			{
				if (!string.Equals(strategy.Name, name, System.StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}

				if (!this.SetActiveStrategy(strategy))
				{
					commandInfo.ReplyToCommand($"[OnStratCommand] failed setting {strategy.Name} as active strategy");

					return;
				}

				if (!this.StartActiveStrategy())
				{
					commandInfo.ReplyToCommand("[OnStratCommand] failed starting strategy");

					return;
				}

				commandInfo.ReplyToCommand($"[OnStratCommand] set active strategy to {strategy.Name}");

				if (this.ActiveStrategy is not null)
				{
					this.AnnounceStrategy(this.ActiveStrategy);
				}

				return;
			}

			commandInfo.ReplyToCommand("[OnStratCommand] strategy not found");
		}

		[ConsoleCommand("css_map", "Change map")]
		[CommandHelper(1, "[map]")]
		[RequiresPermissions("@css/changemap")]
		public void OnMapCommand(CCSPlayerController? player, CommandInfo commandInfo)
		{
			var name = commandInfo.GetArg(1);

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
	}
#endif
}
