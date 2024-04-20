using System.Collections.Generic;
using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Utils;
using CS2StratRoulette.Constants;
using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;
using CS2StratRoulette.Helpers;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Retakes : Strategy
	{
		public override string Name =>
			"Retakes";

		public override string Description =>
			"Bomb will be planted at a random bombsite. T's have to defend, CT's have to retake the site and defuse.";

		private const string ExtendC4Timer = "mp_c4timer 55";
		private const string NormalC4Timer = "mp_c4timer 40";

		private readonly List<CCSPlayerController> cts = new(Server.MaxPlayers / 2);
		private readonly List<CCSPlayerController> ts = new(Server.MaxPlayers / 2);

		private static readonly System.Random Random = new();

		private BombSite bombsite;

		private string mapName = string.Empty;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(Retakes.ExtendC4Timer);

			this.mapName = Server.MapName;

			foreach (var player in Utilities.GetPlayers())
			{
				if (player.Team is CsTeam.Terrorist)
				{
					this.ts.Add(player);
				}
				else if (player.Team is CsTeam.CounterTerrorist)
				{
					this.cts.Add(player);
				}
			}

			var randomT = this.ts[Retakes.Random.Next(this.ts.Count)];

			this.PlantTheBomb(randomT);

			this.TeleportPlayers();

			var siteAnnouncement = this.bombsite switch
			{
				BombSite.A => $"Bomb has been planted on bombsite A.",
				BombSite.B => $"Bomb has been planted on bombsite B",
				_ => string.Empty,
			};

			foreach (var player in Utilities.GetPlayers())
			{
				player.PrintToCenter(siteAnnouncement);

				if (player.Team is CsTeam.CounterTerrorist)
				{
					player.GiveNamedItem("item_defuser");
				}
			}

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(Retakes.NormalC4Timer);

			return true;
		}

		private static void SendBombPlantedEvent(CCSPlayerController bombCarrier, BombSite bombSite)
		{
			if (!bombCarrier.IsValid || bombCarrier.PlayerPawn.Value == null)
			{
				return;
			}

			var eventPtr = NativeAPI.CreateEvent("bomb_planted", true);
			NativeAPI.SetEventPlayerController(eventPtr, "userid", bombCarrier.Handle);
			NativeAPI.SetEventInt(eventPtr, "userid", (int)bombCarrier.PlayerPawn.Value.Index);
			NativeAPI.SetEventInt(eventPtr, "site", (int)bombSite);

			NativeAPI.FireEvent(eventPtr, false);
		}

		private void PlantTheBomb(CCSPlayerController player)
		{
			if (!RetakeSpots.Maps.TryGetValue(this.mapName, out var map))
			{
				return;
			}
			
			this.bombsite = (Retakes.Random.FiftyFifty() ? BombSite.A : BombSite.B);
			var randomSite = this.bombsite is BombSite.A ? map.BombA : map.BombB;

			var position = randomSite[Retakes.Random.Next(randomSite.Length)];

			var bombEntity = Utilities.CreateEntityByName<CPlantedC4>("planted_c4");

			player.Teleport(
				position,
				QAngle.Zero,
				Vector.Zero
			);

			if (bombEntity?.AbsOrigin is null || player.AbsOrigin is null)
			{
				return;
			}

			bombEntity.AbsOrigin.X = player.AbsOrigin.X;
			bombEntity.AbsOrigin.Y = player.AbsOrigin.Y;
			bombEntity.AbsOrigin.Z = player.AbsOrigin.Z;
			bombEntity.HasExploded = false;

			
			bombEntity.BombSite = (int)this.bombsite;

			bombEntity.BombTicking = true;
			bombEntity.CannotBeDefused = false;

			bombEntity.DispatchSpawn();

			var gameRules = Game.Rules();

			if (gameRules is null)
			{
				return;
			}

			gameRules.BombPlanted = true;
			gameRules.BombDefused = false;
			Retakes.SendBombPlantedEvent(player, this.bombsite);
			System.Console.WriteLine($"Bomb planted at Bombsite: {this.bombsite}");
		}

		private void TeleportPlayers()
		{
			if (!RetakeSpots.Maps.TryGetValue(this.mapName, out var map))
			{
				return;
			}

			var ctTeleportSpots = this.GetCtPositions(map);
			var tTeleportSpots = this.GetTPositions(map);

			Retakes.TeleportTeam(this.cts, ctTeleportSpots, CsTeam.CounterTerrorist);
			Retakes.TeleportTeam(this.ts, tTeleportSpots, CsTeam.Terrorist);
		}

		private static void TeleportTeam(List<CCSPlayerController> teams, Vector[] positions, CsTeam team)
		{
			System.Random.Shared.Shuffle(positions);
			var n = 0;

			var theTeam = team is CsTeam.CounterTerrorist ? RetakeBuys.CT : RetakeBuys.T;

			var weapons = Retakes.Random.Next(3) switch
			{
				0 => theTeam.Eco,
				1 => theTeam.Force,
				_ => theTeam.Full,
			};

			foreach (var player in teams)
			{
				if (!player.TryGetPlayerPawn(out var pawn))
				{
					continue;
				}

				Server.NextFrame(() =>
				{
					pawn.KeepWeaponsByType(CSWeaponType.WEAPONTYPE_KNIFE);
					if (player.Team is CsTeam.Terrorist)
					{
						pawn.ForEachWeapon((weapon) =>
						{
							if (!string.Equals(weapon.DesignerName, "weapon_c4", System.StringComparison.OrdinalIgnoreCase))
							{
								return;
							}

							player.RemoveItemByDesignerName("weapon_c4");
						});
					}
				});

				Server.NextFrame(() => player.GiveNamedItem(weapons[Retakes.Random.Next(weapons.Length)]));

				pawn.Teleport(positions[n++], QAngle.Zero, Vector.Zero);
				if (n >= positions.Length)
				{
					n = 0;
				}
			}
		}

		private Vector[] GetCtPositions(RetakeSpots.AllRetakeSpots map)
		{
			return this.bombsite is BombSite.B ? map.CtsB : map.CtsA;
		}

		private Vector[] GetTPositions(RetakeSpots.AllRetakeSpots map)
		{
			return this.bombsite is BombSite.B ? map.TsB : map.TsA;
		}
	}
}
