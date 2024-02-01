using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections;

namespace LongerEntrances
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.LongerEntrances";
        public const string PluginName = "LongerEntrances";
        public const string PluginVer = "1.0.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;
        // static int oldLPBCEGPJNMF;
        static float oldValueEntrance;
        static float oldValueCurtain;
        public static int EntranceDelayButton;
        public static ConfigEntry<bool> DontStopEntrance;
        public static Plugin plugin;

        public static Dictionary<int,float> EntranceDelays = new Dictionary<int,float>();
        public static bool Frozen = false;
        private void Awake()
        {
            Plugin.Log = base.Logger;
            plugin = this;
            PluginPath = Path.GetDirectoryName(Info.Location);

            DontStopEntrance = Config.Bind("General",
             "DontStopEntrance",
             false,
             "If enabled, the mod will prevent entrances from ending when you enter the ring. The only way to end the entrance is to press Skip button.");
        }

        private void OnEnable()
        {
            Harmony.PatchAll();
            Logger.LogInfo($"Loaded {PluginName}!");
        }

        private void OnDisable()
        {
            Harmony.UnpatchSelf();
            Logger.LogInfo($"Unloaded {PluginName}!");
        }
        private void Start()
        {
            LoadDelaysFromFile();
        }
        public static void LoadDelaysFromFile()
        {
            try
            {
                if (File.Exists(Path.Combine(Paths.ConfigPath, "EntranceDelays.json")))
                {
                    using (StreamReader file = File.OpenText(Path.Combine(Paths.ConfigPath, "EntranceDelays.json")))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        EntranceDelays = (Dictionary<int, float>)serializer.Deserialize(file, typeof(Dictionary<int, float>));
                        if (EntranceDelays == null)
                        {
                            Debug.LogError("Failed to load the file for an unknown reason.");
                            EntranceDelays = new();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.LogError("An error has occured while trying to load the file.");
                Log.LogError(e);
                EntranceDelays = new();
            }
        }
        public static void SaveDelaysToFile()
        {
            using (StreamWriter file = File.CreateText(Path.Combine(Paths.ConfigPath, "EntranceDelays.json")))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, EntranceDelays);
            }
        }
        [HarmonyPatch(typeof(CHLPMKEGJBJ), nameof(CHLPMKEGJBJ.DGAGLLKPNJB))]
        [HarmonyPostfix]
        static void CHLPMKEGJBJ_DGAGLLKPNJB(int JJNMLJBGPAH, float MCJHGEHEPMD, float CDNNGHGFALM)
        {
            if (JJNMLJBGPAH > 0)
            {
                if (FFCEGMEAIBP.LOBDMDPMFLK == 1)
                {
                    if (World.location <= 1)
                    {
                        float timer = 0;
                        if (EntranceDelays.ContainsKey(NJBJIIIACEP.OAAMGFLINOB[FFCEGMEAIBP.LPBCEGPJNMF].EMDMDLNJFKP.id))
                        {
                            timer = EntranceDelays[NJBJIIIACEP.OAAMGFLINOB[FFCEGMEAIBP.LPBCEGPJNMF].EMDMDLNJFKP.id];
                        }
                        if (timer != 0)
                        {
                            plugin.StartCoroutine(FreezeEntrants(timer));
                        }
                    }
                }
            }
        }
        static IEnumerator FreezeEntrants(float timer)
        {
            Frozen = true;
            yield return new WaitForSecondsRealtime(timer);
            Frozen = false;

        }
        //WARNING CALLED FOR EVERY CHAR
        [HarmonyPatch(typeof(DFOGOCNBECG), nameof(DFOGOCNBECG.LCAJNIOJAPG))]
        [HarmonyPrefix]
        static void DFOGOCNBECG_LCAJNIOJAPG_Prefix(DFOGOCNBECG __instance)
        {
            if (FFCEGMEAIBP.LOBDMDPMFLK == 1)
            {
                if (DontStopEntrance.Value == true)
                {
                    // oldLPBCEGPJNMF = FFCEGMEAIBP.LPBCEGPJNMF;
                    oldValueEntrance = __instance.AHBNKMMMGFI;
                    
                }
            }
            
        }
        [HarmonyPatch(typeof(DFOGOCNBECG), nameof(DFOGOCNBECG.LCAJNIOJAPG))]
        [HarmonyPostfix]
        static void DFOGOCNBECG_LCAJNIOJAPG_Postfix(DFOGOCNBECG __instance)
        {
            if (FFCEGMEAIBP.LOBDMDPMFLK == 1)
            {
                if (DontStopEntrance.Value == true)
                {
                    if (oldValueEntrance == 2 && __instance.AHBNKMMMGFI != oldValueEntrance)
                    {
                        if (FFCEGMEAIBP.JPEDLBIAKGA != 1)
                        {
                            __instance.AHBNKMMMGFI = oldValueEntrance;
                        }
                    }
                }

            }
        }
        [HarmonyPatch(typeof(DFOGOCNBECG), nameof(DFOGOCNBECG.JCAKMBCFLCK))]
        [HarmonyPostfix]
        static void DFOGOCNBECG_JCAKMBCFLCK_Postfix(DFOGOCNBECG __instance)
        {
            if (FFCEGMEAIBP.LOBDMDPMFLK == 1)
            {
                if (__instance.AHBNKMMMGFI == 1)
                {
                    if (Frozen)
                    {
                        __instance.MOIMCJOBJME = __instance.NJDGEELLAKG;
                        __instance.CEKNDFGOILP = __instance.BMFDFFLPBOJ;
                    }
                }
            }
        }
        [HarmonyPatch(typeof(LIPNHOMGGHF))]
        public static class LIPNHOMGGHF_Patch
        {
            [HarmonyPostfix]
            [HarmonyPatch("ICGNAJFLAHL")]
            public static void ICGNAJFLAHL_Patch()
            {
                if (LIPNHOMGGHF.FAKHAFKOBPB == 60)
                {
                    if (LIPNHOMGGHF.CHLJMEPFJOK == 1)
                    {
                        LIPNHOMGGHF.DFLLBNMHHIH();
                        LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(3, "Entrance Delay", 0f, -250f, 1.6f, 1.6f);
                        EntranceDelayButton = LIPNHOMGGHF.HOAOLPGEBKJ;
                    }
                }
            }
            [HarmonyPrefix]
            [HarmonyPatch(nameof(LIPNHOMGGHF.PMIIOCMHEAE))]
            public static void PMIIOCMHEAE_Postfix()
            {
                Frozen = false;
                Plugin.plugin.StopAllCoroutines();
            }
        }

        [HarmonyPatch(typeof(Scene_Editor))]
        public static class Scene_Editor_Patch
        {
            [HarmonyPostfix]
            [HarmonyPatch("Update")]
            public static void Scene_Editor_Update()
            {
                if (LIPNHOMGGHF.CHLJMEPFJOK == 1)
                {
                    DFOGOCNBECG character = NJBJIIIACEP.OAAMGFLINOB[1];
                    Character chardata = character.EMDMDLNJFKP;
                    int char_id = chardata.id;

                    LoadDelaysFromFile();
                    float delay = 0;
                    if (EntranceDelays.ContainsKey(char_id))
                    {
                        delay = EntranceDelays[char_id];
                    }
                    delay = Mathf.RoundToInt(LIPNHOMGGHF.FKANHDIMMBJ[EntranceDelayButton].ODONMLDCHHF(delay, 1f, 10f, 0f, 40f, 1));
                    if (delay == 0)
                    {
                        EntranceDelays.Remove(char_id);
                    }
                    else
                    {
                        EntranceDelays[char_id] = delay;
                    }
                    SaveDelaysToFile();
                }
            }
        }
    }
}