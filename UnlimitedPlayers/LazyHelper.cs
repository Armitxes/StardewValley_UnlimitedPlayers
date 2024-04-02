using System;
using System.Collections.Generic;
using System.Reflection;
using StardewModdingAPI;
using StardewValley;

namespace UnlimitedPlayers
{
  public static class LazyHelper
  {
    public static int PlayerLimit { get; set; } = 10;
    public static int KickOnModsMismatch { get; set; } = 0;
    public static List<string> RequiredMods { get; set; } = new List<string>();
    public static List<string> OptionalMods { get; set; } = new List<string>();
    public static IModHelper ModHelper { get; set; }
    public static ModEntry ModEntry { get; set; }

    internal static object GetInstanceField(Type type, object instance, string fieldName)
    {
      BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
      FieldInfo field = type.GetField(fieldName, bindFlags);
      return field?.GetValue(instance);
    }

    public static void UpdateHost()
    {
      if (Game1.netWorldState.Value == null || !Game1.IsMasterGame)
        return;

      int currentPlayerLimit = Game1.netWorldState.Value.CurrentPlayerLimit;
      int highestPlayerLimit = Game1.netWorldState.Value.CurrentPlayerLimit;

      if (currentPlayerLimit != PlayerLimit)
        Game1.netWorldState.Value.CurrentPlayerLimit = PlayerLimit;

      if (highestPlayerLimit != PlayerLimit)
        Game1.netWorldState.Value.HighestPlayerLimit = PlayerLimit;

      if (GetInstanceField(typeof(Game1), Game1.game1, "multiplayer") is Multiplayer mp)
      {
        int playerLimit = mp.playerLimit;
        if (playerLimit != PlayerLimit)
          mp.playerLimit = PlayerLimit;

        // GameRunner.instance expects public StardewValley.GameRunner - which we can inject and override.
        // However, SMAPI injected its internal StardewModdingAPI.Framework.SGameRunner - which we can't inject nor override.

        if (
          currentPlayerLimit == PlayerLimit
          && highestPlayerLimit == PlayerLimit
          && playerLimit == PlayerLimit
        )
          return;

        LogInfo(
          "\n[SERVER] Adjusting limit to " + PlayerLimit + " players." +
          "\n- Multiplayer.playerLimit: " + mp.playerLimit +
          "\n- Multiplayer.MaxPlayers: " + mp.MaxPlayers + " (fixed once you create/join a multiplayer session)" +
          "\n- GameRunnerInstance.GetMaxSimultaneousPlayers(): " + GameRunner.instance.GetMaxSimultaneousPlayers() +
          "\n- netWorldState.CurrentPlayerLimit: " + Game1.netWorldState.Value.CurrentPlayerLimit +
          "\n- netWorldState.HighestPlayerLimit: " + Game1.netWorldState.Value.HighestPlayerLimit
        );
      }
    }

    public static void UpdateClient()
    {
      if (Game1.netWorldState.Value == null || Game1.IsMasterGame)
        return;

      if (GetInstanceField(typeof(Game1), Game1.game1, "multiplayer") is Multiplayer mp)
      {
        int newLimit = Game1.netWorldState.Value.HighestPlayerLimit;
        if (mp.playerLimit == newLimit)
          return;

        mp.playerLimit = newLimit;
        LogInfo(
          "\n[CLIENT] Adjusting limit to " + newLimit + " players." +
          "\n- Multiplayer.playerLimit: " + mp.playerLimit +
          "\n- Multiplayer.MaxPlayers: " + mp.MaxPlayers +
          "\n- GameRunnerInstance.GetMaxSimultaneousPlayers(): " + GameRunner.instance.GetMaxSimultaneousPlayers() +
          "\n- netWorldState.CurrentPlayerLimit: " + Game1.netWorldState.Value.CurrentPlayerLimit +
          "\n- netWorldState.HighestPlayerLimit: " + Game1.netWorldState.Value.HighestPlayerLimit
        );
      }
    }

    public static void LogInfo(string message)
    {
      ModEntry.Monitor.Log(message, LogLevel.Info);
    }

    public static void LogWarn(string message)
    {
      ModEntry.Monitor.Log(message, LogLevel.Warn);
    }
  }

  public class ConfigParser
  {
    public int PlayerLimit { get; set; } = 10;

    public int KickOnModsMismatch { get; set; } = 0;

    public List<string> RequiredMods { get; set; } = new List<string>();

    public List<string> OptionalMods { get; set; } = new List<string>();

    public void Store()
    {
      LazyHelper.PlayerLimit = PlayerLimit;
      LazyHelper.KickOnModsMismatch = KickOnModsMismatch;
      LazyHelper.RequiredMods = RequiredMods;
      LazyHelper.OptionalMods = OptionalMods;
    }
  }
}
