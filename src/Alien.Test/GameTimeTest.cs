using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Alien.Test
{
    [TestFixture]
    public class GameTimeTest
    {
        [TestCaseSource(nameof(NightHours))]
        public void ReturnTickInNextHourIfItsNight(int currentHour)
        {
            var tick = GameTime.RandomTickNextNight(0, currentHour);

            Assert.GreaterOrEqual(tick, 0);
            Assert.LessOrEqual(tick, GameTime.OneHourTicks);
        }

        [TestCaseSource(nameof(DayHours))]
        public void ReturnTickInNextNightIfItsDay(int currentHour)
        {
            var beginningOfNextNight = (23 - currentHour) * GameTime.OneHourTicks;
            var endOfNextNight = beginningOfNextNight + 6 * GameTime.OneHourTicks;

            var tick = GameTime.RandomTickNextNight(0, currentHour);

            Assert.GreaterOrEqual(tick, beginningOfNextNight);
            Assert.LessOrEqual(tick, endOfNextNight);
        }

        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public static List<TestCaseData> DayHours { get; } = Enumerable.Range(6, 23).Select(h => new TestCaseData(h)).ToList();
        public static List<TestCaseData> NightHours { get; } = Enumerable.Range(0, 5).Select(h => new TestCaseData(h)).ToList();
    }
}