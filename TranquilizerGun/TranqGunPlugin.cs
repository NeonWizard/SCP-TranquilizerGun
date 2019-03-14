using Smod2;
using Smod2.API;
using Smod2.Attributes;
using Smod2.Config;
using Smod2.Commands;
using ItemManager;
using ItemManager.Recipes;
using ItemManager.Utilities;
using System.Collections.Generic;
using scp4aiur;
using System;

namespace TranquilizerGun
{
	[PluginDetails(
		author = "Spooky",
		name = "TranquilizerGun",
		description = "Adds a tranquilizer gun with temporarily ragdolls players.",
		id = "xyz.wizardlywonders.TranquilizerGun",
		version = "1.0.1",
		SmodMajor = 3,
		SmodMinor = 3,
		SmodRevision = 0
	)]
	public class TranqGunPlugin : Plugin
	{
		public const int TranqID = 105;

		public CustomWeaponHandler<TranquilizerGun> Handler { get; private set; }

		public static int Damage { get; private set; }
		public static float FireRate { get; private set; }
		public static int Magazine { get; private set; }
		public static int ReserveAmmo { get; private set; }

		public static float TranqDuration { get; private set; }

		public static List<string> SpawnLocations = new List<string>();

		public override void OnDisable()
		{
			this.Info("TranquilizerGun has been disabled.");
		}

		public override void OnEnable()
		{
			this.Info("TranquilizerGun has loaded successfully.");
		}

		public override void Register()
		{
			Timing.Init(this);

			// -- Register config
			this.AddConfig(new ConfigSetting("tranqgun_enable", true, SettingType.BOOL, true, "Whether TranquilizerGun should be enabled on server start."));

			this.AddConfig(new ConfigSetting("tranqgun_damage", 0, SettingType.NUMERIC, true, "Damage dealt by the tranquilizer gun."));
			this.AddConfig(new ConfigSetting("tranqgun_firerate", 3f, SettingType.FLOAT, true, "Time (in seconds) between each shot."));
			this.AddConfig(new ConfigSetting("tranqgun_magazine", 1, SettingType.NUMERIC, true, "Amount of shots per magazine."));
			this.AddConfig(new ConfigSetting("tranqgun_reserveammo", 2, SettingType.NUMERIC, true, "Default reserve ammo for each player."));

			this.AddConfig(new ConfigSetting("tranqgun_duration", 5f, SettingType.FLOAT, true, "Time (in seconds) the target is tranquilized for."));

			this.AddConfig(new ConfigSetting("tranqgun_spawns", new string[] { "wheremyhotdogsat" }, true, SettingType.LIST, true, "Locations that the tranquilizer gun can spawn in."));

			// -- Register events
			this.AddEventHandlers(new MiscEventHandler(this), Smod2.Events.Priority.Low);

			// -- Register weapon
			this.Handler = new CustomWeaponHandler<TranquilizerGun>(TranqGunPlugin.TranqID)
			{
				DefaultType = ItemType.USP,
				AmmoName = "Tranquilizer Dart",
				DefaultReserveAmmo = 2
			};
			this.Handler.Register();
			Items.AddRecipe(new Id914Recipe(KnobSetting.FINE, (int)ItemType.USP, TranqGunPlugin.TranqID, 1));

			this.ReloadConfig();
		}

		public void ReloadConfig()
		{
			TranqGunPlugin.Damage = GetConfigInt("tranqgun_damage");
			TranqGunPlugin.FireRate = GetConfigFloat("tranqgun_firerate");
			TranqGunPlugin.Magazine = GetConfigInt("tranqgun_magazine");

			TranqGunPlugin.ReserveAmmo = GetConfigInt("tranqgun_reserveammo");
			this.Handler.DefaultReserveAmmo = TranqGunPlugin.ReserveAmmo;

			TranqGunPlugin.TranqDuration = GetConfigFloat("tranqgun_duration");

			List<string> spawns = new List<string>(GetConfigList("tranqgun_spawns"));
			if (spawns.Count == 1 && spawns[0] == "wheremyhotdogsat")
			{
				// -- Random defaults {173chamber|surfacenuke|nuke}
				List<string> hotdogDefault = new List<string> { "173chamber", "surfacenuke", "nuke" };
				spawns = new List<string> { hotdogDefault[new Random().Next(0, hotdogDefault.Count)] };
			}
			TranqGunPlugin.SpawnLocations = spawns;
		}
	}
}
