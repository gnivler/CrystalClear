using Harmony;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using BattleTech.Rendering;
using UnityEngine;
using UnityEngine.PostProcessing;
using static CrystalClear.Logger;

namespace CrystalClear
{
    public class Settings
    {
        public bool Dithering;
        public bool Grain;
        public bool Vignette;
        public bool Bloom;
        public bool UIBloom;
        public bool Shadows;
        public bool ChromaticAberration;
        public bool EyeAdaptation;
        public bool Fog;
        public bool ColorGrading;
        public bool AmbientOcclusion;
        public bool Taa;
        public bool DepthOfField;
        public bool Fxaa;
        public bool HDR;
        public bool Grunge;
        public bool Scanlines;
        public bool enableDebug;
    }

    public static class CrystalClear
    {
        public static Settings modSettings;
        public static string modDirectory;

        public static void Init(string modDirectory, string settingsJson)
        {
            FileLog.logPath = Path.Combine(modDirectory, "log.txt");
            Clear();
            var harmony = HarmonyInstance.Create("ca.gnivler.CrystalClear");
            //HarmonyInstance.DEBUG = false;
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            try
            {
                CrystalClear.modDirectory = modDirectory;
                modSettings = JsonConvert.DeserializeObject<Settings>(settingsJson);
            }
            catch (Exception e)
            {
                Error(e);
            }

            if (modSettings.Grunge || modSettings.Scanlines) return;
            int mainTex = Shader.PropertyToID("_MainTex");
            Type uniformsType = AccessTools.Inner(typeof(BTPostProcess), "Uniforms");
            if (!modSettings.Grunge)
            {
                AccessTools.Field(uniformsType, "_GrungeTex").SetValue(null, mainTex);
            }

            if (!modSettings.Scanlines)
            {
                AccessTools.Field(uniformsType, "_ScanlineTex").SetValue(null, mainTex);
            }

            if (!modSettings.Dithering)
            {
                AccessTools.Field(uniformsType, "_DitheringTex").SetValue(null, 0);
            }
        }

        [HarmonyPatch(typeof(BTPostProcess), "OnEnable", MethodType.Normal)]
        public static class BTPostProcess_Ctor_Patch
        {
            public static void Postfix(ref float ___uiBloomIntensity)
            {
                if (!modSettings.UIBloom)
                {
                    ___uiBloomIntensity = 0f;
                    LogDebug("Patching UI bloom");
                }
            }
        }

        [HarmonyPatch(typeof(MenuCamera), "OnEnable", MethodType.Normal)]
        public static class MenuCamera_Ctor_Patch
        {
            public static void Postfix(ref float ___uiBloomIntensity)
            {
                if (!modSettings.UIBloom)
                {
                    LogDebug("Patching menu bloom");
                    ___uiBloomIntensity = 0f;
                }
            }
        }

        [HarmonyPatch(typeof(DitheringComponent), "active", MethodType.Getter)]
        public static class PatchDitheringComp
        {
            public static bool Prefix(ref bool __result)
            {
                __result = modSettings.Dithering;
                return false;
            }
        }

        [HarmonyPatch(typeof(GrainComponent), "active", MethodType.Getter)]
        public static class PatchGrainComp
        {
            public static bool Prefix(ref bool __result)
            {
                __result = modSettings.Grain;
                return false;
            }
        }

        [HarmonyPatch(typeof(VignetteComponent), "active", MethodType.Getter)]
        public static class PatchVignette
        {
            public static bool Prefix(ref bool __result)
            {
                __result = modSettings.Vignette;
                return false;
            }
        }

        [HarmonyPatch(typeof(BloomComponent), "active", MethodType.Getter)]
        public static class PatchBloom
        {
            public static bool Prefix(ref bool __result)
            {
                __result = modSettings.Bloom;
                return false;
            }
        }

        [HarmonyPatch(typeof(ScreenSpaceShadowsComponent), "active", MethodType.Getter)]
        public static class PatchShadows
        {
            public static bool Prefix(ref bool __result)
            {
                __result = modSettings.Shadows;
                return false;
            }
        }

        [HarmonyPatch(typeof(ChromaticAberrationComponent), "active", MethodType.Getter)]
        public static class ChromaticAberration
        {
            public static bool Prefix(ref bool __result)
            {
                __result = modSettings.ChromaticAberration;
                return false;
            }
        }

        [HarmonyPatch(typeof(EyeAdaptationComponent), "active", MethodType.Getter)]
        public static class EyeAdaptation
        {
            public static bool Prefix(ref bool __result)
            {
                __result = modSettings.EyeAdaptation;
                return false;
            }
        }

        [HarmonyPatch(typeof(FogComponent), "active", MethodType.Getter)]
        public static class Fog
        {
            public static bool Prefix(ref bool __result)
            {
                __result = modSettings.Fog;
                return false;
            }
        }

        [HarmonyPatch(typeof(ColorGradingComponent), "active", MethodType.Getter)]
        public static class ColorGrading
        {
            public static bool Prefix(ref bool __result)
            {
                __result = modSettings.ColorGrading;
                return false;
            }
        }

        [HarmonyPatch(typeof(AmbientOcclusionComponent), "active", MethodType.Getter)]
        public static class AmbientOcclusion
        {
            public static bool Prefix(ref bool __result)
            {
                __result = modSettings.AmbientOcclusion;
                return false;
            }
        }

        [HarmonyPatch(typeof(TaaComponent), "active", MethodType.Getter)]
        public static class Taa
        {
            public static bool Prefix(ref bool __result)
            {
                __result = modSettings.Taa;
                return false;
            }
        }

        [HarmonyPatch(typeof(DepthOfFieldComponent), "active", MethodType.Getter)]
        public static class DepthOfField
        {
            public static bool Prefix(ref bool __result)
            {
                __result = modSettings.DepthOfField;
                return false;
            }
        }

        [HarmonyPatch(typeof(FxaaComponent), "active", MethodType.Getter)]
        public static class Fxaa
        {
            public static bool Prefix(ref bool __result)
            {
                __result = modSettings.Fxaa;
                return false;
            }
        }

        [HarmonyPatch(typeof(PostProcessingContext), "isHdr", MethodType.Getter)]
        public static class HdrPatch
        {
            public static bool Prefix(ref bool __result)
            {
                __result = modSettings.HDR;
                return false;
            }
        }
    }
}