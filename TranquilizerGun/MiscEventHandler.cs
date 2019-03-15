using Smod2;
using Smod2.API;
using Smod2.Events;
using Smod2.EventHandlers;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;
using UnityEngine;

namespace TranquilizerGun
{
	class MiscEventHandler : IEventHandlerWaitingForPlayers, IEventHandlerSetConfig, IEventHandlerRoundStart
	{
		private readonly TranqGunPlugin plugin;

		public MiscEventHandler(TranqGunPlugin plugin) => this.plugin = plugin;

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			if (!this.plugin.GetConfigBool("tranqgun_enable")) this.plugin.pluginManager.DisablePlugin(plugin);

			plugin.ReloadConfig();
		}

		public void OnSetConfig(SetConfigEvent ev)
		{
			// TODO: Instead of setting config, check if config exists. If not, teleport people to 0,0,0 for tranq duration
			// -- Allow ghost mode to make tranquilized players invisible
			if (ev.Key == "sm_enable_ghostmode")
			{
				ev.Value = true;
			}
		}

		public void OnRoundStart(RoundStartEvent ev)
		{
			RandomItemSpawner ris = UnityEngine.Object.FindObjectOfType<RandomItemSpawner>();

			List<SpawnLocation> spawns = new List<SpawnLocation>();
			foreach (string slRaw in TranqGunPlugin.SpawnLocations)
			{
				string sl = slRaw.ToLower();
				this.plugin.Info("Spawning tranquilizer gun at: " + sl);
				List<SpawnLocation> choices = null;

				if (sl == "049chamber")
				{
					choices = ris
						.posIds
						.Where(x => x.posID == "049_Medkit")
						.Select(x => new SpawnLocation(x.position.position, x.position.rotation))
						.ToList();
				}
				else if (sl == "096chamber")
				{
					choices = ris
						.posIds
						.Where(x => x.posID == "Fireman")
						.Select(x => new SpawnLocation(x.position.position, x.position.rotation))
						.ToList();
				}
				else if (sl == "173armory")
				{
					choices = ris
						.posIds
						.Where(x => x.posID == "RandomPistol" && x.position.parent.parent.gameObject.name == "Root_173")
						.Select(x => new SpawnLocation(x.position.position, x.position.rotation))
						.ToList();
				}
				else if (sl == "surfacenuke")
				{
					Transform t = GameObject.Find("SCPSLNukeRoom/Table01 (2)").transform;
					choices = new List<SpawnLocation>() { new SpawnLocation(t.position + Vector3.up*2.5f, Quaternion.identity) };
				}
				else if (sl == "nuke")
				{
					choices = ris
						.posIds
						.Where(x => x.posID == "Nuke")
						.Select(x => new SpawnLocation(x.position.position, x.position.rotation))
						.ToList();
				}
				else if (sl == "bathrooms")
				{
					choices = ris
						.posIds
						.Where(x => x.posID == "toilet_keycard")
						.Select(x => new SpawnLocation(x.position.position, x.position.rotation))
						.ToList();
				}

				if (choices == null || choices.Count == 0)
				{
					this.plugin.Info("Invalid spawn location: " + sl);
					return;
				}

				spawns.Add(choices[UnityEngine.Random.Range(0, choices.Count)]);
			}

			foreach (SpawnLocation sl in spawns)
			{
				this.plugin.Handler.CreateOfType(sl.position, sl.rotation);
			}
		}
	}

	public struct SpawnLocation
	{
		public Vector3 position { get; private set; }
		public Quaternion rotation { get; private set; }
		public SpawnLocation(Vector3 p, Quaternion r)
		{
			this.position = p;
			this.rotation = r;
		}
	}
}
