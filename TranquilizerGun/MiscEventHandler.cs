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
			foreach (RandomItemSpawner.PositionPosIdRelation item in UnityEngine.Object.FindObjectOfType<RandomItemSpawner>().posIds)
			{
				if (item.posID == "Fireman")
				{
					this.plugin.Handler.CreateOfType(item.position.position, item.position.rotation);
					break;
				}
			}

			foreach (string sl in TranqGunPlugin.SpawnLocations)
			{
				if (sl.ToLower() == "049chamber")
				{

				}
				else if (sl.ToLower() == "173chamber")
				{

				}
				else if (sl.ToLower() == "surfacenuke")
				{

				}
				else if (sl.ToLower() == "nuke")
				{

				}
				else if (sl.ToLower() == "bathrooms")
				{

				}
				else
				{
					this.plugin.Info("Invalid spawn location: " + sl);
				}
			}
		}
	}
}
