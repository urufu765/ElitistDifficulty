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

[BepInPlugin(MOD_ID, "Elitist Difficulty", "0.0.1")]
public class Plugin : BaseUnityPlugin
{
    public static Plugin ins;
    public EliteConfig config;
    public const string MOD_ID = "urufudoggo.elitist";

    public static bool patch_Guardian;


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
        L("Done");
    }

    private void LoadTheFrigginLoad(On.RainWorld.orig_OnModsInit orig, RainWorld self)
    {
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
