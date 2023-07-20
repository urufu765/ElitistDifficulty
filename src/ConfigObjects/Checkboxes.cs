using System;
using Menu.Remix.MixedUI;

namespace ElitistDiff;

public partial class EliteConfig
{
    private UIelement[] Checkbox_Init()
    {
        chkEliteElectroKill = new(eliteElectroKill, default);
        chkEliteElectroKill.Hide();
        chkEliteFailEscape = new(eliteFailEscape, default);
        chkEliteFailEscape.Hide();
        chkEliteFallKill = new(eliteFallKill, default);
        chkEliteFallKill.Hide();
        chkEliteFatigue = new(eliteFatigue, default);
        chkEliteFatigue.Hide();
        return new UIelement[]{
            chkEliteElectroKill, chkEliteFailEscape, chkEliteFallKill, chkEliteFatigue
        };
    }
}
