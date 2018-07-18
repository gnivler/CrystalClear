using Harmony;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.PostProcessing;

namespace CrystalClear
{
    class ModSettings
    {
        internal bool Debug = true;
        internal bool EnablePostProc = true;
        internal bool Dithering = true;
        internal bool Grain = true;
        internal bool Vignette = true;
        internal bool Bloom = true;
        internal bool Shadows = true;
    }

    public class CrystalClear
    {
        private static ModSettings _settings;
        private static string _modDirectory;
        public static void Init(string modDirectory, string settingsJson)
        {
            FileLog.Log("Init");
            var harmony = HarmonyInstance.Create("ca.gnivler.CrystalClear");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            try
            {
                _modDirectory = modDirectory;
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
            public static bool Prefix()
            {
                FileLog.Log("PostProcessingModel Prefix");
                return true;
            }

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                FileLog.Log("PostProcessingModel Transpiler");
                // var codes = new List<CodeInstruction>(instructions);
                // var sb = new StringBuilder();
                //
                // foreach (CodeInstruction code in codes)
                // {
                //     FileLog.Log($"{code.opcode} {code.operand}".ToString());
                // }
                //
                // //codes.Where(x => x.opcode == OpCodes.Ldfld).Select(x => x.opcode = OpCodes.Ldc_I4_0);
                // for (var i = 0; i < codes.Count; i++)
                // {
                //     FileLog.Log(codes[i] + "\n");
                //     if (codes[i].opcode == OpCodes.Ldfld)
                //     {
                //         codes[i].opcode = OpCodes.Ldc_I4_0;
                //         codes[i].operand = null;
                //     }               
                // }
                //
                // foreach (CodeInstruction code in codes)
                // {
                //     FileLog.Log($"{code.opcode} {code.operand}".ToString());
                // }
                //
                // //FileLog.Log(codes.Select(x => x.operand).ToString());
                return instructions;
            }
        }

        [HarmonyPatch(typeof(DitheringComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PacthDitheringComp
        {
            public static bool Prefix(ref bool __result)
            {
                bool flag = false;
                if (!flag)
                    FileLog.Log("Dithering");
                flag = true;
                __result = false;
                return false;
            }
        }

        [HarmonyPatch(typeof(GrainComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchGrainComp
        {
            public static bool Prefix(ref bool __result)
            {
                bool flag = false;
                if (!flag)
                    FileLog.Log("Grain");
                flag = true;
                __result = false;
                return false;
            }
        }

        [HarmonyPatch(typeof(VignetteComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchVignette
        {
            public static bool Prefix(ref bool __result)
            {
                bool flag = false;
                if (!flag)
                    FileLog.Log("Vignette");
                flag = true;
                __result = false;
                return false;
            }
        }

        [HarmonyPatch(typeof(BloomComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchBloom
        {
            public static bool Prefix(ref bool __result)
            {
                bool flag = false;
                if (!flag)
                    FileLog.Log("Bloom");
                flag = true;
                __result = false;
                return false;
            }
        }

        [HarmonyPatch(typeof(ScreenSpaceShadowsComponent))]
        [HarmonyPatch("active", PropertyMethod.Getter)]
        public static class PatchShadows
        {
            public static bool Prefix(ref bool __result)
            {
                bool flag = false;
                if (!flag)
                    FileLog.Log("Shadows");
                flag = true;
                __result = false;
                return false;
            }
        }
    }
}