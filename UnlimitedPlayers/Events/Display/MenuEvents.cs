using System;
using System.Linq;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using static StardewValley.Menus.CarpenterMenu;


namespace UnlimitedPlayers.Events.Display
{
	public class MenuEvents
	{
		public void MenuChanged(object sender, MenuChangedEventArgs e)
		{
			if (e.NewMenu is CarpenterMenu)
				MenuChanged_CarpenterMenu(sender, e);
		}

		public void RenderingActiveMenu(object sender, RenderingActiveMenuEventArgs e)
		{
			if (Game1.activeClickableMenu is CharacterCustomization)
				RenderingActiveMenu_CharacterCustomization(sender, e);
		}

		public void MenuChanged_CarpenterMenu(object sender, MenuChangedEventArgs e)
		{
			if (!Context.IsWorldReady)
				return;

			if (Game1.IsMasterGame || Game1.IsServer)
			{
				int buildingsConstructed = Game1.getFarm().getNumberBuildingsConstructed("Cabin");
				if (buildingsConstructed <= 8) return;

				CarpenterMenu newMenu = e.NewMenu as CarpenterMenu;
				int index = newMenu.Blueprints.Last().Index + 1;

				var cabin = Game1.buildingData["Cabin"];
				var buildings = newMenu.Blueprints;

				buildings.Add(new BlueprintEntry(index, "Cabin", cabin, null));
			}
		}

		public void RenderingActiveMenu_CharacterCustomization(object sender, RenderingActiveMenuEventArgs e)
		{
			Type type = typeof(CharacterCustomization);
			CharacterCustomization activeMenu = Game1.activeClickableMenu as CharacterCustomization;

			TextBox nameBox = LazyHelper.GetInstanceField(type, activeMenu, "nameBox") as TextBox;
			TextBox farmNameBox = LazyHelper.GetInstanceField(type, activeMenu, "farmnameBox") as TextBox;
			TextBox favThingBox = LazyHelper.GetInstanceField(type, activeMenu, "favThingBox") as TextBox;

			if (!nameBox.Selected && string.IsNullOrWhiteSpace(nameBox.Text))
				nameBox.Text = "Player";
				Game1.player.Name = nameBox.Text;

			if (!farmNameBox.Selected && string.IsNullOrWhiteSpace(farmNameBox.Text))
				farmNameBox.Text = "Farm";

			if (!favThingBox.Selected && string.IsNullOrWhiteSpace(favThingBox.Text))
				favThingBox.Text = "Bananas";
		}
	}
}
