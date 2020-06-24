using Harmony;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BattleTech.Rendering;
using BattleTech.Rendering.Mood;
using BattleTech.UI;
using HBS.Extensions;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

// ReSharper disable InconsistentNaming

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
        public string MotionBlur;
        public string TurnBanner;
        public string DustStorms;
        public string Pollen;
        public string Rain;
        public string Mist;
    }

    public static class CrystalClear
    {
        private static Settings modSettings;

        public static void Init(string settingsJson)
        {
            Log("Startup");
            var harmony = HarmonyInstance.Create("ca.gnivler.BattleTech.CrystalClear");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            try
            {
                modSettings = JsonConvert.DeserializeObject<Settings>(settingsJson);
            }
            catch (Exception ex)
            {
                Log(ex);
            }


            // UI grunge
            var mainTex = Shader.PropertyToID("_MainTex");
            var uniformsType = AccessTools.Inner(typeof(BTPostProcess), "Uniforms");

            if (modSettings.Grunge == "OFF")
            {
                Log("Patching UI grunge");
                AccessTools.Field(uniformsType, "_GrungeTex").SetValue(null, mainTex);
            }

            // UI scanlines
            if (modSettings.Scanlines == "OFF")
            {
                Log("Patching UI scanlines");
                AccessTools.Field(uniformsType, "_ScanlineTex").SetValue(null, mainTex);
            }

            // Grainy
            if (modSettings.Dithering == "OFF")
            {
                Log("Patching dithering");
                AccessTools.Field(uniformsType, "_DitheringTex").SetValue(null, 0);
            }
        }

        private static void Log(object input)
        {
            //FileLog.Log($"[CC] {input ?? "null"}");
        }

        // hide turn banner overlay
        [HarmonyPatch(typeof(TurnEventNotification), "ShowTeamNotification")]
        public class TurnEventNotification_ShowTeamNotification_Patch
        {
            public static void Postfix(ref List<Graphic> ___graphicList)
            {
                if (modSettings.TurnBanner == "OFF")
                {
                    ___graphicList.Find(x => x.name == "tt_barFill").gameObject.SetActive(false);
                }
            }
        }

        // hide dust storms
        [HarmonyPatch(typeof(WeatherController), "SetWeather")]
        public static class WeatherController_SetWeather_Patch
        {
            public static void Postfix(WeatherController __instance)
            {
                try
                {
                    if (__instance.currentWeatherVFX == null)
                    {
                        return;
                    }

                    var transforms = __instance.currentWeatherVFX
                        .GetComponentsInChildren<Transform>().Where(x => x != null)
                        .Where(x => !string.IsNullOrEmpty(x.name));
                    //transforms.Do(Log);

                    if (modSettings.Rain == "OFF")
                    {
                        Log("Patching rain");
                        DisableComponent("rain");
                    }

                    if (modSettings.Pollen == "OFF")
                    {
                        Log("Patching pollen");
                        DisableComponent("pollen");
                    }

                    if (modSettings.Mist == "OFF")
                    {
                        Log("Patching mist");
                        DisableComponent("mist");
                    }

                    if (modSettings.DustStorms == "OFF")
                    {
                        Log("Patching dust");
                        DisableComponent("dust");
                    }

                    void DisableComponent(string name)
                    {
                        name = name.ToLower();
                        foreach (var transform in transforms)
                        {
                            transform.gameObject.SetActive(false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log(ex);
                }
            }
        }

        // UI bloom
        [HarmonyPatch(typeof(BTPostProcess), "OnEnable", MethodType.Normal)]
        public static class BTPostProcess_OnEnable_Patch
        {
            public static void Postfix(ref float ___uiBloomIntensity)
            {
                if (modSettings.UIBloom == "OFF")
                {
                    Log("Patching UI bloom");
                    ___uiBloomIntensity = 0f;
                }
            }
        }

        // UI bloom
        [HarmonyPatch(typeof(MenuCamera), "OnEnable", MethodType.Normal)]
        public static class MenuCamera_OnEnable_Patch
        {
            public static void Postfix(ref float ___uiBloomIntensity)
            {
                if (modSettings.UIBloom == "OFF")
                {
                    Log("Patching menu bloom");
                    ___uiBloomIntensity = 0f;
                }
            }
        }

        // Dithering
        [HarmonyPatch(typeof(DitheringComponent), "active", MethodType.Getter)]
        public static class DitheringComponent_active_Patch
        {
            public static bool Prefix(ref bool __result)
            {
                // implicitly does nothing if 'NOTSET'
                switch (modSettings.Dithering)
                {
                    case "ON":
                    {
                        __result = true;
                        break;
                    }

                    case "OFF":
                    {
                        __result = false;
                        break;
                    }

                    default:
                        return true;
                }

                return false;
            }
        }

        // Motion blur
        [HarmonyPatch(typeof(MotionBlurComponent), "active", MethodType.Getter)]
        public static class MotionBlurComponent_active_Patch
        {
            public static bool Prefix(ref bool __result)
            {
                switch (modSettings.MotionBlur)
                {
                    case "ON":
                    {
                        __result = true;
                        break;
                    }

                    case "OFF":
                    {
                        __result = false;
                        break;
                    }

                    default:
                        return true;
                }

                return false;
            }
        }

        // Grain
        [HarmonyPatch(typeof(GrainComponent), "active", MethodType.Getter)]
        public static class GrainComponent_active_Patch
        {
            public static bool Prefix(ref bool __result)
            {
                switch (modSettings.Grain)
                {
                    case "ON":
                    {
                        __result = true;
                        break;
                    }

                    case "OFF":
                    {
                        __result = false;
                        break;
                    }

                    default:
                        return true;
                }

                return false;
            }
        }

        // Vignette
        [HarmonyPatch(typeof(VignetteComponent), "active", MethodType.Getter)]
        public static class VignetteComponent_active_Patch
        {
            public static bool Prefix(ref bool __result)
            {
                switch (modSettings.Vignette)
                {
                    case "ON":
                    {
                        __result = true;
                        break;
                    }

                    case "OFF":
                    {
                        __result = false;
                        break;
                    }

                    default:
                        return true;
                }

                return false;
            }
        }

        // Bloom
        [HarmonyPatch(typeof(BloomComponent), "active", MethodType.Getter)]
        public static class BloomComponent_active_Patch
        {
            public static bool Prefix(ref bool __result)
            {
                switch (modSettings.Bloom)
                {
                    case "ON":
                    {
                        __result = true;
                        break;
                    }

                    case "OFF":
                    {
                        __result = false;
                        break;
                    }

                    default:
                        return true;
                }

                return false;
            }
        }

        // Shadows
        [HarmonyPatch(typeof(ScreenSpaceShadowsComponent), "active", MethodType.Getter)]
        public static class ScreenSpaceShadowsComponent_active_Patch
        {
            public static bool Prefix(ref bool __result)
            {
                switch (modSettings.Shadows)
                {
                    case "ON":
                    {
                        __result = true;
                        break;
                    }

                    case "OFF":
                    {
                        __result = false;
                        break;
                    }

                    default:
                        return true;
                }

                return false;
            }
        }

        // Chromatic abberation
        [HarmonyPatch(typeof(ChromaticAberrationComponent), "active", MethodType.Getter)]
        public static class ChromaticAberrationComponent_active_Patch
        {
            public static bool Prefix(ref bool __result)
            {
                switch (modSettings.ChromaticAberration)
                {
                    case "ON":
                    {
                        __result = true;
                        break;
                    }

                    case "OFF":
                    {
                        __result = false;
                        break;
                    }

                    default:
                        return true;
                }

                return false;
            }
        }

        // Eye adaptation
        [HarmonyPatch(typeof(EyeAdaptationComponent), "active", MethodType.Getter)]
        public static class EyeAdaptationComponent_active_Patch
        {
            public static bool Prefix(ref bool __result)
            {
                switch (modSettings.EyeAdaptation)
                {
                    case "ON":
                    {
                        __result = true;
                        break;
                    }

                    case "OFF":
                    {
                        __result = false;
                        break;
                    }

                    default:
                        return true;
                }

                return false;
            }
        }

        // Fog
        [HarmonyPatch(typeof(FogComponent), "active", MethodType.Getter)]
        public static class FogComponent_active_Patch
        {
            public static bool Prefix(ref bool __result)
            {
                switch (modSettings.Fog)
                {
                    case "ON":
                    {
                        __result = true;
                        break;
                    }

                    case "OFF":
                    {
                        __result = false;
                        break;
                    }

                    default:
                        return true;
                }

                return false;
            }
        }

        // Color grading
        [HarmonyPatch(typeof(ColorGradingComponent), "active", MethodType.Getter)]
        public static class ColorGradingComponent_active_Patch
        {
            public static bool Prefix(ref bool __result)
            {
                switch (modSettings.ColorGrading)
                {
                    case "ON":
                    {
                        __result = true;
                        break;
                    }

                    case "OFF":
                    {
                        __result = false;
                        break;
                    }

                    default:
                        return true;
                }

                return false;
            }
        }

        // Ambient occlusion
        [HarmonyPatch(typeof(AmbientOcclusionComponent), "active", MethodType.Getter)]
        public static class AmbientOcclusionComponent_active_Patch
        {
            public static bool Prefix(ref bool __result)
            {
                switch (modSettings.AmbientOcclusion)
                {
                    case "ON":
                    {
                        __result = true;
                        break;
                    }

                    case "OFF":
                    {
                        __result = false;
                        break;
                    }

                    default:
                        return true;
                }

                return false;
            }
        }

        // Temporal anti-aliasing
        [HarmonyPatch(typeof(TaaComponent), "active", MethodType.Getter)]
        public static class TaaComponent_active_Patch
        {
            public static bool Prefix(ref bool __result)
            {
                switch (modSettings.Taa)
                {
                    case "ON":
                    {
                        __result = true;
                        break;
                    }

                    case "OFF":
                    {
                        __result = false;
                        break;
                    }

                    default:
                        return true;
                }

                return false;
            }
        }

        // Depth of field
        [HarmonyPatch(typeof(DepthOfFieldComponent), "active", MethodType.Getter)]
        public static class DepthOfFieldComponent_active_Patch
        {
            public static bool Prefix(ref bool __result)
            {
                switch (modSettings.DepthOfField)
                {
                    case "ON":
                    {
                        __result = true;
                        break;
                    }

                    case "OFF":
                    {
                        __result = false;
                        break;
                    }

                    default:
                        return true;
                }

                return false;
            }
        }

        // FXAA
        [HarmonyPatch(typeof(FxaaComponent), "active", MethodType.Getter)]
        public static class FxaaComponent_active_Patch
        {
            public static bool Prefix(ref bool __result)
            {
                switch (modSettings.Fxaa)
                {
                    case "ON":
                    {
                        __result = true;
                        break;
                    }

                    case "OFF":
                    {
                        __result = false;
                        break;
                    }

                    default:
                        return true;
                }

                return false;
            }
        }

        // HDR
        [HarmonyPatch(typeof(PostProcessingContext), "isHdr", MethodType.Getter)]
        public static class PostProcessingContext_isHdr_Patch
        {
            public static bool Prefix(ref bool __result)
            {
                switch (modSettings.HDR)
                {
                    case "ON":
                    {
                        __result = true;
                        break;
                    }

                    case "OFF":
                    {
                        __result = false;
                        break;
                    }

                    default:
                        return true;
                }

                return false;
            }
        }
    }
}
