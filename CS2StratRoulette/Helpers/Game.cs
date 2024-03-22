using System.Linq;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace CS2StratRoulette.Helpers
{
	public static class Game
	{
		public static CCSGameRulesProxy? RulesProxy()
		{
			const string gameRules = "cs_gamerules";

			var entity = Utilities.FindAllEntitiesByDesignerName<CCSGameRulesProxy>(gameRules).FirstOrDefault();

			if (entity is null || !entity.IsValid)
			{
				return null;
			}

			return entity;
		}

		public static CCSGameRules? Rules() =>
			Game.RulesProxy()?.GameRules;
	}
}
