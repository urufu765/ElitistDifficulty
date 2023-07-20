using System;
using static EliteHelper.Helper;

namespace ElitistModules;

public static class ApplyViolence
{
    public static void CheckForShockDeath(this Player self, Creature.DamageType type, float stunBonus)
    {
        if (type == Creature.DamageType.Electric)
        {
            self.DeathByShock((int)stunBonus);
        }
    }

    public static void DeathByShock(this Player self, int stunDuration)
    {
        self.L("Death by electricity!", 1);
        self.room?.AddObject(new CreatureSpasmer(self, true, stunDuration));
        self.Die();
    }

    public static void DeathIfSavingThrowFail(this Player self)
    {
        if (self.dangerGraspTime > 30)
        {
            self.L("Death by not responding fast enough.", 1);
            self.Die();
        }
    }
}
