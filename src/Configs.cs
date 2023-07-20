using System;
using BepInEx.Logging;
using Menu.Remix.MixedUI;
using Menu.Remix.MixedUI.ValueTypes;
using UnityEngine;
using static EliteHelper.Helper;
using static ElitistDiff.Buttons;

namespace ElitistDiff;

public partial class EliteConfig : OptionInterface
{
    public Configurable<Difficulty> cfgDifficulty;
    public Configurable<int> cfgLogImportance;
    public UIelement[] difficultySet, hardDiffSet, eliteDiffSet, madDiffSet, customDiffSet, accessibilitySet, miscSet;
    public OpSimpleButton btnHard, btnElite, btnMadland, btnCustom;
    public OpLabelLong lblHard, lblElite, lblMadland;
    public OpLabel lblPlaceholder, lblPlaceholder2;
    public OpTab difficultyTab, accessibilityTab, miscTab;
    public string strHard, strElite, strMadland, strCustom;
    public string[] hardStrings, eliteStrings, madlandStrings;
    private readonly float yoffset = 560f;
    private readonly float xoffset = 30f;
    private readonly float ypadding = 40f;
    private readonly float xpadding = 35f;
    public Color hardColor, eliteColor, madlandColor, customColor, hardDeselectedColor, eliteDeselectedColor, madlandDeselectedColor, customSelectedColor;
    public Configurable<bool> eliteFallKill, eliteFailEscape, eliteElectroKill, eliteFatigue;
    public OpCheckBox chkEliteFallKill, chkEliteFailEscape, chkEliteElectroKill, chkEliteFatigue;
    //public Configurable<bool> madFatigue, madBombWeak, madHalfCycle, madKarmaDrain, madMaxFood;
    //public OpCheckBox chkMadFatigue, chkMadBombWeak, chkMadHalfCycle, chkMadKarmaDrain, chkMadMaxFood;
    //public Configurable<bool> customNoStop, customNoMiss;
    private bool inited;


    public EliteConfig()
    {
        cfgDifficulty = config.Bind("elitecfg_Difficulty_Settings", Difficulty.Hard);
        cfgLogImportance = config.Bind("elitecfg_Log_Importance_Settings", 0, new ConfigAcceptableRange<int>(-1, 4));
        cfgLogImportance.OnChange += SetLogImportance;
        hardColor = new Color(0.9f, 0.9f, 0.9f);
        hardDeselectedColor = new Color(0.3f, 0.3f, 0.3f);
        eliteColor = new Color(0.9f, 0.7f, 0.2f);
        eliteDeselectedColor = new Color(0.4f, 0.27f, 0.1f);
        madlandColor = new Color(1f, 0.25f, 0.2f);
        madlandDeselectedColor = new Color(0.5f, 0.1f, 0.1f);
        customColor = new Color(0f, 0.7f, 0.8f);
        customSelectedColor = new Color(0f, 0.3f, 0.4f);
        strHard = "Your regular Rain World experience";
        strElite = "Rain World tuned to be more brutal and unforgiving";
        //strMadland = "The painful experience taken to the extreme";
        strMadland = "Coming Soon...";
        //strCustom = "Customize your Rain World happy meal";
        strCustom = "Coming Soon...";

        eliteFallKill = config.Bind("elitecfg_elite_fallskill", false);
        eliteFailEscape = config.Bind("elitecfg_elite_killonescapefail", false);
        eliteElectroKill = config.Bind("elitecfg_elite_electricitykill", false);
        eliteFatigue = config.Bind("elitecfg_elite_fatigue", false);

        /*
        madFatigue = config.Bind("elitecfg_madland_fatigue", false);
        madBombWeak = config.Bind("elitecfg_madland_bombweakness", false);
        madHalfCycle = config.Bind("elitecfg_madland_fastcycle", false);
        madKarmaDrain = config.Bind("elitecfg_madland_karmadownthedrain", false);
        madMaxFood = config.Bind("elitecfg_madland_maxfoodorgohome", false);

        customNoStop = config.Bind("elitecfg_custom_dontstopmoving", false);
        customNoMiss = config.Bind("elitecfg_custom_hitallspears", false);
        */
    }

    private void SetLogImportance()
    {
        logImportance = cfgLogImportance.Value;
    }


    public override void Initialize()
    {
        hardStrings = new string[]{"No change."};
        eliteStrings = new string[]{"Hard falls kill", "Death on failed escape opportunity", "Electric shocks kill", "Fatigue builds up while you are tired"};
        madlandStrings = new string[]{eliteStrings[0], eliteStrings[1], eliteStrings[2], "Become exhausted when tired", "Weaker to explosions", "Cycles last half as long", "Karma resets to 1 on death (unless protected)", "Must always hibernate with max food"};
        base.Initialize();

        Label_Init();

        Button_Init();

        difficultyTab = new(this, Translate("Difficulty"));
        accessibilityTab = new(this, Translate("Accessibility"));
        miscTab = new(this, Translate("Miscellaneous"));
        Tabs = new OpTab[]
        {
            difficultyTab,
            accessibilityTab,
            miscTab
        };

        difficultySet = new UIelement[]
        {
            btnHard,
            btnElite,
            btnMadland,
            btnCustom
        };
        customDiffSet = Checkbox_Init();
        accessibilitySet = new UIelement[]
        {
            lblPlaceholder
        };
        miscSet = new UIelement[]
        {
            lblPlaceholder2
        };


        difficultyTab.AddItems(difficultySet);
        difficultyTab.AddItems(lblHard, lblElite, lblMadland);
        difficultyTab.AddItems(customDiffSet);
        accessibilityTab.AddItems(accessibilitySet);
        miscTab.AddItems(miscSet);
    }

    public override void Update()
    {
        base.Update();
        if (!inited)
        {
            switch (cfgDifficulty.Value)
            {
                case var x when x == Difficulty.Hard:
                    this.Unset_Elite_Difficulty(true);
                    this.Set_Hard_Difficulty(true);
                    break;
                case var x when x == Difficulty.Elite:
                    this.Unset_Hard_Difficulty(true);
                    this.Set_Elite_Difficulty(true);
                    break;
            }
            inited = true;
        }
        // if (difficultyTab.isInactive && inited || !lblHard.myContainer.isVisible && !lblElite.myContainer.isVisible && !lblMadland.myContainer.isVisible)
        // {
        //     inited = false;
        // }
    }
}