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

    public static void ManageFatigue(this Player self)
    {
        if (self.dead) return;
        self.GetCat().lacticAcid += self.Malnourished? Mathf.Lerp(0, 0.05f, Mathf.InverseLerp(0.85f, 1f, self.aerobicLevel)) : Mathf.Lerp(0, 0.01f, Mathf.InverseLerp(0.85f, 1f, self.aerobicLevel));
        self.GetCat().lacticAcid = Mathf.Max(0, self.GetCat().lacticAcid + Mathf.Lerp(-0.00001f, 0, Mathf.InverseLerp(0, 0.6f, self.aerobicLevel)));
        if (self.exhausted && UnityEngine.Random.value < self.GetCat().lacticAcid / 50)
        {
            self.Stun(40);
        }
    }
}
