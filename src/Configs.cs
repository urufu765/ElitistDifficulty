using System;
using BepInEx.Logging;
using Menu.Remix.MixedUI;
using Menu.Remix.MixedUI.ValueTypes;
using UnityEngine;
using static EliteHelper.Helper;

namespace ElitistDiff;

public class EliteConfig : OptionInterface
{
    public Configurable<Difficulty> cfgDifficulty;
    public Configurable<int> cfgLogImportance;
    public UIelement[] difficultySet, accessibilitySet, miscSet;
    public OpSimpleButton btnHard, btnElite, btnMadland;
    public OpLabel lblHard, lblElite, lblMadland;
    private readonly float yoffset = 560f;
    private readonly float xoffset = 30f;
    private readonly float ypadding = 40f;
    private readonly float xpadding = 35f;
    public readonly Color hardColor, eliteColor, madlandColor;


    public EliteConfig()
    {
        cfgDifficulty = config.Bind("elitecfg_Difficulty_Settings", Difficulty.Hard);
        cfgLogImportance = config.Bind("elitecfg_Log_Importance_Settings", 0, new ConfigAcceptableRange<int>(-1, 4));
        cfgLogImportance.OnChange += SetLogImportance;
        hardColor = new Color(0.9f, 0.9f, 0.9f);
        eliteColor = new Color(0.9f, 0.7f, 0.2f);
        madlandColor = new Color(1f, 0.25f, 0.2f);
    }

    private void SetLogImportance()
    {
        logImportance = cfgLogImportance.Value;
    }


    public override void Initialize()
    {
        string hardString = "- No changes to game.";
        string eliteString = "- Hard falls kill<LINE>- Death on failed escape opportunity<LINE>- Electric shocks kill<LINE>- Weaker to explosions<LINE>- Become fatigued after being tired<REPLACE0>";
        string madlandString = "<LINE>- Health depletes upon being attacked<LINE>- Karma resets to 1 on death (unless protected)<LINE>- Must always eat max food";
        base.Initialize();
        lblHard = new OpLabel(xoffset + (xpadding * 0), yoffset + (ypadding * 0), 
            hardString.Swapper()
        )
        {
            color = hardColor,
            Hidden = true
        };
        lblElite = new OpLabel(xoffset + (xpadding * 0), yoffset + (ypadding * 0), 
            eliteString.Swapper(" for too long")
        )
        {
            color = eliteColor,
            Hidden = true
        };
        lblMadland = new OpLabel(xoffset + (xpadding * 0), yoffset + (ypadding * 0), 
            eliteString.Swapper("") + madlandString.Swapper()
        )
        {
            color = madlandColor,
            Hidden = true
        };
    }
}