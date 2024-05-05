using CS2StratRoulette.Constants;
using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;
using CS2StratRoulette.Enums;
using CS2StratRoulette.Helpers;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class ArmsDealer : Strategy
	{
		public override string Name =>
			"Arms Dealer";

		public override string Description =>
			"Choose wisely.";

		public override StrategyFlags Flags =>
			StrategyFlags.AlwaysVisible;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(ConsoleCommands.BuyAllowNone);
			Server.ExecuteCommand(ConsoleCommands.BuyAllowGrenadesDisable);

			CCSPlayerController? ct = null;
			CCSPlayerController? t = null;

			Player.ForEach((controller) =>
			{
				if (controller.TryGetPlayerPawn(out var pawn))
				{
					controller.EquipKnife();

					pawn.KeepWeaponsByType(CSWeaponType.WEAPONTYPE_KNIFE, CSWeaponType.WEAPONTYPE_C4);

					if (controller.IsBot)
					{
						return;
					}

					// ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
					switch (controller.Team)
					{
						case CsTeam.CounterTerrorist:
							ct = controller;
							break;
						case CsTeam.Terrorist:
							t = controller;
							break;
					}
				}
			});

			if (ct is not null)
			{
				var menu = ArmsDealer.MakeMenu(ref plugin);

				MenuManager.OpenCenterHtmlMenu(plugin, ct, menu);
			}

			if (t is not null)
			{
				var menu = ArmsDealer.MakeMenu(ref plugin);

				MenuManager.OpenCenterHtmlMenu(plugin, t, menu);
			}

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(ConsoleCommands.BuyAllowAll);
			Server.ExecuteCommand(ConsoleCommands.BuyAllowGrenadesEnable);

			return true;
		}

		private static CenterHtmlMenu MakeMenu(ref CS2StratRoulettePlugin plugin)
		{
			var menu = new CenterHtmlMenu("Pick your team's loadout", plugin)
			{
				PostSelectAction = PostSelectAction.Close,
			};

			menu.MenuOptions.Add(new ChatMenuOption2(ArmsDealer.OnSelect));
			menu.MenuOptions.Add(new ChatMenuOption2(ArmsDealer.OnSelect));

			return menu;
		}

		private static void OnSelect(CCSPlayerController captain, ChatMenuOption baseOption)
		{
			var team = captain.Team;

			if (team is not (CsTeam.Terrorist or CsTeam.CounterTerrorist))
			{
				return;
			}

			var option = (ChatMenuOption2)baseOption;

			Player.ForEach((controller) =>
			{
				if (controller.Team != team || !controller.IsValid)
				{
					return;
				}

				controller.GiveNamedItem(option.Gun);

				if (option.HasArmor)
				{
					controller.GiveNamedItem(CsItem.KevlarHelmet);
				}
			});
		}
	}

	[SuppressMessage("Design", "MA0048")]
	public sealed class ChatMenuOption2 : ChatMenuOption
	{
		public readonly CsItem Gun = Items.Weapons[System.Random.Shared.Next(Items.Weapons.Length)];
		public readonly bool HasArmor = (System.Random.Shared.FiftyFifty());

		public ChatMenuOption2(System.Action<CCSPlayerController, ChatMenuOption> onSelect) :
			base(string.Empty, false, onSelect)
		{
			var join = (this.HasArmor) ? "with" : "without";

			this.Text = $"{this.Gun.Str()} {join} armor";
		}
	}
}
