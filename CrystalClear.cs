using Harmony;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using BattleTech;
using BattleTech.Rendering;
using Org.BouncyCastle.Math.Raw;
using UnityEngine;
using UnityEngine.PostProcessing;
using static CrystalClear.Logger;

namespace CrystalClear
{
    public class ModSettings
    {
        public bool Dithering;
        public bool Grain;
        public bool Vignette;
        public bool Bloom;
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
    }

    public static class CrystalClear
    {
        private static ModSettings settings;
        public static string modDirectory;

        public static void Init(string modDirectory, string settingsJson)
        {
            FileLog.logPath = Path.Combine(modDirectory, "log.txt");
            Clear();
            var harmony = HarmonyInstance.Create("ca.gnivler.ScorchedEarth");
            //HarmonyInstance.DEBUG = false;
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            try
            {
                CrystalClear.modDirectory = modDirectory;
                settings = JsonConvert.DeserializeObject<ModSettings>(settingsJson);
            }
            catch (Exception e)
            {
                Error(e);
            }

            if (settings.Grunge || settings.Scanlines) return;
            int mainTex = Shader.PropertyToID("_MainTex");
            Type uniformsType = AccessTools.Inner(typeof(BTPostProcess), "Uniforms");
            if (!settings.Grunge)
            {
                AccessTools.Field(uniformsType, "_GrungeTex").SetValue(null, mainTex);
            }

            if (!settings.Scanlines)
            {
                AccessTools.Field(uniformsType, "_ScanlineTex").SetValue(null, mainTex);
            }
        }

        [HarmonyPatch(typeof(DitheringComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchDitheringComp
        {
            public static bool Prefix(ref bool __result)
            {
                __result = settings.Dithering;
                return false;
            }
        }

        [HarmonyPatch(typeof(GrainComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchGrainComp
        {
            public static bool Prefix(ref bool __result)
            {
                __result = settings.Grain;
                return false;
            }
        }

        [HarmonyPatch(typeof(VignetteComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchVignette
        {
            public static bool Prefix(ref bool __result)
            {
                __result = settings.Vignette;
                return false;
            }
        }

        [HarmonyPatch(typeof(BloomComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchBloom
        {
            public static bool Prefix(ref bool __result)
            {
                __result = settings.Bloom;
                return false;
            }
        }

        [HarmonyPatch(typeof(ScreenSpaceShadowsComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchShadows
        {
            public static bool Prefix(ref bool __result)
            {
                __result = settings.Shadows;
                return false;
            }
        }

        [HarmonyPatch(typeof(ChromaticAberrationComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class ChromaticAberration
        {
            public static bool Prefix(ref bool __result)
            {
                __result = settings.ChromaticAberration;
                return false;
            }
        }

        [HarmonyPatch(typeof(EyeAdaptationComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class EyeAdaptation
        {
            public static bool Prefix(ref bool __result)
            {
                __result = settings.EyeAdaptation;
                return false;
            }
        }

        [HarmonyPatch(typeof(FogComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class Fog
        {
            public static bool Prefix(ref bool __result)
            {
                __result = settings.Fog;
                return false;
            }
        }

        [HarmonyPatch(typeof(ColorGradingComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class ColorGrading
        {
            public static bool Prefix(ref bool __result)
            {
                __result = settings.ColorGrading;
                return false;
            }
        }

        [HarmonyPatch(typeof(AmbientOcclusionComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class AmbientOcclusion
        {
            public static bool Prefix(ref bool __result)
            {
                __result = settings.AmbientOcclusion;
                return false;
            }
        }

        [HarmonyPatch(typeof(TaaComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class Taa
        {
            public static bool Prefix(ref bool __result)
            {
                __result = settings.Taa;
                return false;
            }
        }

        [HarmonyPatch(typeof(DepthOfFieldComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class DepthOfField
        {
            public static bool Prefix(ref bool __result)
            {
                __result = settings.DepthOfField;
                return false;
            }
        }

        [HarmonyPatch(typeof(FxaaComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class Fxaa
        {
            public static bool Prefix(ref bool __result)
            {
                __result = settings.Fxaa;
                return false;
            }
        }

        [HarmonyPatch(typeof(PostProcessingContext))]
        [HarmonyPatch("isHdr", PropertyMethod.Getter)]
        public static class HdrPatch
        {
            public static bool Prefix(ref bool __result)
            {
                __result = settings.HDR;
                return false;
            }
        }

        //   System.FormatException: Method BattleTech.Rendering.FogScattering
        //   .GetHaltonValue(System.Int32, System.Int32) cannot be patched.
        //   Reason: Invalid IL code in (wrapper dynamic-method) BattleTech.Rendering.FogScattering
        //   :GetHaltonValue_Patch1 (object,int,int): IL_0013: stloc.2   
        //[HarmonyPatch(typeof(FogScattering), "GetHaltonValue")]
        //public static class HaltonPatch
        //{
        //    public static bool Prefix(ref float __result)
        //    {
        //        Debug("Jabber");
        //        __result = 0;
        //        return false;
        //    }
        //
        //    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        //    {
        //        var codes = new List<CodeInstruction>(instructions);
        //        var newCodes = new List<CodeInstruction>();
        //
        //        newCodes.Add(new CodeInstruction(OpCodes.Nop));
        //        //codes.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
        //        //codes.Add(new CodeInstruction(OpCodes.Ret));
        //   
        //
        //        
        //        return newCodes;
        //    }
        //}
    }
}