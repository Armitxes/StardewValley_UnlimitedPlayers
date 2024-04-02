using StardewModdingAPI;
using UnlimitedPlayers.Events.Display;
using UnlimitedPlayers.Events.GameLoop;
using UnlimitedPlayers.Events.Multiplayer;

namespace UnlimitedPlayers
{
	public class ModEntry : Mod
	{
		public override void Entry(IModHelper helper)
		{
			ConfigParser parser = helper.ReadConfig<ConfigParser>();
			parser.Store();                // Now we can access the config from every class without helper or passing the instance
			LazyHelper.ModHelper = helper; // And here I am just absolutly lazy - terribly sorry >.<
			LazyHelper.ModEntry = this;    // There will always only be one valid instance + read above
      RegisterEvents(helper);
			LazyHelper.LogInfo("Default player limit set to " + LazyHelper.PlayerLimit + " players.");
		}

		public static void RegisterEvents(IModHelper helper)
		{
			MenuEvents menuEvents = new();
			helper.Events.GameLoop.GameLaunched += new TickEvents().FirstUpdateTick;
			helper.Events.GameLoop.DayStarted += new DayEvents().DayStarted;
			helper.Events.Display.MenuChanged += menuEvents.MenuChanged;
			helper.Events.Display.RenderingActiveMenu += menuEvents.RenderingActiveMenu;
			helper.Events.Multiplayer.PeerConnected += new ConnectionEvents().PeerConnected;
		}
	}
}
