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
        public string Dithering;
        public string Grain;
        public string Vignette;
        public string Bloom;
        public string UIBloom;
        public string Shadows;
        public string ChromaticAberration;
        public string EyeAdaptation;
        public string Fog;
        public string ColorGrading;
        public string AmbientOcclusion;
        public string Taa;
        public string DepthOfField;
        public string Fxaa;
        public string HDR;
        public string Grunge;
        public string Scanlines;
    }

    public static class CrystalClear
    {
        public static Settings modSettings;
        public static string modDirectory;
        public const string ON = "On";
        public const string OFF = "Off";
        public const string IGNORE = "Ignore";

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

            if (modSettings.Grunge == ON || modSettings.Scanlines == ON) return;
            if (modSettings.Grunge == IGNORE || modSettings.Scanlines == IGNORE) return;

            int mainTex = Shader.PropertyToID("_MainTex");
            Type uniformsType = AccessTools.Inner(typeof(BTPostProcess), "Uniforms");
            if (modSettings.Grunge == OFF)
            {
                AccessTools.Field(uniformsType, "_GrungeTex").SetValue(null, mainTex);
            }

            if (modSettings.Scanlines == OFF)
            {
                AccessTools.Field(uniformsType, "_ScanlineTex").SetValue(null, mainTex);
            }

            if (modSettings.Dithering == OFF)
            {
                AccessTools.Field(uniformsType, "_DitheringTex").SetValue(null, 0);
            }
        }

        [HarmonyPatch(typeof(BTPostProcess), "OnEnable", MethodType.Normal)]
        public static class BTPostProcess_Ctor_Patch
        {
            public static bool Postfix(ref float ___uiBloomIntensity)
            {
                if (modSettings.UIBloom == IGNORE) return true;
                if (modSettings.UIBloom == ON) return true;
                ___uiBloomIntensity = 0f;
                LogDebug("Patching UI bloom");
                return false;
            }
        }

        [HarmonyPatch(typeof(MenuCamera), "OnEnable", MethodType.Normal)]
        public static class MenuCamera_Ctor_Patch
        {
            public static bool Postfix(ref float ___uiBloomIntensity)
            {
                if (modSettings.UIBloom == IGNORE) return true;
                if (modSettings.UIBloom == ON) return true;
                LogDebug("Patching menu bloom");
                uiBloomIntensity = 0f;
                return false;

            }
        }

        [HarmonyPatch(typeof(DitheringComponent), "active", MethodType.Getter)]
        public static class PatchDitheringComp
        {
            public static bool Prefix(ref bool __result)
            {
                if (modSettings.Dithering == IGNORE) return true;
                __result = modSettings.Dithering == ON;
                return false;
            }
        }

        [HarmonyPatch(typeof(GrainComponent), "active", MethodType.Getter)]
        public static class PatchGrainComp
        {
            public static bool Prefix(ref bool __result)
            {
                if (modSettings.Grain == IGNORE) return true;
                if (modSettings.Grain == ON) return true;
                __result = false;
                return false;
            }
        }

        [HarmonyPatch(typeof(VignetteComponent), "active", MethodType.Getter)]
        public static class PatchVignette
        {
            public static bool Prefix(ref bool __result)
            {
                if (modSettings.Vignette == IGNORE) return true;
                __result = modSettings.Vignette == ON;
                return false;
            }
        }

        [HarmonyPatch(typeof(BloomComponent), "active", MethodType.Getter)]
        public static class PatchBloom
        {
            public static bool Prefix(ref bool __result)
            {
                if (modSettings.Bloom == IGNORE) return true;
                __result = modSettings.Bloom == ON;
                return false;
            }
        }

        [HarmonyPatch(typeof(ScreenSpaceShadowsComponent), "active", MethodType.Getter)]
        public static class PatchShadows
        {
            public static bool Prefix(ref bool __result)
            {
                if (modSettings.Shadows == IGNORE) return true;
                __result = modSettings.Shadows == ON;
                return false;
            }
        }

        [HarmonyPatch(typeof(ChromaticAberrationComponent), "active", MethodType.Getter)]
        public static class ChromaticAberration
        {
            public static bool Prefix(ref bool __result)
            {
                if (modSettings.ChromaticAberration == IGNORE) return true;
                __result = modSettings.ChromaticAberration == ON;
                return false;
            }
        }

        [HarmonyPatch(typeof(EyeAdaptationComponent), "active", MethodType.Getter)]
        public static class EyeAdaptation
        {
            public static bool Prefix(ref bool __result)
            {
                if (modSettings.EyeAdaptation == IGNORE) return true;
                __result = modSettings.EyeAdaptation == ON;
                return false;
            }
        }

        [HarmonyPatch(typeof(FogComponent), "active", MethodType.Getter)]
        public static class Fog
        {
            public static bool Prefix(ref bool __result)
            {
                if (modSettings.Fog == IGNORE) return true;
                __result = modSettings.Fog == ON;
                return false;
            }
        }

        [HarmonyPatch(typeof(ColorGradingComponent), "active", MethodType.Getter)]
        public static class ColorGrading
        {
            public static bool Prefix(ref bool __result)
            {
                if (modSettings.ColorGrading == IGNORE) return true;
                __result = modSettings.ColorGrading == ON;
                return false;
            }
        }

        [HarmonyPatch(typeof(AmbientOcclusionComponent), "active", MethodType.Getter)]
        public static class AmbientOcclusion
        {
            public static bool Prefix(ref bool __result)
            {
                if (modSettings.AmbientOcclusion == IGNORE) return true;
                __result = modSettings.AmbientOcclusion == ON;
                return false;
            }
        }

        [HarmonyPatch(typeof(TaaComponent), "active", MethodType.Getter)]
        public static class Taa
        {
            public static bool Prefix(ref bool __result)
            {
                if (modSettings.Taa == IGNORE) return true;
                __result = modSettings.Taa == ON;
                return false;
            }
        }

        [HarmonyPatch(typeof(DepthOfFieldComponent), "active", MethodType.Getter)]
        public static class DepthOfField
        {
            public static bool Prefix(ref bool __result)
            {
                if (modSettings.DepthOfField == IGNORE) return true;
                __result = modSettings.DepthOfField == ON;
                return false;
            }
        }

        [HarmonyPatch(typeof(FxaaComponent), "active", MethodType.Getter)]
        public static class Fxaa
        {
            public static bool Prefix(ref bool __result)
            {
                if (modSettings.Fxaa == IGNORE) return true;
                __result = modSettings.Fxaa == ON;
                return false;
            }
        }

        [HarmonyPatch(typeof(PostProcessingContext), "isHdr", MethodType.Getter)]
        public static class HdrPatch
        {
            public static bool Prefix(ref bool __result)
            {
                if (modSettings.HDR == IGNORE) return true;
                __result = modSettings.HDR == ON;
                return false;
            }
        }
    }
}