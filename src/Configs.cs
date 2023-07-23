using System;
using BepInEx.Logging;
using Menu.Remix.MixedUI;
using Menu.Remix.MixedUI.ValueTypes;
using UnityEngine;
using static EliteHelper.Helper;
using static ElitistDifficulty.Buttons;

namespace ElitistDifficulty;

public partial class EliteConfig : OptionInterface
{
    public OpDragger chkDifficulty;
    public Configurable<int> cfgDifficulty;
    public Configurable<int> cfgLogImportance;
    public UIelement[] difficultySet, hardDiffSet, eliteDiffSet, madDiffSet, customDiffSet, accessibilitySet, miscSet;
    public OpSimpleButton btnHard, btnElite, btnMadland, btnCustom;
    public OpLabelLong lblHard, lblElite, lblMadland;
    public OpLabel lblPlaceholder;
    public OpTab difficultyTab, accessibilityTab, miscTab;
    public string strHard, strElite, strMadland, strCustom;
    public string[] hardStrings, eliteStrings, madlandStrings;
    private readonly float yoffset = 560f;
    private readonly float xoffset = 30f;
    private readonly float ypadding = 40f;
    private readonly float xpadding = 35f;
    private readonly float tpadding = 6f;
    public Color hardColor, eliteColor, madlandColor, customColor, selectedColor, hardDeselectedColor, eliteDeselectedColor, madlandDeselectedColor, customDeselectedColor;
    public Configurable<bool> eliteFallKill, eliteFailEscape, eliteElectroKill, eliteKarmaDrain, madFatigue;
    public OpCheckBox chkEliteFallKill, chkEliteFailEscape, chkEliteElectroKill, chkEliteKarmaDrain, chkMadFatigue;
    //public Configurable<bool> madFatigue, madBombWeak, madHalfCycle, madMaxFood;
    //public OpCheckBox chkMadFatigue, chkMadBombWeak, chkMadHalfCycle, chkMadMaxFood;
    //public Configurable<bool> customNoStop, customNoMiss;
    public Configurable<bool> cfgMiscDontSparePups;
    public OpCheckBox chkMiscDontSparePups;
    public bool inited;


    public EliteConfig()
    {
        cfgDifficulty = config.Bind("elitecfg_Difficulty_Settings", Difficulty.TRYHARD);
        cfgLogImportance = config.Bind("elitecfg_Log_Importance_Settings", 0, new ConfigAcceptableRange<int>(-1, 4));
        cfgLogImportance.OnChange += SetLogImportance;
        selectedColor = Color.white;
        hardColor = new Color(0.8f, 0.8f, 0.8f);
        hardDeselectedColor = new Color(0.2f, 0.2f, 0.2f);
        eliteColor = new Color(0.9f, 0.7f, 0.2f);
        eliteDeselectedColor = new Color(0.35f, 0.2f, 0.14f);
        madlandColor = new Color(1f, 0.25f, 0.2f);
        madlandDeselectedColor = new Color(0.4f, 0.1f, 0.1f);
        customColor = new Color(0f, 0.7f, 0.8f);
        customDeselectedColor = new Color(0f, 0.25f, 0.35f);
        strHard = "Your regular Rain World experience.";
        strElite = "Rain World tuned to be a little bit more unforgiving.";
        //strMadland = "The brutal experience taken to the extreme.";
        strMadland = "Coming Soon...";
        //strCustom = "Customize your Rain World happy meal.";
        strCustom = "Coming Soon...";

        eliteFallKill = config.Bind("elitecfg_elite_fallskill", false);
        eliteFailEscape = config.Bind("elitecfg_elite_killonescapefail", false);
        eliteElectroKill = config.Bind("elitecfg_elite_electricitykill", false);
        eliteKarmaDrain = config.Bind("elitecfg_elite_karmadownthedrain", false);

        madFatigue = config.Bind("elitecfg_madland_fatigue", false);
        /*
        madBombWeak = config.Bind("elitecfg_madland_bombweakness", false);
        madHalfCycle = config.Bind("elitecfg_madland_fastcycle", false);
        madMaxFood = config.Bind("elitecfg_madland_maxfoodorgohome", false);

        customNoStop = config.Bind("elitecfg_custom_dontstopmoving", false);
        customNoMiss = config.Bind("elitecfg_custom_hitallspears", false);
        */

        cfgMiscDontSparePups = config.Bind("elitecfg_Dont_Spare_Slugpups", false);
    }

    private void SetLogImportance()
    {
        logImportance = cfgLogImportance.Value;
    }


    public override void Initialize()
    {
        hardStrings = new string[]{"No change."};
        eliteStrings = new string[]{"Hard falls kill", "Death on failed escape opportunity", "Electric shocks kill", "Karma resets to 1 on death (unless protected)"};
        madlandStrings = new string[]{eliteStrings[0], eliteStrings[1], eliteStrings[2], eliteStrings[3], "Weaker to explosions", "Cycles last half as long", "Fatigue builds up while you are tired", "Must always hibernate with max food"};
        base.Initialize();

        Label_Init();

        Button_Init();

        Checkbox_Init();

        chkDifficulty = new(cfgDifficulty, default, 0);
        chkDifficulty.Hide();
        chkDifficulty.OnDeactivate += this.Revert_Button_Pressed;

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
            new OpLabel(xoffset + (xpadding * 0), yoffset - (ypadding * 0), "Difficulty".Swapper(), true),
            new OpLabelLong(new(xoffset + (xpadding * 0), yoffset - (ypadding * 2)), new(500f, ypadding * 2), "Choose your difficulty.".Swapper()),
            btnHard,
            btnElite,
            btnMadland,
            btnCustom
        };
        customDiffSet = new UIelement[]
        {
            chkEliteElectroKill, chkEliteFailEscape, chkEliteFallKill, chkEliteKarmaDrain, chkMadFatigue
        };
        accessibilitySet = new UIelement[]
        {
            lblPlaceholder
        };
        miscSet = new UIelement[]
        {
            new OpLabel(xoffset + (xpadding * 0), yoffset - (ypadding * 0), "Miscellaneous".Swapper(), true),
            new OpLabelLong(new(xoffset + (xpadding * 0), yoffset - (ypadding * 2)), new(500f, ypadding * 2), "Modify some stuff that may or may not have impact on the gameplay.".Swapper()),
            new OpLabel(xoffset + (xpadding * 8) + 7f, yoffset - (ypadding * 2) + tpadding, "Log To Console".Swapper()),
            new OpSliderTick(this.cfgLogImportance, new Vector2(xoffset + (xpadding * 0) + 7f, yoffset - (ypadding * 2)), 300 - (int)xpadding - 7)
            {
                min = -1,
                max = 4,
                description = "Controls what are allowed to be sent to the logs. -1 supresses all devlogs, even errors.".Swapper()
            },

            new OpLabel(xoffset + (xpadding * 1), yoffset - (ypadding * 3) + tpadding/2, "Don't Spare Slugpups".Swapper()),
            chkMiscDontSparePups
        };


        difficultyTab.AddItems(difficultySet);
        difficultyTab.AddItems(chkDifficulty, lblHard, lblElite, lblMadland);
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
                case Difficulty.TRYHARD:
                    this.Unset_Elite_Difficulty(true);
                    this.Set_Hard_Difficulty(true);
                    break;
                case Difficulty.ELITIST:
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