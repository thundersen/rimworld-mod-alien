using System.Reflection;
using Harmony;
using Verse;

namespace Alien
{
    [StaticConstructorOnStartup]
    class Bootstrap
    {
        static Bootstrap()
        {
            var harmony = HarmonyInstance.Create("com.thundersen.rimworld.mod.alien");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}