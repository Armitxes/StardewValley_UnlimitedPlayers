using System;
using StardewModdingAPI;
using System.Reflection;

namespace Armitxes.StardewValley.UnlimitedPlayers
{
	public static class LazyHelper
	{
		public static int PlayerLimit { get; set; } = 10;
		public static IModHelper ModHelper { get; set; }
		public static ModEntry ModEntry { get; set; }

		internal static object GetInstanceField(Type type, object instance, string fieldName)
		{
			BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
			FieldInfo field = type.GetField(fieldName, bindFlags);
			return field.GetValue(instance);
		}
	}

	public class ConfigParser
	{
		public int PlayerLimit { get; set; } = 10;

		public void Store()
		{
			LazyHelper.PlayerLimit = this.PlayerLimit;
		}
	}
}