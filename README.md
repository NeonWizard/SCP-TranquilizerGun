# Tranquilizer Gun
A custom weapon plugin for SCP:SL Smod2 servers that adds a tranquilizer gun. When a player is shot, they are temporarily ragdolled for the duration of the tranquilization. This gun has a configurable limited amount of uses and magazine size, as well as further configurations for weapon damage and fire rate.

## Installation
* **[Smod2](https://github.com/Grover-c13/Smod2) must be installed for this to work.**
* **[ItemManager](https://github.com/probe4aiur/ItemManager) must be installed for this to work.**

1. Grab the [latest release](https://github.com/NeonWizard/SCP-SCPSwap/releases/latest) of TranquilizerGun.
2. Place TranquilizerGun.dll in your server's `sm_plugins` folder.

## Commands
Command | Description
:---: | ---
`IMSPAWN 105 <playerID>` | Gives a tranquilizer gun to the player identified by `playerID`.

## Configuration
Config Option | Value Type | Default Value | Description
---: | :---: | :---: | ---
tranqgun_enable | Bool | True | Whether TranquilizerGun should be enabled on server start.
tranqgun_damage | Int | 0 | Damage dealt by the tranquilizer gun.
tranqgun_firerate | Float | 3 | Time (in seconds) between each shot. Can be ignored with magazine size of 1.
tranqgun_magazine | Int | 1 | Amount of shots per magazine.
tranqgun_reserveammo | Int | 2 | Default reserve ammo for each player.
tranqgun_duration | Float | 5 | Time (in seconds) the target is tranquilized for.
tranqgun_spawns | RList | 096chamber | RList of locations to spawn the gun. Valid locations are: 049chamber, 096chamber, 173armory, surfacenuke, nuke, bathrooms.

*Note that all configs should go in your server config file, not config_remoteadmin.txt

### Place any suggestions/problems in [issues](https://github.com/NeonWizard/SCP-TranquilizerGun/issues)!
