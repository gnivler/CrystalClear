using Harmony;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using BattleTech.Rendering;
using BattleTech.UI.Tooltips;
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

            int mainTex = Shader.PropertyToID("_MainTex");
            Type uniformsType = AccessTools.Inner(typeof(BTPostProcess), "Uniforms");

            if (modSettings.Grunge == "OFF")
            {
                LogDebug("Patching UI grunge");
                AccessTools.Field(uniformsType, "_GrungeTex").SetValue(null, mainTex);
            }

            if (modSettings.Scanlines == "OFF")
            {
                LogDebug("Patching UI scanlines");
                AccessTools.Field(uniformsType, "_ScanlineTex").SetValue(null, mainTex);
            }

            if (modSettings.Dithering == "OFF")
            {
                LogDebug("Patching dithering");
                AccessTools.Field(uniformsType, "_DitheringTex").SetValue(null, 0);
            }
        }

        [HarmonyPatch(typeof(BTPostProcess), "OnEnable", MethodType.Normal)]
        public static class BTPostProcess_Ctor_Patch
        {
            public static void Postfix(ref float ___uiBloomIntensity)
            {
                if (modSettings.UIBloom == "OFF")
                {
                    LogDebug("Patching UI bloom");
                    ___uiBloomIntensity = 0f;
                }
            }
        }

        [HarmonyPatch(typeof(MenuCamera), "OnEnable", MethodType.Normal)]
        public static class MenuCamera_Ctor_Patch
        {
            public static void Postfix(ref float ___uiBloomIntensity)
            {
                if (modSettings.UIBloom == "OFF")
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
                // implicitly does nothing if 'NotSet'
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
                }

                return false;
            }
        }

        [HarmonyPatch(typeof(GrainComponent), "active", MethodType.Getter)]
        public static class PatchGrainComp
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

        [HarmonyPatch(typeof(VignetteComponent), "active", MethodType.Getter)]
        public static class PatchVignette
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

        [HarmonyPatch(typeof(BloomComponent), "active", MethodType.Getter)]
        public static class PatchBloom
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

        [HarmonyPatch(typeof(ScreenSpaceShadowsComponent), "active", MethodType.Getter)]
        public static class PatchShadows
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

        [HarmonyPatch(typeof(ChromaticAberrationComponent), "active", MethodType.Getter)]
        public static class ChromaticAberration
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

        [HarmonyPatch(typeof(EyeAdaptationComponent), "active", MethodType.Getter)]
        public static class EyeAdaptation
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

        [HarmonyPatch(typeof(FogComponent), "active", MethodType.Getter)]
        public static class Fog
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

        [HarmonyPatch(typeof(ColorGradingComponent), "active", MethodType.Getter)]
        public static class ColorGrading
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

        [HarmonyPatch(typeof(AmbientOcclusionComponent), "active", MethodType.Getter)]
        public static class AmbientOcclusion
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

        [HarmonyPatch(typeof(TaaComponent), "active", MethodType.Getter)]
        public static class Taa
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

        [HarmonyPatch(typeof(DepthOfFieldComponent), "active", MethodType.Getter)]
        public static class DepthOfField
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

        [HarmonyPatch(typeof(FxaaComponent), "active", MethodType.Getter)]
        public static class Fxaa
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

        [HarmonyPatch(typeof(PostProcessingContext), "isHdr", MethodType.Getter)]
        public static class HdrPatch
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