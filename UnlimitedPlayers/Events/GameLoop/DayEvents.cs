using StardewModdingAPI.Events;

namespace UnlimitedPlayers.Events.GameLoop
{
	class DayEvents
	{
		public void DayStarted(object sender, DayStartedEventArgs e)
		{
			LazyHelper.OverwritePlayerLimit();
		}
	}
}
