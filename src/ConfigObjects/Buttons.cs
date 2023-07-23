using System;
using Menu.Remix.MixedUI.ValueTypes;
using Menu.Remix;
using Menu.Remix.MixedUI;
using static EliteHelper.Helper;
using UnityEngine;

namespace ElitistDifficulty;

public partial class EliteConfig
{
    private void Button_Init()
    {
        btnHard = new OpSimpleButton(new Vector2(xoffset + (xpadding * 0), yoffset - (ypadding * 2)), new Vector2(115, 35), Translate("Hard"))
        {
            colorEdge = hardColor,
            colorFill = hardDeselectedColor,
            description = strHard
        };
        btnHard.OnClick += this.Button_Hard_Pressed;
        btnElite = new OpSimpleButton(new Vector2(xoffset + (xpadding * 4), yoffset - (ypadding * 2)), new Vector2(115, 35), Translate("Elite"))
        {
            colorEdge = eliteColor,
            colorFill = eliteDeselectedColor,
            description = strElite
        };
        btnElite.OnClick += this.Button_Elite_Pressed;
        btnMadland = new OpSimpleButton(new Vector2(xoffset + (xpadding * 8), yoffset - (ypadding * 2)), new Vector2(115, 35), Translate("Madland"))
        {
            colorEdge = madlandColor,
            colorFill = madlandDeselectedColor,
            description = strMadland,
            greyedOut = true
        };
        btnCustom = new OpSimpleButton(new Vector2(xoffset + (xpadding * 12), yoffset - (ypadding * 2)), new Vector2(115, 35), Translate("Custom"))
        {
            colorEdge = customColor,
            colorFill = customDeselectedColor,
            description = strCustom,
            greyedOut = true
        };
    }
}


public static class Buttons
{
    public static void Revert_Button_Pressed(this EliteConfig self)
    {
        self.inited = false;
    }


    public static void Button_Hard_Pressed(this EliteConfig self, UIfocusable _)
    {
        self.Unset_Elite_Difficulty();
        self.Unset_Madland_Difficulty();
        self.Set_Hard_Difficulty();
        if (self.chkDifficulty.GetValueInt() is not Difficulty.TRYHARD && RWCustom.Custom.rainWorld.processManager.currentMainLoop is Menu.ModdingMenu && SoundID.MENU_Continue_Game is not null){
            ConfigContainer.PlaySound(SoundID.MENU_Switch_Page_In, 0, 0.8f, 0.95f);
        }
        self.chkDifficulty.SetValueInt(Difficulty.TRYHARD);
    }

    public static void Button_Elite_Pressed(this EliteConfig self, UIfocusable _)
    {
        self.Unset_Hard_Difficulty();
        self.Unset_Madland_Difficulty();
        self.Set_Elite_Difficulty();
        if (self.chkDifficulty.GetValueInt() is not Difficulty.ELITIST && RWCustom.Custom.rainWorld.processManager.currentMainLoop is Menu.ModdingMenu && SoundID.MENU_Continue_Game is not null){
            ConfigContainer.PlaySound(SoundID.MENU_Switch_Page_In, 0, 0.8f, 0.7f);
        }
        self.chkDifficulty.SetValueInt(Difficulty.ELITIST);
    }

    public static void Set_Hard_Difficulty(this EliteConfig self, bool cosmetic = false)
    {
        //self.btnHard.colorEdge = self.selectedColor;
        self.btnHard.colorFill = self.hardColor;
        self.lblHard.Show();
    }

    public static void Unset_Hard_Difficulty(this EliteConfig self, bool cosmetic = false)
    {
        //self.btnHard.colorEdge = self.hardColor;
        self.btnHard.colorFill = self.hardDeselectedColor;
        self.lblHard.Hide();
    }

    public static void Set_Elite_Difficulty(this EliteConfig self, bool cosmetic = false)
    {
        //self.btnElite.colorEdge = self.selectedColor;
        self.btnElite.colorFill = self.eliteColor;
        self.lblElite.Show();
        if (cosmetic) return;
        self.chkEliteFallKill.SetValueBool(true);
        self.chkEliteFailEscape.SetValueBool(true);
        self.chkEliteElectroKill.SetValueBool(true);
        self.chkEliteKarmaDrain.SetValueBool(true);
    }

    public static void Unset_Elite_Difficulty(this EliteConfig self, bool cosmetic = false)
    {
        //self.btnElite.colorEdge = self.eliteColor;
        self.btnElite.colorFill = self.eliteDeselectedColor;
        self.lblElite.Hide();
        if (cosmetic) return;
        self.chkEliteFallKill.SetValueBool(false);
        self.chkEliteFailEscape.SetValueBool(false);
        self.chkEliteElectroKill.SetValueBool(false);
        self.chkEliteKarmaDrain.SetValueBool(false);
    }

    public static void Unset_Madland_Difficulty(this EliteConfig self, bool cosmetic = false)
    {
        self.btnMadland.colorFill = self.madlandDeselectedColor;
        self.lblMadland.Hide();
        if (cosmetic) return;
    }
}
