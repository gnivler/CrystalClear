using Harmony;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using BattleTech;
using BattleTech.Rendering;
using UnityEngine;
using UnityEngine.PostProcessing;
using static CrystalClear.Logger;

namespace CrystalClear
{
    public class ModSettings
    {
        public bool Debug;
        public bool Dithering;
        public bool Grain;
        public bool Vignette;
        public bool Bloom;
        public bool Shadows;
        public bool ChromaticAberration;
        public bool EyeAdaptation;          // not recommended true
        public bool Fog;                    // not recommended true
        public bool ColorGrading;
        public bool AmbientOcclusion;
        public bool Taa;
        public bool DepthOfField;
        public bool UserLut;
        public bool Fxaa;
        public bool HDR;
    }

    public static class CrystalClear
    {
        internal static ModSettings Settings;
        internal static string ModDirectory;

        public static void Init(string modDirectory, string settingsJson)
        {
            var harmony = HarmonyInstance.Create("com.gnivler.CrystalClear");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            try
            {
                ModDirectory = modDirectory;
                Settings = JsonConvert.DeserializeObject<ModSettings>(settingsJson);
            }
            catch (Exception e)
            {
                Error(e);
            }

            Clear();
        }

        [HarmonyPatch(typeof(DitheringComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchDitheringComp
        {
            public static bool Prefix(ref bool __result)
            {
                __result = Settings.Dithering;
                return false;
            }
        }

        [HarmonyPatch(typeof(GrainComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchGrainComp
        {
            public static bool Prefix(ref bool __result)
            {
                __result = Settings.Grain;
                return false;
            }
        }

        [HarmonyPatch(typeof(VignetteComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchVignette
        {
            public static bool Prefix(ref bool __result)
            {
                __result = Settings.Vignette;
                return false;
            }
        }

        [HarmonyPatch(typeof(BloomComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchBloom
        {
            public static bool Prefix(ref bool __result)
            {
                __result = Settings.Bloom;
                return false;
            }
        }

        [HarmonyPatch(typeof(ScreenSpaceShadowsComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchShadows
        {
            public static bool Prefix(ref bool __result)
            {
                __result = Settings.Shadows;
                return false;
            }
        }

        [HarmonyPatch(typeof(ChromaticAberrationComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class ChromaticAberration
        {
            public static bool Prefix(ref bool __result)
            {
                __result = Settings.ChromaticAberration;
                return false;
            }
        }

        [HarmonyPatch(typeof(EyeAdaptationComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class EyeAdaptation
        {
            public static bool Prefix(ref bool __result)
            {
                __result = Settings.EyeAdaptation;
                return false;
            }
        }

        [HarmonyPatch(typeof(FogComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class Fog
        {
            public static bool Prefix(ref bool __result)
            {
                __result = Settings.Fog;
                return false;
            }
        }

        [HarmonyPatch(typeof(ColorGradingComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class ColorGrading
        {
            public static bool Prefix(ref bool __result)
            {
                __result = Settings.ColorGrading;
                return false;
            }
        }

        [HarmonyPatch(typeof(AmbientOcclusionComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class AmbientOcclusion
        {
            public static bool Prefix(ref bool __result)
            {
                __result = Settings.AmbientOcclusion;
                return false;
            }
        }

        [HarmonyPatch(typeof(TaaComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class Taa
        {
            public static bool Prefix(ref bool __result)
            {
                __result = Settings.Taa;
                return false;
            }
        }

        [HarmonyPatch(typeof(DepthOfFieldComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class DepthOfField
        {
            public static bool Prefix(ref bool __result)
            {
                __result = Settings.DepthOfField;
                return false;
            }
        }

        [HarmonyPatch(typeof(UserLutComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class UserLut
        {
            public static bool Prefix(ref bool __result)
            {
                __result = Settings.UserLut;
                return false;
            }
        }

        [HarmonyPatch(typeof(FxaaComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class Fxaa
        {
            public static bool Prefix(ref bool __result)
            {
                __result = Settings.Fxaa;
                return false;
            }
        }

        [HarmonyPatch(typeof(PostProcessingContext))]
        [HarmonyPatch("isHdr", PropertyMethod.Getter)]
        public static class HdrPatch
        {
            public static bool Prefix(ref bool __result)
            {
                __result = Settings.HDR;
                return false;
            }
        }
    }
}