# Better Chef
This mod intends to make Chef a much stronger, more viable survivor. We accomplished this by providing a variety of changes to Chef's base kit, along with a few minor additions to his skills; all of which is documented below. Further, much of Chef and his abilities have been outlined into a configuration file so that you can perfectly tweak Chef to your liking.

Note: this mod was originally based on the popular [StormTweaks](https://thunderstore.io/package/pseudopulse/StormTweaks/) mod, and several features of StormTweaks have been included within BetterChef, including the changes to Dice and the ability for an oil trail to appear on the ground while using roll.

Notable changes include:
- Dice duration/damage buffs, fixes to various bugs (including cleavers getting stuck), and increasing the proc coefficient to 1.0
- RolyPoly speed/duration/damage scaling with each charge token, and the ability to cancel roll
- Sear duration/damage buffs, burn damage over time, distance-based damage buffs, and the ability to cancel Sear
- Glaze damage buffs and smart knockback (scales based on whether or not Chef is in the air, looking upward (reduced for suicide prevention), or looking downwards (amplified in comparison to looking straight forward))

## Showcase
![flying with glaze knockback](https://i.imgur.com/JbL2Apm.gif)

![roll speed comparison](https://i.imgur.com/kyhuuRs.gif)

![sear demo](https://i.imgur.com/uU0tSVT.gif)

## Configuration
Below is each change more precisely, along with the section and variable name (which can be found in the configuration file for BetterChef) that allows you to modify the values for each change (or disable certain changes altogether).

You can find the configuration file for BetterChef within your BepInEx profile under the path `<"BepInEx" folder path>/config/ror2.BetterChef.cfg`.

## Changes to Dice
### Overview
- Throw up to 3 cleavers for 250% damage by holding down the left mouse button and recall them for 375% damage by letting go of the left mouse button
- Proc coefficient increased from 0.5 to 1.0
- Fixed delay with being unable to use other abilities after Sear is done
- Fixed most cleaver bugs
### Configuration Options
- **Dice Enabled** (bool, default: true) - Enable changes to this skill
- **Dice Attack Duration** (float, default: 0.3f) - The time it takes before another cleaver becomes throwable
- **Dice Raise Proc Coefficient** (bool, default: true) - Raise the proc coefficient on each cleaver
- **Dice Proc Coefficient** (float, default: 1.0f) - Proc coefficient of each cleaver

## Changes to Roll
### Overview
- Damage increased from 500%-800% to 600%-1200%, stunning enemies upon hit
- Damage, speed, and duration scale with each charge
- Can be canceled mid-roll
- Knockback to enemies can be configured
### Configuration Options
- **Roll Enabled** (bool, default: true) - Enable changes to this skill
- **Roll Scale Speed With Charge Tokens** (bool, default: true) - Increase the speed of roll for each stage of roll charge
- **Roll Speed Multiplier** (float, default: 0.35f) - The multiplier applied to each roll charge
- **Roll Scale Damage With Charge Tokens** (bool, default: true) - Increase the damage of roll for each stage of roll charge
- **Roll Added Base Damage** (float, default: 6.0f) - The added base damage of roll for each stage of roll charge
- **Roll Added Damage Multiplier** (float, default: 2.0f) - The added multiplier used in teh damage calculation for roll when factoring in added damage on each stage of roll charge
- **Roll Increase Knockback** (bool, default: false) - Increase the knockback of roll
- **Roll Increase Knockback Value** (float, default: 0.0f) - The increase in knockback of roll
- **Roll Extend Duration** (bool, default: true) - Extend the roll duration for each stage of roll charge
- **Roll Extend Duration Multiplier** (float, default: 0.3f) - The duration extension multiplier applied to roll for each stage of roll charge
- **Oil Trail** (bool, default: true) - Enables roll to leave a trail of oil when boosted by "Yes, Chef"
- **Roll Can Cancel** (bool, default: true) - Allow the user to cancel roll mid-roll by hitting the activation button again

## Changes to Sear
### Overview
- Scorch enemies for 2000%-6000% damage, based on distance from target. Glazed enemies take extra damage
- Base damage has been buffed
- Duration has been buffed from 1.3 seconds to 3 seconds
- The burn damage has been buffed and is now applied for longer (via a proper Damage-over-Time effect)
- You now have the ability to do more damage to targets who are closer to you and less to targets further away (up to 3-times more damage)
- Can be canceled mid-Sear
### Configuration Options
- **Sear Enabled** (bool, default: true) - Enable changes to this skill
- **Sear Max Distance** (float, default: 22.0f) - The distance Sear should damage targets
- **Sear No Direction Lock** (bool, default: true) - Make Sear remain omnidirectional even during sprint
- **Sear Factor in Distance when Applying Sear Damage** (bool, default: true) - Factor in distance when applying damage using Sear so that targets closer receive more damage than targets further away
- **Sear Distance Damage Multiplier** (float, default: 3.0f) - Multiplier of how much damage should scale based on distance (the value here is the maximum damage multiplier one could achieve by being right next to an enemy)
- **Sear Apply Burning Damage Over Time** (bool, default: true) - Make the damage over time modifier for Sear apply additional fire damage for a longer period
- **Sear Damage Over Time Value** (float, default: 10.0f) - The raw damage value applied on each tick to the Sear burning effect
- **Sear Damage Over Time Duration** (float, default: 0.0f) - The duration applied to the Sear burning effect (note: this is a raw DoT duration value, which is *NOT* the amount of seconds the effect will persist for)
- **Sear Can Cancel** (bool, default: true) - Allow the user to cancel Sear mid-Sear by either releasing the skill activation input (in hold mode), or by activating the skill again (in toggle mode)
- **Sear Base Exit Duration** (float, default: 0.4f) - Sear base exit duration
- **Sear Base Flamethrower Duration** (float, default: 3.0f) - Sear base flamethrower duration
- **Sear Tick Damage Coefficient** (float, default: 0.0f) - Sear tick damage coefficient (note: this setting is enabled upon a configured value greater than zero--default is 6)
- **Sear Tick Frequency** (float, default: 0.0f) - Sear tick frequency (note: this setting is enabled upon a configured value greater than zero--default is 8)

## Changes to Glaze
### Overview
- Fire globs of oil in quick succession, dealing 7x300% damage and weakening enemies
- Optionally employs knockback when mid-air, and this knockback value can scale depending on where the user is looking (reduced while looking straight up to prevent accidental suicide, standard knockback value when *not* looking at the ground, and amplified knockback value when looking downward)
- By default, knockback only applies when you are off the ground (you jumped, are falling, were struck into the air, etc.), but this can also be configured to happen all the time
- All of these modifications, along with the base damage coefficient, are configurable
### Configuration Options
- **Glaze Enabled** (bool, default: true) - Enable changes to this skill
- **Glaze Knockback Self** (bool, default: true) - Apply knockback to Chef while firing Glaze
- **Glaze Amplified Knockback** (bool, default: true) - Apply additional knockback to Chef when firing Glaze while looking downward (instead of upward or on a level plane)
- **Glaze Standard Knockback Amount** (Int32, default: -500) - The standard knockback applied to chef when firing Glaze (while not looking downward or upward)
- **Glaze Amplified Knockback Amount** (Int32, default: -800) - The amplified knockback applied to chef when firing Glaze (while looking downward)
- **Glaze Knockback Suicide Prevention** (bool, default: true) - Remove knockback from Glaze when looking upwards, reducing the chance of applying too much knockback while already falling resulting in accidental suicide
- **Glaze Knockback Always**  (bool, false) - Always apply knockback when using Glaze regardless of if the user is in the air or not
- **Glaze Damage Coefficient**  (float, 3.0f) - Damage coefficient for each ball of Glaze oil (default is 3 which means 300% damage)

## Comparisons to StormTweaks
- "Chef" renaming to capitalized "CHEF" removed
- "Yes, Chef" skill renaming removed
- All changes to other characters or core gameplay mechanics that don't have to do with Chef have been removed
- Kept "Yes, Chef" cooldown reduction (to 8 seconds)
- Kept changes to Dice and RolyPoly oil trail
- Kept Sear fixes to unlock your aim while Sear is ongoing
- Kept interruptibility of all skills
- Kept Sear distance extension (22m)
- Fixed bugs related to cleavers not returning to Chef after being thrown (ones left stuck in the air)
- Fixed long wait time incorrectly imposed on other skills after using Sear

## Build Instructions
Visual Studio 2022 Community Edition with .NET standard 2.1 can be used to build this project.

Additionally, please note that two Risk of Rain 2 official game assemblies need to be included within this project in order to build it from source. These two files are named `Decalicious.dll` and `Unity.Postprocessing.Runtime.dll`. You can find these assemblies within your game's installation directory. Example paths for these assemblies can be found below.
 - `C:\Program Files (x86)\Steam\steamapps\common\Risk of Rain 2\Risk of Rain 2_Data\Managed\Decalicious.dll`
 - `C:\Program Files (x86)\Steam\steamapps\common\Risk of Rain 2\Risk of Rain 2_Data\Managed\Unity.Postprocessing.Runtime.dll`

Including these assemblies within the existing project directory at `StormTweaks/StormTweaks` (adjacent to `Plugin.cs`) should automatically include them within Visual Studio's build chain.

## Known Bugs
- Occasionally it appears multiple cleavers can be incorrectly thrown in rapid succession
- Chef's cleaver icons have disappeared from the default crosshair reticle
- Glaze knockback, while experiencing lag in a multiplayer game, may launch you much further than was intended

# Acknowledgements
- Thank you to [pseudopulse's StormTweaks mod](https://thunderstore.io/package/pseudopulse/StormTweaks/), which this project is largely based on
- Thank you to [Nuxlar's BetterChefProc mod](https://thunderstore.io/package/Nuxlar/BetterChefProc/), which includes the logic we used to increase Chef's cleaver proc coefficient
