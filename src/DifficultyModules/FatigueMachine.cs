using System;
using UnityEngine;
using RWCustom;
using static EliteHelper.Helper;
using ElitistDifficulty;

namespace ElitistModules;

public static class FatigueMachine
{
    public static void SendPlayerDown(this Player self)
    {
        if (self.GetCat().lacticAcid >= 1 && self.aerobicLevel > 0.94f)
        {
            self.exhausted = true;
            self.GetCat().lacticAcid -= 0.15f;
        }
    }
}
