using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;

namespace UnlimitedPlayers.Events.Display
{
	public class MenuEvents
	{
		public void MenuChanged(object sender, MenuChangedEventArgs e)
		{
			if (e.NewMenu is CarpenterMenu)
				this.MenuChanged_CarpenterMenu(sender, e);
		}

		public void MenuChanged_CarpenterMenu(object sender, MenuChangedEventArgs e)
		{
			if (!Context.IsWorldReady)
				return;

			if (Game1.IsMasterGame || Game1.IsServer)
			{
				// int buildingsConstructed = Game1.getFarm().getNumberBuildingsConstructed("Cabin");
				Type type = typeof(CarpenterMenu);
				object newMenu = e.NewMenu;

				List<BluePrint> bluePrints = LazyHelper.GetInstanceField(type, newMenu, "blueprints") as List<BluePrint>;
				bluePrints?.Add(new BluePrint("Stone Cabin"));
				bluePrints?.Add(new BluePrint("Plank Cabin"));
				bluePrints?.Add(new BluePrint("Log Cabin"));
			}
		}
	}
}
