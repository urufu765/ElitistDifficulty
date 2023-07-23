using System;
using EliteHelper;
using Menu.Remix.MixedUI;

namespace ElitistDifficulty;

public partial class EliteConfig
{
    private void Checkbox_Init()
    {
        chkEliteElectroKill = new(eliteElectroKill, default);
        chkEliteElectroKill.Hide();
        chkEliteFailEscape = new(eliteFailEscape, default);
        chkEliteFailEscape.Hide();
        chkEliteFallKill = new(eliteFallKill, default);
        chkEliteFallKill.Hide();
        chkEliteKarmaDrain = new(eliteKarmaDrain, default);
        chkEliteKarmaDrain.Hide();

        chkMiscDontSparePups = new(cfgMiscDontSparePups, new(xoffset + (xpadding * 0), yoffset - (ypadding * 3)))
        {
            description = "Don't restrict the increased difficulties to just players. Make slugpups suffer.".Swapper()
        };
    }
}
