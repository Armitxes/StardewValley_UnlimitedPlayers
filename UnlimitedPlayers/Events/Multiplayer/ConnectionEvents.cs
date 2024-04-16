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
      List<string> clientMods = new List<string>();
      List<string> denylistMods = new List<string>();

      foreach (IMultiplayerPeerMod mod in e.Peer.Mods)
      {
        clientMods.Add(mod.ID);
        if (LazyHelper.ClientModsDenylist.Contains(mod.ID))
          denylistMods.Add(mod.ID);
      }

      string clientModsStr = string.Join(", ", clientMods);
      string deniedModsStr = string.Join(", ", denylistMods);

      if (clientMods.Count > 0) {
        LazyHelper.LogInfo($"Peer {e.Peer.PlayerID} uses mods: {clientModsStr}");
      }
      if (denylistMods.Count > 0) {
        LazyHelper.LogWarn($"Peer {e.Peer.PlayerID} kicked for using illegal mods: {deniedModsStr}");
        Game1.server.kick(e.Peer.PlayerID);
      }
    }

  }
}
