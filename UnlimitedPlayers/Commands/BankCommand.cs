using System.Linq;
using StardewModdingAPI;
using StardewValley;

namespace UnlimitedPlayers.Commands
{
  public class BankCommand
  {
    public BankCommand()
    {
      IModHelper helper = LazyHelper.ModHelper;
      helper.ConsoleCommands.Add(
        name: "bank",
        documentation: "Usage: bank\nPrint possible bank actions.",
        callback: HandleCommand
      );
    }

    public static void HandleCommand(string cmd, string[] args)
    {
      if (args.Length == 0) {
        BankHelp();
        return;
      }

      switch (args[0]) {
        case "statement": BankStatement(); break;
        case "suslist": BankSusList(); break;
        case "wipe": BankWipe(args); break;
        default: BankHelp(); break;
      }
    }

    public static void BankHelp() {
      LazyHelper.LogInfo(
        "\nUsage: bank <command>\nCommands:\n"
        + "- statement: Print bank statement.\n"
        + "- suslist: List of suspicious accounts.\n"
        + "- wipe <uid>: Wipe balance of given uid to 0.\n"
      );
    }

    public static void BankStatement() {
      FarmerTeam team = Game1.player.team;
      string result = "\n--- Bank Statement ---\n";
      int teamBalance = team.money.Value;

      if (team.useSeparateWallets.Value) {
        teamBalance = 0;
        foreach(Farmer farmer in Game1.getAllFarmers()) {
          string uid = farmer.UniqueMultiplayerID.ToString();
          int money = team.GetMoney(farmer).Value;
          teamBalance += money;
          result += $"{uid}\t${money}\t{farmer.Name}\n";
        }
        result += "\n";
      }

      string earned = team.totalMoneyEarned.Value.ToString();
      result += $"Balance: ${teamBalance}\n";
      result += $" Earned: ${earned}\n";
      result += "----------------------\n";
      LazyHelper.LogInfo(result);
    }

    public static void BankSusList() {
      FarmerTeam team = Game1.player.team;
      if (!team.useSeparateWallets.Value) {
        LazyHelper.LogWarn("Command only available with separate wallets.");
        return;
      }

      string result = "\n--- Suspicious Accounts ---\n";
      int earned = team.totalMoneyEarned.Value;

      foreach(Farmer farmer in Game1.getAllFarmers()) {
        string uid = farmer.UniqueMultiplayerID.ToString();
        int money = team.GetMoney(farmer).Value;
        if (money > earned)
          result += $"{uid}\t${money}\t{farmer.Name}\n";
      }
      result += "---------------------------\n";
      LazyHelper.LogInfo(result);
    }

    public static void BankWipe(string[] args) {
      FarmerTeam team = Game1.player.team;
      if (!team.useSeparateWallets.Value) {
        LazyHelper.LogWarn("Command only available with separate wallets.");
        return;
      }

      if (args.Length < 2) {
        LazyHelper.LogWarn("bank wipe: Missing uid");
        return;
      }

      if(!long.TryParse(args[1], out long uid)) {
        LazyHelper.LogWarn("Invalid uid");
        return;
      }
      team.SetIndividualMoney(who: Game1.getFarmerMaybeOffline(uid), 0);
      LazyHelper.LogInfo("Bank account wiped.");
    }

  }
}