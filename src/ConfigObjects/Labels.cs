using System;
using EliteHelper;
using Menu.Remix.MixedUI;

namespace ElitistDifficulty;

public partial class EliteConfig
{
    private void Label_Init()
    {
        lblHard = new OpLabelLong(new(xoffset + (xpadding * 0), xoffset - (ypadding * 1)), new(400, yoffset),
            "- <REPLACE0>".Swapper(hardStrings)
        )
        {
            color = hardColor
        };
        lblHard.Hide();
        lblElite = new OpLabelLong(new(xoffset + (xpadding * 0), xoffset - (ypadding * 1)), new(400, yoffset),
            "- <REPLACE0><LINE>- <REPLACE1><LINE>- <REPLACE2><LINE>- <REPLACE3>".Swapper(eliteStrings)
        )
        {
            color = eliteColor
        };
        lblElite.Hide();
        lblMadland = new OpLabelLong(new(xoffset + (xpadding * 0), xoffset - (ypadding * 1)), new(400, yoffset),
            "- <REPLACE0><LINE>- <REPLACE1><LINE>- <REPLACE2><LINE>- <REPLACE3><LINE>- <REPLACE4><LINE>- <REPLACE5><LINE>- <REPLACE6><LINE>- <REPLACE7>".Swapper(madlandStrings)
        )
        {
            color = madlandColor
        };
        lblMadland.Hide();
        lblPlaceholder = new OpLabel(xoffset + (xpadding * 0), yoffset - (ypadding * 0), "Check back later!".Swapper());
        lblPlaceholder2 = new OpLabel(xoffset + (xpadding * 0), yoffset - (ypadding * 0), "Check back later!".Swapper());
    }
}
