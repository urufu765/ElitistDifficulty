﻿using System;
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
            self.GetCat().lacticAcid += acid / (self.lungsExhausted? 50 : 100);
        }
        if (!self.lungsExhausted && self.GetCat().lacticAcid >= 1 && self.aerobicLevel > 2 - self.GetCat().lacticAcid)
        {
            self.lungsExhausted = true;
            self.GetCat().lacticAcid += self.aerobicLevel;
        }
        else if (self.lungsExhausted && UnityEngine.Random.value < self.GetCat().lacticAcid / (100 / acid))
        {
            self.Stun((int)(100 * self.GetCat().lacticAcid));
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
        if (self.Stunned && self.GetCat().lacticAcid > 0.8f)
        {
            self.LoseAllGrasps();
        }
    }


    public static void ManageFatigue(this Player self)
    {
        if (self.dead) return;
        if (self.GetCat().lacticAcid < 1 && !self.lungsExhausted)
        {
            self.GetCat().lacticAcid += self.Malnourished? Mathf.Lerp(0, 0.005f, Mathf.InverseLerp(0.85f, 1f, self.aerobicLevel)) : Mathf.Lerp(0, 0.001f, Mathf.InverseLerp(0.85f, 1f, self.aerobicLevel));
            self.GetCat().lacticAcid = Mathf.Max(0, self.GetCat().lacticAcid + Mathf.Lerp(-0.0005f, 0, Mathf.InverseLerp(0, 0.75f, self.aerobicLevel)));
        }
        else
        {
            self.GetCat().lacticAcid -= self.Stunned? 0.00125f : 0.00075f;
        }
        if (self.lungsExhausted)
        {
            self.airInLungs = Mathf.Min(self.airInLungs, Mathf.Max(0.2f, 1.25f - self.GetCat().lacticAcid));
            if (UnityEngine.Random.value < self.GetCat().lacticAcid / 1000)
            {
                self.Stun((int)(100 * self.GetCat().lacticAcid));
            }
            self.exhausted = true;
        }
        // if (self.lungsExhausted && self.aerobicLevel < 0.4f)
        // {
        //     self.lungsExhausted = false;
        // }
    }

}
