using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace UnlimitedPlayers.Events.Multiplayer
{
	class ConnectionEvents
	{

    public void PeerConnected(object sender, PeerConnectedEventArgs e)
    {
      List<string> requiredMods = LazyHelper.RequiredMods;
      List<string> unknownMods = new List<string>();

      foreach (IMultiplayerPeerMod mod in e.Peer.Mods)
      {
        bool removed = requiredMods.Remove(mod.ID);
        if (!removed && !LazyHelper.OptionalMods.Contains(mod.ID))
          unknownMods.Add(mod.ID);
      }

      string requiredModsStr = string.Join(", ", requiredMods);
      string unknownModsStr = string.Join(", ", unknownMods);

      string warning = "";
      if (requiredMods.Count > 0)
        warning += $"Peer {e.Peer.PlayerID} misses required mods: {requiredModsStr}\n";
      if (unknownMods.Count > 0)
        warning += $"Peer {e.Peer.PlayerID} uses non-whitelisted mods: {unknownModsStr}\n";
      if (warning != "")
      {
        LazyHelper.LogWarn(warning);
        if (LazyHelper.KickOnModsMismatch > 0)
          Game1.server.kick(e.Peer.PlayerID);
      }
    }

	}
}
