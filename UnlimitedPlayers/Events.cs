using System;
using StardewModdingAPI;
using System.Collections.Generic;
using StardewValley;
using StardewValley.Menus;
using StardewModdingAPI.Events;

namespace Armitxes.StardewValley.UnlimitedPlayers
{
	public static class Events
	{
		public static void MenuEvents_MenuChanged(object sender, EventArgsClickableMenuChanged e)
		{
			if (!Context.IsWorldReady)
				return;

			if (e.NewMenu is CarpenterMenu)
			{
				int buildingsConstructed = Game1.getFarm().getNumberBuildingsConstructed("Cabin");
				if (Game1.IsMasterGame && buildingsConstructed < LazyHelper.PlayerLimit)
				{
					Type type = typeof(CarpenterMenu);
					object newMenu = (object)e.NewMenu;

					List<BluePrint> bluePrints = LazyHelper.GetInstanceField(type, newMenu, "blueprints") as List<BluePrint>;
					bluePrints.Add(new BluePrint("Stone Cabin"));
					bluePrints.Add(new BluePrint("Plank Cabin"));
					bluePrints.Add(new BluePrint("Log Cabin"));
				}
			}
		}

		public static void GameEvents_FirstUpdateTick(object sender, EventArgs e)
		{
			Type type = typeof(Game1);
			Multiplayer mp_mp = LazyHelper.GetInstanceField(type, Game1.game1, "multiplayer") as Multiplayer;
			mp_mp.playerLimit = LazyHelper.PlayerLimit;
		}
	}
}