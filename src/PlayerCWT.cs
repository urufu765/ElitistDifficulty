using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ElitistDifficulty;

public static class PlayerCWT
{
    public class AnSlugcat
    {
        // Define your variables to store here!
        public int readyForShock;
        public float lacticAcid;

        public AnSlugcat(){
            // Initialize your variables here! (Anything not added here will be null or false or 0 (default values))
            readyForShock = 0;
            lacticAcid = 0;
        }
    }

    // This part lets you access the stored stuff by simply doing "self.GetCat()" in Plugin.cs or everywhere else!
    private static readonly ConditionalWeakTable<Player, AnSlugcat> CWT = new();
    public static AnSlugcat GetCat(this Player player) => CWT.GetValue(player, _ => new());
}
