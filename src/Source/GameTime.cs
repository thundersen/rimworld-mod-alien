using System;

namespace Alien
{
    public class GameTime
    {
        public const int OneHourTicks = 2500;
        public const int NightEnd = 5;
        public const int NightStart = 0;

        public static int RandomTickNextNight(int currentTick, int currentHour)
        {
            
            
            if (NightStart <= currentHour && currentHour <= NightEnd)
            {
                return currentTick + new Random().Next(0, OneHourTicks);
            }

            var ticksUntilNight = (24 - currentHour) * OneHourTicks;
            
            return currentTick + ticksUntilNight + new Random().Next(0, 5 * OneHourTicks);
        }
    }
}