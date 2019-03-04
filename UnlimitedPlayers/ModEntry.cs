using StardewModdingAPI;

namespace UnlimitedPlayers
{
	/// <summary>The mod entry point.</summary>
	public class ModEntry : Mod
	{
		/// <summary>The mod entry point, called after the mod is first loaded.</summary>
		/// <param name="helper">Provides simplified APIs for writing mods.</param>
		public override void Entry(IModHelper helper)
		{
			ConfigParser parser = helper.ReadConfig<ConfigParser>();
			parser.Store(); // Now we can access the config from every class without helper or passing the instance
			LazyHelper.ModHelper = helper; // And here I am just absolutly lazy - terribly sorry >.<
			LazyHelper.ModEntry = this; // There will always only be one valid instance + see above
		    helper.Events.GameLoop.GameLaunched += Events.GameEvents_FirstUpdateTick;
			//GameEvents.FirstUpdateTick += Events.GameEvents_FirstUpdateTick;
			helper.Events.Display.MenuChanged += Events.MenuEvents_MenuChanged;

			// Overwrite the player limit in Stardew Valley source code
			LazyHelper.OverwritePlayerLimit();
			LazyHelper.ModEntry.Monitor.Log("Player limit set to " + LazyHelper.PlayerLimit + " players.", LogLevel.Info);
		}
	}
}
