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
        internal bool Dithering = false;
        internal bool Grain = false;
        internal bool Vignette = false;
        internal bool Bloom = false;
        internal bool Shadows = false;
        internal bool AmbientOcclusion = false;
    }

    public class CrystalClear
    {
        private static string _modDirectory;
        private static ModSettings _settings;

        public void Init(string modDirectory, string settingsJson)
        {
            var harmony = HarmonyInstance.Create("ca.gnivler.ModifiersMod");
            _modDirectory = modDirectory;

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

        // TODO all procs off  (transpiler?)

        [HarmonyPatch(typeof(DitheringComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PacthDitheringComp
        {
            public static bool Prefix()
            {
                if (_settings.Dithering)
                {
                    FileLog.Log($"Dither");
                }
                return false;
            }
        }

        [HarmonyPatch(typeof(GrainComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchGrainComp
        {
            public static bool Prefix()
            {
                if (_settings.Grain)
                {
                    FileLog.Log($"Grain");
                }
                return false;
            }
        }

        [HarmonyPatch(typeof(VignetteComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchVignette
        {
            public static bool Prefix()
            {
                if (_settings.Vignette)
                {
                    FileLog.Log($"Vignette");
                }
                return false;
            }
        }

        [HarmonyPatch(typeof(BloomComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchBloom
        {
            public static bool Prefix()
            {
                if (_settings.Bloom)
                {
                    FileLog.Log($"Bloom");
                }
                return false;
            }
        }

        [HarmonyPatch(typeof(ScreenSpaceShadowsComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchShadows
        {
            public static bool Prefix()
            {
                if (_settings.Shadows)
                {
                    FileLog.Log($"Shadows");
                }
                return false;
            }
        }

        [HarmonyPatch(typeof(AmbientOcclusionComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchAmbientOcclusion
        {
            public static bool Prefix()
            {
                if (_settings.AmbientOcclusion)
                {
                    FileLog.Log($"AmbientOcclusion");
                }
                return false;
            }
        }
    }
}