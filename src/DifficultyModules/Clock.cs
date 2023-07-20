using System;
using static EliteHelper.Helper;
using ElitistDifficulty;

namespace ElitistModules;

public static class Clock
{
    public static int FallImmunity {get; private set;} = 0;
    public static string LastRegion {get; private set;} = "";

    public static void Tick(this Player player)
    {
        if (FallImmunity > 0)
        {
            FallImmunity--;
        }

        try
        {
            if (player?.room?.world?.region is not null)
            {
                if (player.room.world.region.name != LastRegion)
                {
                    LastRegion = player.room.world.region.name;
                    FallImmunity = 400;
                }
            }
        }
        catch (Exception ex)
        {
            L(ex, "Couldn't check if player changed regions");
        }
    }
}
