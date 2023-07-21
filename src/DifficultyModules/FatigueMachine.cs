using System;
using UnityEngine;
using RWCustom;
using static EliteHelper.Helper;
using ElitistDifficulty;

namespace ElitistModules;

public static class FatigueMachine
{
    public static void SendPlayerDown(this Player self, float acid)
    {
        if (!self.dead)
        {
            self.GetCat().lacticAcid += acid / 10;
        }
        if (self.GetCat().lacticAcid >= 1 && self.aerobicLevel > 2 - self.GetCat().lacticAcid)
        {
            self.exhausted = true;
            self.Stun(80);
            self.GetCat().lacticAcid = 0.85f;
        }
    }

    public static void VisualizeFatigue(this PlayerGraphics self)
    {
        try
        {
            if (!self.player.dead && !self.player.Sleeping)
            {
                self.breath += 1f / Mathf.Lerp(100f, 15f, Mathf.Pow(self.player.GetCat().lacticAcid, 1.5f));
            }

        }
        catch (Exception e)
        {
            if (self?.player is not null) self.player.L(e, "Couldn't visualize lactic acid buildup");
            else L(e, "Couldn't visualize lactic acid buildup");
        }
    }

    public static void SendPlayerDownForTheCount(this Player self)
    {
        if (self.dead) return;
        if (self.exhausted && self.GetCat().lacticAcid > 0.8f)
        {
            self.LoseAllGrasps();
        }
    }
}
