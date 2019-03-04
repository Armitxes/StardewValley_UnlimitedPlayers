using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace UnlimitedPlayers
{
	public static class Events
	{
		public static void MenuEvents_MenuChanged(object sender, MenuChangedEventArgs e)
		{
			if (!Context.IsWorldReady)
				return;

			if (e.NewMenu is CarpenterMenu)
			{
				int buildingsConstructed = Game1.getFarm().getNumberBuildingsConstructed("Cabin");
				if (Game1.IsMasterGame && buildingsConstructed < LazyHelper.PlayerLimit)
				{
					Type type = typeof(CarpenterMenu);
					object newMenu = e.NewMenu;

					List<BluePrint> bluePrints = LazyHelper.GetInstanceField(type, newMenu, "blueprints") as List<BluePrint>;
					bluePrints?.Add(new BluePrint("Stone Cabin"));
					bluePrints?.Add(new BluePrint("Plank Cabin"));
					bluePrints?.Add(new BluePrint("Log Cabin"));
				}
			}
		}

		public static void GameEvents_FirstUpdateTick(object sender, GameLaunchedEventArgs e)
		{
			// Overwrite the player limit in Stardew Valley source code
            LazyHelper.OverwritePlayerLimit();
		}
	}
}
