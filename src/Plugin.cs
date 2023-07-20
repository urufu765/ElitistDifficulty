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
using ElitistModules;

#pragma warning disable CS0618


[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace ElitistDifficulty;

[BepInPlugin(MOD_ID, "Elitist Difficulty", "0.1.0")]
public class Plugin : BaseUnityPlugin
{
    public static Plugin ins;
    public EliteConfig config;
    public const string MOD_ID = "urufudoggo.elitist";

    public static bool Patch_Guardian {get; private set;}
    public static bool Patch_MSC {get; private set;}

    public void OnEnable()
    {
        L("Start");
        try
        {
            ins = this;
        }
        catch (Exception e)
        {
            L(e, "Unable to instantiate plugin");
        }
        On.RainWorld.OnModsInit += LoadTheFrigginLoad;
        On.RainWorld.PostModsInit += CheckTheModPatches;
        L("Done");
        L("Hooking into hook required hooking methods begin!");
        On.Player.Update += Clocks;
        On.Player.TerrainImpact += FallToDeath;
        L("What was I doing again?");
    }

    private void FallToDeath(On.Player.orig_TerrainImpact orig, Player self, int chunk, IntVector2 direction, float speed, bool firstContact)
    {
        orig(self, chunk, direction, speed, firstContact);
        if (config.eliteFallKill.Value && !(Patch_MSC && self.abstractCreature.creatureTemplate.type == MoreSlugcats.MoreSlugcatsEnums.CreatureTemplateType.SlugNPC))
        {
            self.CheckDeathCondition(speed, firstContact);
        }
    }


    private void Clocks(On.Player.orig_Update orig, Player self, bool eu)
    {
        orig(self, eu);
        self.Tick();
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
        L("End");
    }

}
