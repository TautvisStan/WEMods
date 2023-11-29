using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;

namespace IncreasedCharacterLimit
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.IncreasedCharacterLimit";
        public const string PluginName = "IncreasedCharacterLimit";
        public const string PluginVer = "1.0.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;
        public static float? oldHealth;


        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);
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
        private static void AKFIIKOMPLL_ODONMLDCHHFPrefix(AKFIIKOMPLL __instance, float __result, float CAAJBNHEFJJ, float OEGLNPMNEOE, float NOGFHHECJBM, float JBPAELFIDOP, ref float LKBOHHGFJFO, int LKJHMOHMKCM)
        {
            if (LIPNHOMGGHF.CHLJMEPFJOK == 3 && LIPNHOMGGHF.PIEMLEPEDFN == 0)
            {
                if(LIPNHOMGGHF.FKANHDIMMBJ[1] == __instance)
                {
                    LKBOHHGFJFO = 100;
                }
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
                    ResizeRequiredArrays();
                }
            }
        }
        //TODO RESIZE ON GAME LOAD
        public static void ResizeRequiredArrays()
        {
            Resize(ref FFCEGMEAIBP.NMMABDGIJNC, NJBJIIIACEP.KLDJKHPCDHM + 1);
            Resize(ref FFCEGMEAIBP.DJCDPNPLICD, NJBJIIIACEP.KLDJKHPCDHM + 1);
            Resize(ref FFCEGMEAIBP.EKKIPMFPMEE, NJBJIIIACEP.KLDJKHPCDHM + 1);
            Resize(ref FFCEGMEAIBP.COIGEGPKLCP, NJBJIIIACEP.KLDJKHPCDHM + 1);
            Resize(ref FFCEGMEAIBP.AJMAFHIBCGJ, NJBJIIIACEP.KLDJKHPCDHM + 1);
            Resize(ref FFCEGMEAIBP.MHHLHMDOFBP, NJBJIIIACEP.KLDJKHPCDHM + 1);
            if (FFCEGMEAIBP.EHIDHAPMAKG > NJBJIIIACEP.KLDJKHPCDHM) FFCEGMEAIBP.EHIDHAPMAKG = NJBJIIIACEP.KLDJKHPCDHM;



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
        //TODO LOCK WEAPON FURNITURE SLIDERS
    }
}