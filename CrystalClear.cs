using System;
using Harmony;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine.PostProcessing;
using static CrystalClear.CrystalClear;

namespace CrystalClear
{
    class ModSettings
    {
        internal bool Debug = false;
        internal bool DisableAllPostProc = false;
        internal bool Dithering = true;
        internal bool Grain = true;
        internal bool Vignette = true;
        internal bool Bloom = true;
        internal bool Shadows = true;
        internal bool AmbientOcclusion = true;
    }

    public class CrystalClear
    {
        private static ModSettings _settings;

        public void Init(string modDirectory, string settingsJson)
        {
            var harmony = HarmonyInstance.Create("ca.gnivler.ModifiersMod");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            try
            {
                _settings = JsonConvert.DeserializeObject<ModSettings>(settingsJson);

            }
            catch (Exception e)
            {
                FileLog.Log(e.ToString());
            }
        }

        [HarmonyPatch(typeof(PostProcessingModel))]
        [HarmonyPatch("enabled", PropertyMethod.Getter)]
        public static class PatchPostProcessing
        {
            public static bool Prefx()
            {
                return (!_settings.DisableAllPostProc);
            }
        }

        [HarmonyPatch(typeof(DitheringComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PacthDitheringComp
        {
            public static bool Prefix()
            {
                return (!_settings.Dithering);
            }
        }

        [HarmonyPatch(typeof(GrainComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchGrainComp
        {
            public static bool Prefix()
            {
                return (!_settings.Grain);
            }
        }

        [HarmonyPatch(typeof(VignetteComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchVignette
        {
            public static bool Prefix()
            {
                return (!_settings.Vignette);
            }
        }

        [HarmonyPatch(typeof(BloomComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchBloom
        {
            public static bool Prefix()
            {
                return (!_settings.Bloom);
            }
        }

        [HarmonyPatch(typeof(ScreenSpaceShadowsComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchShadows
        {
            public static bool Prefix()
            {
                return (!_settings.Shadows);
            }
        }

        [HarmonyPatch(typeof(AmbientOcclusionComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchAmbientOcclusion
        {
            public static bool Prefix()
            {
                return (!_settings.AmbientOcclusion);
            }
        }
    }
}