using System;
using static EliteHelper.Helper;
using static ElitistDifficulty.Plugin;
using ElitistModules;


namespace ElitistModules;

public static class FallToDeath
{
    public static void CheckDeathCondition(this Player self, float speed, bool firstContact)
    {
        if (Clock.FallImmunity > 0) return;
        float limit = 35f;
        bool extraConditions = true;
        if (Patch_MSC) 
        {
            if (self.isGourmand) limit = 40f;
            extraConditions = self.tongue is null || !self.tongue.Attached;
        }
        if (self.Stunned && firstContact && speed > limit && extraConditions)
        {
            L("Death by fall!");
            self.Die();
        }
    }
}
