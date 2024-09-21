using System;
using Mono.Cecil;
using EntityStates.Chef;
using JetBrains.Annotations;
using MonoMod.Cil;
using Rewired;
using ThreeEyedGames;
using Mono.Cecil.Cil;
using R2API.Utils;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace StormTweaks {
    public class CHEF
    {
        public static class Settings
        {
            public static float DiceDuration => Bind<float>("CHEF: Dice", "Dice Attack Duration", "The time it takes before another knife becomes throwable.", 0.3f);
            public static bool DiceEnabled => Bind<bool>("CHEF: Dice", "Dice Enabled", "Enable changes to this skill?", true);
            public static bool DiceRaiseProcCoefficient => Bind<bool>("CHEF: Dice", "Dice Raise Proc Coefficient", "Raise the proc coefficient on each cleaver", true);
            public static float DiceProcCoefficient => Bind<float>("CHEF: Dice", "Dice Proc Coefficient", "Proc coefficient of each cleaver (default is 0.5)", 1.0f);
            public static float SpecialCooldown => Bind<float>("CHEF: Yes Chef", "Special Cooldown", "Lower the cooldown of the alternate special.", 8f);
            public static bool InterruptableSkills => Bind<bool>("CHEF: General", "Skill Interrupts", "Make CHEF skills be able to interrupt each other.", true);
            public static bool RollEnabled => Bind<bool>("CHEF: Roll", "Roll Enabled", "Enable changes to this skill?", true);
            public static bool RollSpeed => Bind<bool>("CHEF: Roll", "Roll Scale Speed With Charge Tokens", "Increase the speed of Roll for each stage of Roll charge", true);
            public static float RollSpeedMultiplier => Bind<float>("CHEF: Roll", "Roll Speed Multiplier", "The multiplier applied to each Roll charge", 0.35f);
            public static bool RollDamage => Bind<bool>("CHEF: Roll", "Roll Scale Damage With Charge Tokens", "Increase the damage of Roll for each stage of Roll charge", true);
            public static float RollDamageBase => Bind<float>("CHEF: Roll", "Roll Added Base Damage", "Added base damage of Roll for each stage of Roll charge", 6.0f);
            public static float RollDamageMultiplier => Bind<float>("CHEF: Roll", "Roll Added Damage Multiplier", "Added multiplier used in the damage calculation for Roll when factoring in added damage on each stage of Roll charge", 2.0f);
            public static bool RollKnockback => Bind<bool>("CHEF: Roll", "Roll Increase Knockback", "Increase the knockback of Roll", false);
            public static float RollKnockbackValue => Bind<float>("CHEF: Roll", "Roll Increase Knockback Value", "The knockback force of Roll", 0.0f);
            public static bool RollExtendDuration => Bind<bool>("CHEF: Roll", "Roll Extend Duration With Charge Tokens", "Extend the Roll duration for each stage of Roll charge (along with a base extension)", true);
            public static float RollExtendDurationMultiplier => Bind<float>("CHEF: Roll", "Roll Extend Duration Multiplier", "The duration extension multiplier applied to Roll for each stage of Roll charge", 0.3f);
            public static bool RollOil => Bind<bool>("CHEF: Roll", "Oil Trail", "Roll should leave a trail of oil when boosted by Yes Chef?", true);
            public static bool RollCanCancel => Bind<bool>("CHEF: Roll", "Roll Can Cancel", "Allow the user to cancel RolyPoly mid-roll by hitting the activation button again", true);
            public static bool SearEnabled => Bind<bool>("CHEF: Sear", "Sear Enabled", "Enable changes to this skill?", true);
            public static float SearDistance => Bind<float>("CHEF: Sear", "Sear Max Distance", "The distance Sear should damage targets.", 22);
            public static bool SearNoDirLock => Bind<bool>("CHEF: Sear", "Sear No Direction Lock", "Make Sear remain omnidirectional even during sprint.", true);
            public static bool SearDamageFactorDistance => Bind<bool>("CHEF: Sear", "Sear Factor in Distance when Applying Sear Damage", "Factor in distance when applying damage using Sear so that targets closer receive more damage than targets further away", true);
            public static float SearDistanceDamageMultiplier => Bind<float>("CHEF: Sear", "Sear Distance Damage Multiplier", "Multiplier of how much damage should scale based on distance (the value here is the maximum damage multiplier one could achieve by being right next to an enemy)", 3.0f);
            public static bool SearBuffDamageOverTime => Bind<bool>("CHEF: Sear", "Sear Apply Burning Damage Over Time", "Make the damage over time modifier for Sear apply additional fire damage for a longer period", true);
            public static float SearDamageOverTimeValue => Bind<float>("CHEF: Sear", "Sear Damage Over Time Value", "The raw damage value applied on each tick to the Sear burning effect", 10.0f);
            public static float SearDamageOverTimeDuration => Bind<float>("CHEF: Sear", "Sear Damage Over Time Duration", "The duration applied to the Sear burning effect (note: this is a raw DoT duration value, which is *NOT* the amount of seconds the effect will persist for)", 0.0f);
            public static bool SearCanCancel => Bind<bool>("CHEF: Sear", "Sear Can Cancel", "Allow the user to cancel Sear mid-Sear by either releasing the skill activation input (in hold mode), or by activating the skill again (in toggle mode)", true);
            // public static bool SearHoldMode => Bind<bool>("CHEF: Sear", "Sear Hold Mode", "Enabling Sear requires the user hold down the Sear skill input activation (disabling this setting will put Sear into toggle activation mode)", true);
            public static float SearBaseExitDuration => Bind<float>("CHEF: Sear", "Sear Base Exit Duration", "Sear base exit duration", 0.4f);
            public static float SearBaseFlamethrowerDuration => Bind<float>("CHEF: Sear", "Sear Base Flamethrower Duration", "Sear base flamethrower duration", 3.0f);
            public static float SearTickDamageCoefficient => Bind<float>("CHEF: Sear", "Sear Tick Damage Coefficient", "Sear tick damage coefficient (note: this setting is enabled upon a configured value greater than zero--default is 6)", 0.0f);
            public static float SearTickFrequency => Bind<float>("CHEF: Sear", "Sear Tick Frequency", "Sear tick frequency (note: this setting is enabled upon a configured value greater than zero--default is 8)", 0.0f);
            public static bool GlazeEnabled => Bind<bool>("CHEF: Glaze", "Glaze Enabled", "Enable changes to this skill?", true);
            public static bool GlazeKnockbackSelf => Bind<bool>("CHEF: Glaze", "Glaze Knockback Self", "Apply knockback to Chef while firing Glaze", true);
            public static bool GlazeKnockbackAmplified => Bind<bool>("CHEF: Glaze", "Glaze Amplified Knockback", "Apply additional knockback to Chef when firing Glaze while looking at the ground rather than directly ahead", true);
            public static Int32 GlazeStandardKnockbackAmount => Bind<Int32>("CHEF: Glaze", "Glaze Standard Knockback Amount", "Standard knockback applied to chef while firing Glaze", -500);
            public static Int32 GlazeAmplifiedKnockbackAmount => Bind<Int32>("CHEF: Glaze", "Glaze Amplified Knockback Amount", "Amplified knockback applied to chef while firing Glaze while looking down", -800);
            public static bool GlazeKnockbackSuicidePrevention => Bind<bool>("CHEF: Glaze", "Glaze Knockback Suicide Prevention", "Remove knockback from Glaze when looking upwards, reducing the chance of applying too much knockback while already falling", true);
            public static bool GlazeKnockbackAlways => Bind<bool>("CHEF: Glaze", "Glaze Knockback Always", "Always apply knockback when using Glaze regardless of if the user is in the air or not", false);
            public static float GlazeDamageCoefficient => Bind<float>("CHEF: Glaze", "Glaze Damage Coefficient", "Damage coefficient for each ball of Glaze oil (default is 3 which means 300% damage)", 3.0f);
        }

        private static CleaverSkillDef DiceStandard;
        private static GameObject OilTrailSegment;
        private static GameObject OilTrailSegmentGhost;
        public static DamageAPI.ModdedDamageType GlazeOnHit = DamageAPI.ReserveDamageType();
        public static void Init() {
            try
            {
                SurvivorDef CHEF = Paths.SurvivorDef.Chef;

                Paths.SkillDef.YesChef.baseRechargeInterval = Settings.SpecialCooldown;

                /*
                 * Ensure all of our settings are auto-populated into the BepInEx default config file
                 */

                foreach (var field in typeof(Settings).GetProperties())
                {
                    try
                    {
                        field.GetValue(null);
                    }
                    catch { }
                }

                if (Settings.DiceEnabled)
                {
                    On.EntityStates.Chef.Dice.FixedUpdate += Dice_FixedUpdate;
                    On.ChefController.Update += ChefController_Update;
                    On.EntityStates.Chef.Dice.OnExit += Dice_OnExit;
                    On.EntityStates.Chef.Dice.OnEnter += Dice_OnEnter;
                    On.RoR2.Projectile.CleaverProjectile.Start += CleaverProjectile_Start;
                    On.RoR2.Projectile.CleaverProjectile.OnDestroy += CleaverProjectile_OnDestroy;

                    DiceStandard = ScriptableObject.CreateInstance<CleaverSkillDef>();
                    DiceStandard.Clone(Paths.SkillDef.ChefDice);
                    DiceStandard.mustKeyPress = false;
                    DiceStandard.rechargeStock = 0;
                    DiceStandard.stockToConsume = 0;

                    ContentAddition.AddSkillDef(DiceStandard);

                    Paths.SkillFamily.ChefPrimaryFamily.variants[0].skillDef = DiceStandard;

                    Paths.GameObject.ChefBody.AddComponent<ChefCleaverStorage>();

                    LanguageAPI.Add(Paths.SkillDef.ChefDice.skillDescriptionToken, 
                        "Throw up to <style=cIsUtility>3</style> cleavers for <style=cIsDamage>250% damage</style>. Release to recall the cleavers, dealing <style=cIsDamage>375% damage</style> on the return trip."
                    );
                }

                if (Settings.InterruptableSkills)
                {
                    Paths.SkillDef.ChefSear.interruptPriority = InterruptPriority.PrioritySkill;
                    Paths.SkillDef.ChefSearBoosted.interruptPriority = InterruptPriority.Frozen;
                    Paths.SkillDef.ChefRolyPoly.interruptPriority = InterruptPriority.PrioritySkill;
                    Paths.SkillDef.ChefRolyPolyBoosted.interruptPriority = InterruptPriority.Frozen;
                    Paths.SkillDef.ChefGlaze.interruptPriority = InterruptPriority.PrioritySkill;

                    On.EntityStates.Chef.Sear.GetMinimumInterruptPriority += Sear_GetMinimumInterruptPriority;
                    On.EntityStates.Chef.RolyPoly.GetMinimumInterruptPriority += RolyPoly_GetMinimumInterruptPriority;
                    On.EntityStates.Chef.Glaze.GetMinimumInterruptPriority += Glaze_GetMinimumInterruptPriority;
                    On.EntityStates.Chef.ChargeRolyPoly.GetMinimumInterruptPriority += ChargeGlaze_GetMinimumInterruptPriority;
                }

                if (Settings.SearEnabled)
                {
                    On.EntityStates.Chef.Sear.Update += Sear_Update;

                    On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
                    On.RoR2.DotController.InflictDot_refInflictDotInfo += DotController_InflictDot_refInflictDotInfo;

                    On.EntityStates.Chef.Sear.OnEnter += Sear_OnEnter;

                    LanguageAPI.Add(Paths.SkillDef.ChefSear.skillDescriptionToken, 
                        "<style=cIsDamage>Ignite</style>. Scorch enemies for <style=cIsDamage>2000%-6000% damage</style>, based on distance from the target. Glazed enemies take extra damage."
                    );

                    if (Settings.SearNoDirLock)
                    {
                        IL.EntityStates.Chef.Sear.FirePrimaryAttack += Sear_FirePrimaryAttack;
                    }

                    // Determine whether or not the user wants to put this into toggle mode (NOTE: THIS WILL REQUIRE A CHANGE TO THE SEAR CAN CANCEL SETTING)
                    Paths.SkillDef.ChefSear.mustKeyPress = true; // SearHoldMode;
                }

                if (Settings.DiceRaiseProcCoefficient)
                {
                    GameObject cleaverProjectile = Addressables.LoadAssetAsync<GameObject>((object)"RoR2/DLC2/Chef/ChefCleaver.prefab").WaitForCompletion();
                    cleaverProjectile.GetComponent<ProjectileController>().procCoefficient = Settings.DiceProcCoefficient;
                }

                if (Settings.GlazeEnabled)
                {
                    On.EntityStates.Chef.Glaze.FireGrenade += Glaze_FireGrenade;

                    LanguageAPI.Add(Paths.SkillDef.ChefGlaze.skillDescriptionToken, 
                        "Fire globs of oil in quick succession, dealing <style=cIsDamage>7x300% damage</style> and <style=cIsDamage>Weakening</style> enemies. Employs a hefty knockback when mid-air."
                    );
                }

                if (Settings.RollEnabled)
                {
                    if (Settings.RollOil)
                    {
                        OilTrailSegment = PrefabAPI.InstantiateClone(Paths.GameObject.CrocoLeapAcid, "OilTrailSegment");
                        OilTrailSegmentGhost = PrefabAPI.InstantiateClone(Paths.GameObject.CrocoLeapAcidGhost, "OilTrailSegmentGhost");

                        OilTrailSegmentGhost.FindComponent<Decal>("Decal").Material = Paths.Material.matClayBossGooDecal;
                        OilTrailSegmentGhost.FindParticle("Spittle").gameObject.SetActive(false);
                        OilTrailSegmentGhost.FindParticle("Gas").gameObject.SetActive(false);
                        OilTrailSegmentGhost.FindComponent<Light>("Point Light").gameObject.SetActive(false);
                        OilTrailSegmentGhost.GetComponent<ProjectileGhostController>().inheritScaleFromProjectile = true;

                        OilTrailSegment.GetComponent<ProjectileDamage>().damageType = DamageType.ClayGoo | DamageType.Silent;
                        OilTrailSegment.transform.localScale *= 0.5f;

                        OilTrailSegment.GetComponent<ProjectileController>().ghostPrefab = OilTrailSegmentGhost;
                        OilTrailSegment.GetComponent<ProjectileDotZone>().damageCoefficient = 0f;
                        OilTrailSegment.GetComponent<ProjectileDotZone>().impactEffect = null;
                        OilTrailSegment.GetComponent<ProjectileDotZone>().fireFrequency = 20;
                        OilTrailSegment.GetComponent<ProjectileDotZone>().overlapProcCoefficient = 0;
                        OilTrailSegment.RemoveComponents<AkEvent>();
                        OilTrailSegment.RemoveComponent<AkGameObj>();
                        OilTrailSegment.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>().Add(GlazeOnHit);

                        GlobalEventManager.onServerDamageDealt += OnDamageDealt;
    
                        ContentAddition.AddProjectile(OilTrailSegment);
                        Paths.GameObject.ChefBody.AddComponent<ChefTrailBehaviour>();
                    }

                    On.EntityStates.Chef.RolyPoly.OnEnter += RolyPoly_OnEnter;
                    On.EntityStates.Chef.RolyPoly.FixedUpdate += RolyPoly_FixedUpdate;

                    LanguageAPI.Add(Paths.SkillDef.ChefRolyPoly.skillDescriptionToken, 
                        "Charge the rolling pin up to three times to <style=cIsUtility>speed</style> forward, dealing <style=cIsDamage>600%-1200%</style> damage and <style=cIsDamage>Stunning</style> any enemies on hit."
                    );
                }
            }
            catch (Exception e)
            {
                ModLogger.LogError($"StormTweaks::CHEF.Init() failure - {e}");
            }
        }

        private static void Glaze_FireGrenade(On.EntityStates.Chef.Glaze.orig_FireGrenade orig, Glaze self, string targetMuzzle)
        {
            // Up the damage of each ball of glaze
            Glaze.damageCoefficient = Settings.GlazeDamageCoefficient;

            if (Settings.GlazeKnockbackSelf)
            {
                // apply knockback if in the air/jumping and you use glaze
                if (self.characterBody.characterMotor.isGrounded == false || Settings.GlazeKnockbackAlways == true)
                {
                    Vector3 aimRayDirection = self.GetAimRay().direction;

                    if (Settings.GlazeKnockbackAmplified)
                    {
                        // check if the user is aiming kinda downward
                        if (aimRayDirection.y <= -0.7f)
                        {
                            // this is how railgunner applies similar knockback force :)
                            self.characterBody.characterMotor.ApplyForce(Settings.GlazeAmplifiedKnockbackAmount * aimRayDirection, false, false);
                        }
                        // prevent the user from killing themselves with it by only applying backwards trajectory at non-near-perfect-upward angles
                        else if (Settings.GlazeKnockbackSuicidePrevention && aimRayDirection.y < 0.8f)
                        {
                            self.characterBody.characterMotor.ApplyForce(Settings.GlazeStandardKnockbackAmount * aimRayDirection, false, false);
                        }
                    }
                    else
                    {
                        self.characterBody.characterMotor.ApplyForce(Settings.GlazeStandardKnockbackAmount * aimRayDirection, false, false);
                    }
                }
            }

            orig(self, targetMuzzle);
        }

        private static void RolyPoly_FixedUpdate(On.EntityStates.Chef.RolyPoly.orig_FixedUpdate orig, RolyPoly self)
        {
            // Allow the user to cancel roll
            if (Settings.RollCanCancel == true && self.inputBank.skill3.justPressed == true)
            {
                self.duration = 0;
            }

            orig(self);
        }

        private static void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            // Make sear factor in distance to the enemy, and apply extra damage to enemies closer to the fire
            if (Settings.SearDamageFactorDistance == true
                && damageInfo.attacker
                && damageInfo.attacker.name.Contains("ChefBody")
                && damageInfo.damageType.damageType == RoR2.DamageType.IgniteOnHit
                && damageInfo.damageType.damageTypeExtended == RoR2.DamageTypeExtended.ChefSearDamage)
            {
                // Buff base sear damage and also factor in distance
                float distanceToAttacker = Vector3.Distance(self.transform.position, damageInfo.attacker.transform.position);

                float maxDistance = Settings.SearDistance + 1;
                float minDistance = 0;

                float clampedDistance = Math.Max(Math.Min(distanceToAttacker, maxDistance), minDistance);
                float maxDistanceDamageMultiplier = Settings.SearDistanceDamageMultiplier;
            
                float distanceFactor = 1 + (maxDistanceDamageMultiplier - 1) * (maxDistance - clampedDistance) / (maxDistance - minDistance);

                // damageInfo.damage = (damageInfo.damage * 2) * distanceFactor;
                damageInfo.damage *= distanceFactor;
            }

            orig(self, damageInfo);
        }

        private static void DotController_InflictDot_refInflictDotInfo(On.RoR2.DotController.orig_InflictDot_refInflictDotInfo orig, ref InflictDotInfo inflictDotInfo)
        {
            // Make the sear effect take longer and apply more damage
            if (Settings.SearBuffDamageOverTime)
            {
                if (inflictDotInfo.attackerObject.name.Contains("ChefBody") && inflictDotInfo.dotIndex == RoR2.DotController.DotIndex.Burn) {
                    inflictDotInfo.totalDamage = Settings.SearDamageOverTimeValue;
                    inflictDotInfo.duration = Settings.SearDamageOverTimeDuration; // 0.0010f

                    /*
                     * Looking at this mod to figure out how to apply DoT burning (or other) effects? Check out the game's GlobalEventManager::ProcIgniteOnKill function
                     * An example derived from this function:
			         *  InflictDotInfo inflictDotInfo = new InflictDotInfo
			         *  {
			         *  	victimObject = hurtBox.healthComponent.gameObject,
			         *  	attackerObject = damageReport.attacker,
			         *  	totalDamage = 10.0f,
			         *  	dotIndex = DotController.DotIndex.Burn,
			         *  	damageMultiplier = 1.0f
			         *  };
                     *
			         *  DotController.InflictDot(ref inflictDotInfo);
                     */
                }
            }

            orig(ref inflictDotInfo);
        }

        private static void RolyPoly_OnEnter(On.EntityStates.Chef.RolyPoly.orig_OnEnter orig, RolyPoly self)
        {
            float prevCharge = self.charge;

            if (Settings.RollSpeed)
            {
                // make rolypoly speed scale with each token of charge
                self.charge = 0;
                self.speedMultiplier += (prevCharge + 1) * Settings.RollSpeedMultiplier;
            }

            if (Settings.RollDamage)
            {
                // up the damage on each token of charge for rolypoly
                RolyPoly.chargeDamageCoefficient = Settings.RollDamageBase + (prevCharge * Settings.RollDamageMultiplier);
            }

            if (Settings.RollKnockback)
            {
                RolyPoly.knockbackForce = Settings.RollKnockbackValue;
            }

            if (Settings.RollExtendDuration)
            {
                // make rolypoly slightly longer with every charge
                self.baseDuration += (prevCharge + 1) * Settings.RollExtendDurationMultiplier;
            }

            orig(self);
        }

        private static void Sear_OnEnter(On.EntityStates.Chef.Sear.orig_OnEnter orig, Sear self)
        {
            Sear.maxDistance = Settings.SearDistance;

            // up the flamethrower duration
            Sear.baseExitDuration = Settings.SearBaseExitDuration;
            Sear.baseFlamethrowerDuration = Settings.SearBaseFlamethrowerDuration;

            if (Settings.SearTickDamageCoefficient > 0)
            {
                self.tickDamageCoefficient = Settings.SearTickDamageCoefficient;
            }

            if (Settings.SearTickFrequency > 0)
            {
                Sear.tickFrequency = Settings.SearTickFrequency;
            }

            orig(self);
        }

        private static void Sear_FirePrimaryAttack(ILContext il)
        {
            ILCursor c = new(il);

            c.TryGotoNext(MoveType.Before, 
                x => x.MatchCallOrCallvirt(out _),
                x => x.MatchDup(),
                x => x.MatchLdcR4(0)
            );

            c.Emit(OpCodes.Ldarg_0);
            c.EmitDelegate<Func<Vector3, Sear, Vector3>>((inVec, instance) => {
                return instance.GetAimRay().direction;
            });
        }

        private static void Sear_Update(On.EntityStates.Chef.Sear.orig_Update orig, Sear self)
        {
            // allow the user to cancel sear
            if (Settings.SearCanCancel)
            {
                // if (self.inputBank.skill2.down == false)
                // {
                //     self.flamethrowerDuration = 0;
                // }
            }

            orig(self);

            if (Settings.SearNoDirLock)
            {
                if (self.flamethrowerEffectInstance) {
                    self.flamethrowerEffectInstance.forward = self.GetAimRay().direction;
                }
            }
        }

        private static void OnDamageDealt(DamageReport report)
        {
            if (report.damageInfo.HasModdedDamageType(GlazeOnHit) && report.victimBody) {
                report.victimBody.AddTimedBuff(DLC2Content.Buffs.CookingOiled, 3f, 1);
            }
        }

        public class ChefTrailBehaviour : MonoBehaviour {
            public ChefController chef;
            public Timer timer = new(0.1f, false, true, false, true);
            public void Start() {
                chef = GetComponent<ChefController>();
            }
            public void FixedUpdate() {
                if (chef.rolyPolyActive && chef.yesChefHeatActive && chef.hasAuthority) {
                    if (timer.Tick()) {
                        FireProjectileInfo info = new();
                        info.damage = chef.characterBody.damage;
                        info.crit = false;
                        info.rotation = Quaternion.identity;
                        info.position = chef.characterBody.footPosition;
                        info.projectilePrefab = OilTrailSegment;
                        info.owner = base.gameObject;
                        
                        ProjectileManager.instance.FireProjectile(info);
                    }
                }
            }
        }

        private static InterruptPriority RolyPoly_GetMinimumInterruptPriority(On.EntityStates.Chef.RolyPoly.orig_GetMinimumInterruptPriority orig, RolyPoly self)
        {
            return InterruptPriority.PrioritySkill;
        }

        private static InterruptPriority Glaze_GetMinimumInterruptPriority(On.EntityStates.Chef.Glaze.orig_GetMinimumInterruptPriority orig, Glaze self)
        {
            return InterruptPriority.PrioritySkill;
        }

        private static InterruptPriority ChargeGlaze_GetMinimumInterruptPriority(On.EntityStates.Chef.ChargeRolyPoly.orig_GetMinimumInterruptPriority orig, ChargeRolyPoly self)
        {
            return InterruptPriority.PrioritySkill;
        }

        private static InterruptPriority Sear_GetMinimumInterruptPriority(On.EntityStates.Chef.Sear.orig_GetMinimumInterruptPriority orig, Sear self)
        {
            return InterruptPriority.PrioritySkill;
        }

        private static void Dice_OnEnter(On.EntityStates.Chef.Dice.orig_OnEnter orig, Dice self)
        {
            orig(self);

            ChefCleaverStorage.Reset(self.characterBody, self.chefController.yesChefHeatActive ? 0.8f : 0.4f);
        }

        private static void CleaverProjectile_OnDestroy(On.RoR2.Projectile.CleaverProjectile.orig_OnDestroy orig, CleaverProjectile self)
        {
            orig(self);

            if (self.chefController) {
                CharacterBody body = self.chefController.characterBody;
                if (ChefCleaverStorage.cleaverMap.ContainsKey(body)) {
                    if (!ChefCleaverStorage.cleaverMap[body].Contains(self)) return;
                    ChefCleaverStorage.cleaverMap[body].Remove(self);
                }
            }
        }

        private static void CleaverProjectile_Start(On.RoR2.Projectile.CleaverProjectile.orig_Start orig, CleaverProjectile self)
        {
            orig(self);

            if (self.chefController) {
                CharacterBody body = self.chefController.characterBody;
                if (ChefCleaverStorage.cleaverMap.ContainsKey(body)) {
                    ChefCleaverStorage.cleaverMap[body].Add(self);
                }
            }
        }

        private static void Dice_OnExit(On.EntityStates.Chef.Dice.orig_OnExit orig, Dice self)
        {
            self.chefController.SetYesChefHeatState(false);

            if (NetworkServer.active) {
                self.characterBody.RemoveBuff(DLC2Content.Buffs.boostedFireEffect);
            }

            if (self.isAuthority) {
                self.chefController.ClearSkillOverrides();
            }

            self.PlayAnimation("Gesture, Override", "DiceReturnCatch", "DiceReturnCatch.playbackRate", self.duration);
		    self.PlayAnimation("Gesture, Additive", "DiceReturnCatch", "DiceReturnCatch.playbackRate", self.duration);
        }
        private static void ChefController_Update(On.ChefController.orig_Update orig, ChefController self)
        {
            orig(self);

            // Remove sticking cleavers
            self.recallCleaver = !self.characterBody.inputBank.skill1.down && ChefCleaverStorage.GetCanBodyRecall(self.characterBody);
        }

        private static void Dice_FixedUpdate(On.EntityStates.Chef.Dice.orig_FixedUpdate orig, Dice self)
        {
            self.recallInputPressed = false;
            self.recallBackupCountdown = 900f;
            orig(self);

            if (self.fixedAge >= Settings.DiceDuration / self.attackSpeedStat) {
                self.outer.SetNextStateToMain();
            }
        }

        public class ChefCleaverStorage : MonoBehaviour {
            public static Dictionary<CharacterBody, List<CleaverProjectile>> cleaverMap = new();
            public static Dictionary<CharacterBody, ChefCleaverStorage> storageMap = new();
            private CharacterBody body;
            public float timer = 0f;

            public void OnEnable() {
                body = GetComponent<CharacterBody>();
                cleaverMap.Add(body, new());
                storageMap.Add(body, this);
            }

            public void FixedUpdate() {
                if (timer >= 0f) {
                    timer -= Time.fixedDeltaTime;
                }
            }

            public static bool GetCanBodyRecall(CharacterBody body) {
                if (storageMap.ContainsKey(body)) {
                    return storageMap[body].timer <= 0f;
                }

                return true;
            }

            public static void Reset(CharacterBody body, float time = 0.4f) {
                if (storageMap.ContainsKey(body)) {
                    storageMap[body].timer = time;
                }
            }

            public void OnDestroy() {
                if (body && cleaverMap.ContainsKey(body)) {
                    cleaverMap.Remove(body);
                }

                storageMap.Remove(body);
            }

            public int GetCleaversActive() {
                if (body) {
                    return cleaverMap[body].Count;
                }

                return 0;
            }
        }

        // cleavers are replaced by this new skill definition:
        public class CleaverSkillDef : SkillDef {
            public class CleaverInstanceData : BaseSkillInstanceData {
                public ChefCleaverStorage controller;
            }
            public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
            {
                return new CleaverInstanceData {
                    controller = skillSlot.GetComponent<ChefCleaverStorage>()
                };
            }
            public override void OnFixedUpdate([NotNull] GenericSkill skillSlot, float deltaTime)
            {
                base.OnFixedUpdate(skillSlot, deltaTime);
                int clcount = (skillSlot.skillInstanceData as CleaverInstanceData).controller.GetCleaversActive();
                int count = skillSlot.maxStock - clcount;
                skillSlot.stock = Mathf.Clamp(count, 0, skillSlot.maxStock);
            }

            public void Clone(SkillDef from) {
                this.activationState = from.activationState;
                this.activationStateMachineName = from.activationStateMachineName;
                this.attackSpeedBuffsRestockSpeed = from.attackSpeedBuffsRestockSpeed;
                this.attackSpeedBuffsRestockSpeed_Multiplier = from.attackSpeedBuffsRestockSpeed_Multiplier;
                this.autoHandleLuminousShot = from.autoHandleLuminousShot;
                this.baseMaxStock = from.baseMaxStock;
                this.baseRechargeInterval = from.baseRechargeInterval;
                this.beginSkillCooldownOnSkillEnd = from.beginSkillCooldownOnSkillEnd;
                this.canceledFromSprinting = from.canceledFromSprinting;
                this.cancelSprintingOnActivation = from.cancelSprintingOnActivation;
                this.dontAllowPastMaxStocks = from.dontAllowPastMaxStocks;
                this.forceSprintDuringState = from.forceSprintDuringState;
                this.fullRestockOnAssign = from.fullRestockOnAssign;
                this.hideStockCount = from.hideStockCount;
                this.icon = from.icon;
                this.interruptPriority = from.interruptPriority;
                this.isCombatSkill = from.isCombatSkill;
                this.keywordTokens = from.keywordTokens;
                this.mustKeyPress = from.mustKeyPress;
                this.rechargeStock = from.rechargeStock;
                this.requiredStock = from.requiredStock;
                this.resetCooldownTimerOnUse = from.resetCooldownTimerOnUse;
                this.skillDescriptionToken = from.skillDescriptionToken;
                this.skillName = from.skillName;
                this.skillNameToken = from.skillNameToken;
                this.stockToConsume = from.stockToConsume;
            }
        }
    }
}
