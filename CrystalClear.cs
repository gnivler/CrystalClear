using Harmony;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using TMPro;
using UnityEngine.PostProcessing;


namespace CrystalClear
{
    internal class ModSettings
    {
        public bool Debug;
        public bool Dithering;
        public bool Grain;
        public bool Vignette;
        public bool Bloom;
        public bool Shadows;
        public bool ChromaticAberration;
        public bool EyeAdaptation;
        public bool Fog;
        public bool ColorGrading;   
    }

    public static class CrystalClear
    {
        internal static ModSettings _settings;
        internal static string _modDirectory;
        public static void Init(string modDirectory, string settingsJson)
        {
            //FileLog.Reset();
            //FileLog.Log($"{DateTime.Now.ToLongTimeString()} Init");
            var harmony = HarmonyInstance.Create("ca.gnivler.CrystalClear");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            try
            {
                _modDirectory = modDirectory;
                _settings = JsonConvert.DeserializeObject<ModSettings>(settingsJson);

            }
            catch (Exception e)
            {
                //FileLog.Log("Exception");
                //FileLog.Log(e.ToString());
            }

            if (_settings.Debug)
            {
                //FileLog.Log("Debug enabled");
            }
        }

        [HarmonyPatch(typeof(DitheringComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PacthDitheringComp
        {
            public static bool Prefix(ref bool __result)
            {
                __result = _settings.Dithering;
                return false;
            }
        }

        [HarmonyPatch(typeof(GrainComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchGrainComp
        {
            public static bool Prefix(ref bool __result)
            {
                __result = _settings.Grain;
                return false;
            }
        }

        [HarmonyPatch(typeof(VignetteComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchVignette
        {
            public static bool Prefix(ref bool __result)
            {
                __result = _settings.Vignette;
                return false;
            }
        }

        [HarmonyPatch(typeof(BloomComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchBloom
        {
            public static bool Prefix(ref bool __result)
            {
                __result = _settings.Bloom;
                return false;
            }
        }

        [HarmonyPatch(typeof(ScreenSpaceShadowsComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchShadows
        {
            public static bool Prefix(ref bool __result)
            {
                __result = _settings.Shadows;
                return false;
            }
        }

        [HarmonyPatch(typeof(ChromaticAberrationComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class ChromaticAberration
        {
            public static bool Prefix(ref bool __result)
            {
                __result = _settings.ChromaticAberration;
                return false;
            }
        }

        [HarmonyPatch(typeof(EyeAdaptationComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class EyeAdaptation
        {
            public static bool Prefix(ref bool __result)
            {
                __result = _settings.EyeAdaptation;
                return false;
            }
        }

        [HarmonyPatch(typeof(FogComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class Fog
        {
            public static bool Prefix(ref bool __result)
            {
                __result = _settings.Fog;
                return false;
            }
        }

        [HarmonyPatch(typeof(ColorGradingComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class ColorGrading
        {
            public static bool Prefix(ref bool __result)
            {
                __result = _settings.ColorGrading;
                return false;
            }
        }

    }
}