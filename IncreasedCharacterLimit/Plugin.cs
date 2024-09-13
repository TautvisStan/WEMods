using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine.SceneManagement;

namespace IncreasedCharacterLimit
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.IncreasedCharacterLimit";
        public const string PluginName = "IncreasedCharacterLimit";
        public const string PluginVer = "1.0.2";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;
        public static float? oldHealth;

        public static bool resized = false;

        public static int Limit { get; set; } = 50;
        public static int hardocdedLimit { get; set; } = 999;
        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);
            if(Characters.no_chars < hardocdedLimit)
            {
                Limit = Characters.no_chars;
            }
            else
            {
                Limit = hardocdedLimit;
            }
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
        [HarmonyPatch(typeof(AKFIIKOMPLL), nameof(AKFIIKOMPLL.ODONMLDCHHF))]
        [HarmonyPrefix]
        private static void AKFIIKOMPLL_ODONMLDCHHFPrefix_OPTIONS(AKFIIKOMPLL __instance, float __result, float CAAJBNHEFJJ, float OEGLNPMNEOE, float NOGFHHECJBM, float JBPAELFIDOP, ref float LKBOHHGFJFO, int LKJHMOHMKCM)
        {
            if (SceneManager.GetActiveScene().name != "Options") return;
            if (LIPNHOMGGHF.CHLJMEPFJOK == 3 && LIPNHOMGGHF.PIEMLEPEDFN == 0)
            {
                if(LIPNHOMGGHF.FKANHDIMMBJ[1] == __instance)
                {
                    LKBOHHGFJFO = Limit;
                }
            }    
        }
        [HarmonyPatch(typeof(AKFIIKOMPLL), nameof(AKFIIKOMPLL.ODONMLDCHHF))]
        [HarmonyPrefix]
        private static void AKFIIKOMPLL_ODONMLDCHHFPrefix_SETUP(AKFIIKOMPLL __instance, float __result, float CAAJBNHEFJJ, float OEGLNPMNEOE, float NOGFHHECJBM, float JBPAELFIDOP, ref float LKBOHHGFJFO, int LKJHMOHMKCM)
        {
            if (SceneManager.GetActiveScene().name != "Match_Setup") return;
            if (LIPNHOMGGHF.CHLJMEPFJOK == 1 && LIPNHOMGGHF.ODOAPLMOJPD == 2)
            {
                if (LIPNHOMGGHF.FKANHDIMMBJ[1] == __instance)
                {
                    if (LKBOHHGFJFO > 100) LKBOHHGFJFO = 100;
                }
                if (LIPNHOMGGHF.FKANHDIMMBJ[4] == __instance)
                {
                    if (LKBOHHGFJFO > 150) LKBOHHGFJFO = 150;
                }
            }
        }
        [HarmonyPatch(typeof(Scene_Titles), nameof(Scene_Titles.Start))]
        [HarmonyPostfix]
        private static void TitlesPostfix(Scene_Titles __instance)
        {
            if(!resized)
            {
                ResizeRequiredArrays();
                resized = true;
            }
        }

            [HarmonyPatch(typeof(AKFIIKOMPLL), nameof(AKFIIKOMPLL.ODONMLDCHHF))]
             [HarmonyPostfix]
             private static void AKFIIKOMPLL_ODONMLDCHHFPostfix(AKFIIKOMPLL __instance, float __result, float CAAJBNHEFJJ, float OEGLNPMNEOE, float NOGFHHECJBM, float JBPAELFIDOP, ref float LKBOHHGFJFO, int LKJHMOHMKCM)
             {
                 if (LIPNHOMGGHF.CHLJMEPFJOK == 3 && LIPNHOMGGHF.PIEMLEPEDFN == 0)
                 {
                     if (LIPNHOMGGHF.FKANHDIMMBJ[1] == __instance)
                     {
                         NJBJIIIACEP.KLDJKHPCDHM = (int)__result;
                     }
                 }
             }
        public static void ResizeRequiredArrays()
        {
            Resize(ref FFCEGMEAIBP.NMMABDGIJNC, Limit + 1);
            Resize(ref FFCEGMEAIBP.DJCDPNPLICD, Limit + 1);
            Resize(ref FFCEGMEAIBP.EKKIPMFPMEE, Limit + 1);
            Resize(ref FFCEGMEAIBP.COIGEGPKLCP, Limit + 1);
            Resize(ref FFCEGMEAIBP.AJMAFHIBCGJ, Limit + 1);
            Resize(ref FFCEGMEAIBP.MHHLHMDOFBP, Limit + 1);
            //FFCEGMEAIBP.EHIDHAPMAKG = hardocdedLimit;
            NJBJIIIACEP.KLDJKHPCDHM = Limit;



        }
        public static void Resize<T>(ref T[] original, int newsize)
        {
            T[] newArray = new T[newsize];
            int copysize;
            if(original.Length > newsize)
            {
                copysize = newsize;
            }
            else
            {
                copysize = original.Length;
            }
            Array.Copy(original, newArray, copysize);
            original = newArray;
        }
    }
}