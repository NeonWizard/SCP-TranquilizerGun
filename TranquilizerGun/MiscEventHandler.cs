using Smod2;
using Smod2.API;
using Smod2.Events;
using Smod2.EventHandlers;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;

namespace TranquilizerGun
{
	class MiscEventHandler : IEventHandlerWaitingForPlayers, IEventHandlerSetConfig
	{
		private readonly TranqGunPlugin plugin;

		public MiscEventHandler(TranqGunPlugin plugin) => this.plugin = plugin;

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			if (!this.plugin.GetConfigBool("tranqgun_enable")) this.plugin.pluginManager.DisablePlugin(plugin);
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
	}
}
