using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using RWCustom;
using BepInEx;
using Debug = UnityEngine.Debug;
using static EliteHelper.Helper;
using static ElitistDifficulty.PlayerCWT;
using ElitistModules;
using MonoMod.Cil;
using Mono.Cecil.Cil;

#pragma warning disable CS0618


[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace ElitistDifficulty;

[BepInPlugin(MOD_ID, "Elitist Difficulty", "0.5.4")]
public class Plugin : BaseUnityPlugin
{
    public static Plugin ins;
    public EliteConfig config;
    public const string MOD_ID = "urufudoggo.elitist";

    public static bool Patch_Guardian {get; private set;}
    public static bool Patch_MSC {get; private set;}


    public void OnEnable()
    {
        LL("Start");
        try
        {
            ins = this;
        }
        catch (Exception e)
        {
            LL(e, "Unable to instantiate plugin");
        }
        On.RainWorld.OnModsInit += LoadTheFrigginLoad;
        On.RainWorld.PostModsInit += CheckTheModPatches;
        LL("Done");
    }

    private void FlushKarmaDownTheDrainElegantly(On.SaveState.orig_SessionEnded orig, SaveState self, RainWorldGame game, bool survived, bool newMalnourished)
    {
        int deathKarma = -1;
        try
        {
            if (config.eliteKarmaDrain.Value && !survived && !self.deathPersistentSaveData.reinforcedKarma && game.IsStorySession)
            {
                L("Player got absolutely rekt", 1, true);
                deathKarma = self.deathPersistentSaveData.karma;
                self.deathPersistentSaveData.karma = 1;
            }
        }
        catch (Exception e)
        {
            L(e, "Oh no, couldn't flush the karma");
        }
        orig(self, game, survived, newMalnourished);
        if (deathKarma != -1) self.deathPersistentSaveData.karma = deathKarma;
    }

    private void FlushKarmaDownVisually(ILContext il)
    {
        var pear = new ILCursor(il);
        try
        {
            pear.GotoNext(MoveType.After,
                i => i.MatchLdarg(0),
                i => i.MatchLdfld<Menu.KarmaLadderScreen>(nameof(Menu.KarmaLadderScreen.karmaLadder)),
                i => i.MatchLdarg(0),
                i => i.MatchLdflda<Menu.KarmaLadderScreen>(nameof(Menu.KarmaLadderScreen.karma)),
                i => i.MatchLdfld<RWCustom.IntVector2>(nameof(IntVector2.x)),
                i => i.MatchLdcI4(1),
                i => i.MatchSub()
            );
            L("Identified point of interest", 1, true);
        }
        catch (Exception e)
        {
            L(e, "IL failed when dragging cursor through the code!");
            throw new Exception("Failure to match stuff!", e);
        }
        try
        {
            pear.Emit(OpCodes.Ldarg, 0);
        }
        catch (Exception e)
        {
            L(e, "IL failed when attempting to inject code!");
            throw new Exception("Failure to inject stuff!", e);
        }
        try
        {
            pear.EmitDelegate(
                (int original, Menu.SleepAndDeathScreen self) => {
                    if (ins.config.eliteKarmaDrain.Value && !self.karmaLadder.reinforced) {
                        L($"Player got abso rekt", 1);
                        //deathKarma = -1;
                        return 0;
                    }
                    return original;
                }
            );
        }
        catch (Exception e)
        {
            L(e, "IL failed when flushing karma down the drain!");
            throw new Exception("Failure to emite the delegate!", e);
        }
    }

#if false
    private void BreatheHeavily(On.PlayerGraphics.orig_Update orig, PlayerGraphics self)
    {
        orig(self);
        if (config.madFatigue.Value)
        {
            self.VisualizeFatigue();
        }
    }


    private void CausePlayerToFall(On.Player.orig_AerobicIncrease orig, Player self, float f)
    {
        orig(self, f);
        if (config.madFatigue.Value && SlugpupCheck(self))
        {
            self.SendPlayerDown(f);
        }
    }
#endif


    /// <summary>
    /// Applies death when player is shocked by jellyfish
    /// </summary>
    private void StunThePlayer(On.Player.orig_Stun orig, Player self, int st)
    {
        orig(self, st);
        if (config.eliteElectroKill.Value && SlugpupCheck(self) && self.GetCat().readyForShock > 0)
        {
            self.DeathByShock(st);
            self.GetCat().readyForShock = 0;
        }
        // if (config.madFatigue.Value && SlugpupCheck(self))
        // {
        //     self.SendPlayerDownForTheCount();
        // }
    }

    /// <summary>
    /// Checks if player is in jellyfish stun condition
    /// </summary>
    private void ShockThePlayer(On.JellyFish.orig_Update orig, JellyFish self, bool eu)
    {
        try
        {
            if (config.eliteElectroKill.Value && !self.Electric)
            {
                for (int i = 0; i < self.tentacles.Length; i++){
                    if (self.latchOnToBodyChunks[i] is not null && self.latchOnToBodyChunks[i].owner is Player player && SlugpupCheck(player))
                    {
                        player.GetCat().readyForShock = 5;
                    }
                }
            }
        }
        catch (Exception e)
        {
            L(e, "Couldn't shock player properly!");
        }
        orig(self, eu);
    }


    private void SmallCentiKillPlayer(On.Centipede.orig_Shock orig, Centipede self, PhysicalObject shockObj)
    {
        orig(self, shockObj);
        if (self.Small && shockObj is Player player && SlugpupCheck(player) && config.eliteElectroKill.Value)
        {
            player.DeathByShock(120);
        }
    }


    /// <summary>
    /// Checks if a slugpup should be affected or not.
    /// </summary>
    /// <param name="self">Player that may or may not be a slugpup</param>
    /// <returns>Should the difficulty affect a slugpup or not</returns>
    public static bool SlugpupCheck(Player self)
    {
        if (!Patch_MSC) return true;
        if (ins.config.cfgMiscDontSparePups.Value) return true;
        return self.abstractCreature.creatureTemplate.type != MoreSlugcats.MoreSlugcatsEnums.CreatureTemplateType.SlugNPC;
    }

    /// <summary>
    /// Apply all sorts of violence on players
    /// </summary>
    private void DeathByManyThings(On.Creature.orig_Violence orig, Creature self, BodyChunk source, Vector2? directionAndMomentum, BodyChunk hitChunk, PhysicalObject.Appendage.Pos hitAppendage, Creature.DamageType type, float damage, float stunBonus)
    {
        orig(self, source, directionAndMomentum, hitChunk, hitAppendage, type, damage, stunBonus);
        if (self is Player player && SlugpupCheck(player))
        {
            if (config.eliteElectroKill.Value)
            {
                player.CheckForShockDeath(type, stunBonus);
            }
        }
    }

    /// <summary>
    /// Applies death when getting stunned from falling.
    /// </summary>
    private void FallToDeath(On.Player.orig_TerrainImpact orig, Player self, int chunk, IntVector2 direction, float speed, bool firstContact)
    {
        orig(self, chunk, direction, speed, firstContact);
        if (config.eliteFallKill.Value && SlugpupCheck(self))
        {
            self.CheckDeathCondition(speed, firstContact);
        }
    }

    /// <summary>
    /// Ticks values.
    /// </summary>
    private void Clocks(On.Player.orig_Update orig, Player self, bool eu)
    {
        orig(self, eu);
        self.FallImmunityTick();
        self.TickShocker();
        if (config.eliteFailEscape.Value && SlugpupCheck(self))
        {
            self.DeathIfSavingThrowFail();
        }
        self.ManageFatigue();
    }


    private void CheckTheModPatches(On.RainWorld.orig_PostModsInit orig, RainWorld self)
    {
        orig(self);
        L("Start");
        try
        {
            if (ModManager.ActiveMods.Exists(mod => mod.id == "vigaro.guardian"))
            {
                L("Found Guardian! Applying patch...");
                Patch_Guardian = true;
            }
            if (ModManager.MSC)
            {
                L("Found MoreSlugcats! Applying patch...");
                Patch_MSC = true;
            }
        }
        catch (Exception e)
        {
            L(e, "Couldn't patch mods!");
        }
        L("End");
    }


    private void LoadTheFrigginLoad(On.RainWorld.orig_OnModsInit orig, RainWorld self)
    {
        orig(self);
        L("Start");
        try
        {
            this.config = new EliteConfig();
            MachineConnector.SetRegisteredOI(MOD_ID, this.config);
        }
        catch (Exception e)
        {
            L(e, "Configuration connection failed");
        }
        L("Hooking into hook required hooking methods begin!");
        On.Player.Update += Clocks;
        On.Player.TerrainImpact += FallToDeath;
        On.Creature.Violence += DeathByManyThings;
        On.Centipede.Shock += SmallCentiKillPlayer;
        On.JellyFish.Update += ShockThePlayer;
        On.Player.Stun += StunThePlayer;
        //On.Player.AerobicIncrease += CausePlayerToFall;
        //On.PlayerGraphics.Update += BreatheHeavily;
        On.SaveState.SessionEnded += FlushKarmaDownTheDrainElegantly;
        try
        {
            IL.Menu.SleepAndDeathScreen.FoodCountDownDone += FlushKarmaDownVisually;
        }
        catch (Exception e)
        {
            L(e, "An IL hook failed!");
        }
        L("What was I doing again?");
        L("End");
    }
}
