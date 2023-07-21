using System;
using static EliteHelper.Helper;
using ElitistDifficulty;
using UnityEngine;

namespace ElitistModules;

public static class Clock
{
    public static int FallImmunity {get; private set;} = 0;
    public static string LastRegion {get; private set;} = "";

    public static void FallImmunityTick(this Player self)
    {
        if (FallImmunity > 0)
        {
            FallImmunity--;
        }

        try
        {
            if (self?.room?.world?.region is not null)
            {
                if (self.room.world.region.name != LastRegion)
                {
                    LastRegion = self.room.world.region.name;
                    FallImmunity = 400;
                }
            }
        }
        catch (Exception ex)
        {
            self.L(ex, "Couldn't check if player changed regions");
        }
    }

    public static void TickShocker(this Player self)
    {
        if (self.GetCat().readyForShock > 0)
        {
            self.GetCat().readyForShock--;
        }
    }

}
