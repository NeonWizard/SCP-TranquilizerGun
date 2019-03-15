using ItemManager;
using ItemManager.Recipes;
using ItemManager.Utilities;
using ServerMod2.API;
using Smod2.API;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using scp4aiur;

namespace TranquilizerGun
{
	public class TranquilizerGun : CustomWeapon
	{
		public override int MagazineCapacity => TranqGunPlugin.Magazine;
		public override float FireRate => TranqGunPlugin.FireRate;

		public int damage => TranqGunPlugin.Damage;

		public override bool OnPickup()
		{
			Player p = new SmodPlayer(base.PlayerObject);

			p.PersonalBroadcast(3, "<color=#fff8>You have picked up a tranquilizer gun.</color>", false);

			return base.OnPickup();
		}

		// TODO: Redirect any damage done to ragdoll to player, make invisible player unshootable
		protected override void OnValidShoot(GameObject target, ref float damage)
		{
			SmodPlayer p;
			try { p = new SmodPlayer(target); }
			catch { return; }
			damage = 0;

			// -- Exclude certain roles
			if (p.TeamRole.Role == Role.SCP_173 || p.TeamRole.Role == Role.SCP_079) return;

			// -- Timed ragdoll logic
			Timing.Run(Tranquilize(p));
		}

		private IEnumerable<float> Tranquilize(Player p)
		{
			GameObject target = (GameObject)p.GetGameObject();
			Vector loc = p.GetPosition();

			// -- Make them invisible
			p.SetGhostMode(true, true, false);

			// -- Spawn ragdoll
			int role = (int)p.TeamRole.Role;
			Class c = PlayerManager.localPlayer.GetComponent<CharacterClassManager>().klasy[role];

			GameObject ragdoll = Object.Instantiate(
				c.model_ragdoll,
				target.transform.position + c.ragdoll_offset.position,
				Quaternion.Euler(target.transform.rotation.eulerAngles + c.ragdoll_offset.rotation)
			);
			NetworkServer.Spawn(ragdoll);
			ragdoll.GetComponent<Ragdoll>().SetOwner(new Ragdoll.Info(p.PlayerId.ToString(), p.Name, new PlayerStats.HitInfo(), role, p.PlayerId));

			// -- Freeze them until duration is up
			float elapsed = 0;
			while (elapsed < TranqGunPlugin.TranqDuration)
			{
				elapsed += Time.deltaTime;
				p.Teleport(loc);
				p.SetCurrentItemIndex(-1); // disallow equipping anything

				yield return 0;
			}

			// -- Remove ghostmode and ragdoll
			p.SetGhostMode(false);
			NetworkServer.Destroy(ragdoll);
		}
	}
}
