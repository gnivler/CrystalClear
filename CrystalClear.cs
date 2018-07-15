using Harmony;
using System.Reflection;
using UnityEngine.PostProcessing;

namespace CrystalClear
{
    public class CrystalClear
    {
        public static void Init(string directory, string settingsJSON)
        {
            var harmony = HarmonyInstance.Create("ca.gnivler.ModName");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        [HarmonyPatch(typeof(DitheringComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PathDitheringComp
        {
            public static bool Prefix()
            {
                FileLog.Log($"Dither\n");
                return false;
            }
        }

        [HarmonyPatch(typeof(GrainComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchGrainComp
        {
            public static bool Prefix()
            {
                FileLog.Log("Grain\n");
                return false;
            }
        }
    }
}