using StardewModdingAPI.Events;

namespace UnlimitedPlayers.Events.GameLoop
{
	public class TickEvents
	{
		public void FirstUpdateTick(object sender, GameLaunchedEventArgs e)
		{
			// Overwrite the player limit in Stardew Valley source code
			LazyHelper.OverwritePlayerLimit();
		}
	}
}
