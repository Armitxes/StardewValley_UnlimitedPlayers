﻿using StardewModdingAPI;
using UnlimitedPlayers.Events.Display;
using UnlimitedPlayers.Events.GameLoop;

namespace UnlimitedPlayers
{
	public class ModEntry : Mod
	{
		public override void Entry(IModHelper helper)
		{
			ConfigParser parser = helper.ReadConfig<ConfigParser>();
			parser.Store();                // Now we can access the config from every class without helper or passing the instance
			LazyHelper.ModHelper = helper; // And here I am just absolutly lazy - terribly sorry >.<
			LazyHelper.ModEntry = this;    // There will always only be one valid instance + see above
			helper.Events.GameLoop.GameLaunched += new TickEvents().FirstUpdateTick;
			helper.Events.GameLoop.DayStarted += new DayEvents().DayStarted;
			helper.Events.Display.RenderingActiveMenu += new RenderingActiveMenuEvents().RenderingActiveMenu;
			helper.Events.Display.MenuChanged += new MenuEvents().MenuChanged;
			LazyHelper.ModEntry.Monitor.Log("Default player limit set to " + LazyHelper.PlayerLimit + " players.", LogLevel.Info);
		}
	}
}
