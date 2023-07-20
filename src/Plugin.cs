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

#pragma warning disable CS0618


[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace ElitistDiff;

[BepInPlugin(MOD_ID, "Elitist Difficulty", "0.0.3")]
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
        
        L("What was I doing again?");
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
