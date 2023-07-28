using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;

namespace ExplodingBarbedWireBat
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.ExplodingBarbedWireBat";
        public const string PluginName = "ExplodingBarbedWireBat";
        public const string PluginVer = "1.0.1";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;
        public static ConfigEntry<float> configSize;
        public static ConfigEntry<bool> configExplosionOnThrow;


        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);
            configSize = Config.Bind("General",
             "ExplosionSize",
             2f,
             "The size of an explosion");
            configExplosionOnThrow = Config.Bind("General",
             "ExplosionOnThrow",
             true,
             "Make explosions when the weapon hits someone midair");
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
        [HarmonyPatch(typeof(DJEKCMMMFJM))]
        public class DJEKCMMMFJMPatch
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(DJEKCMMMFJM.HEDAFGGNBMD))]
            static void Postfix(DJEKCMMMFJM __instance, int HGCAOGIEJLB, int JCEJPDHEDFD, float IIHKEILOOCC, float FKHCFBFKABO, float ENKKJBHNNGO, float OJJHJAFBDCH)
            {
                if (HGCAOGIEJLB > 0)
                {
                    var x = IIHKEILOOCC;
                    var y = FKHCFBFKABO;
                    var z = ENKKJBHNNGO;
                    IAFGPLGNLKO hgbifncnack = IDHJEMEKEMM.EOBOEGHCHGG[HGCAOGIEJLB];
                    if (hgbifncnack.DEIOJMDIMNM == "Barbed Bat")
                    {
                        if (IIHKEILOOCC == 0f && FKHCFBFKABO == 0f && ENKKJBHNNGO == 0f)
                        {
                            x = hgbifncnack.DCLLKPILCBP;
                            y = hgbifncnack.BEHMHIINOGM;
                            z = hgbifncnack.FFEONFCEHDF;
                        }
                        CLJNCLLMLAO.MPPNGEHNAJL(3, 2, new UnityEngine.Color(10f, 10f, 10f), Plugin.configSize.Value, null, x, y, z, 0f, 0f, 0f, 1);
                    }
                }
            }
        }
        [HarmonyPatch(typeof(IAFGPLGNLKO))]
        public class IAFGPLGNLKOPatch
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(IAFGPLGNLKO.NOCELPOHHIO))]
            static void NOCELPOHHIOPostfix(IAFGPLGNLKO __instance)
            {
                if (Plugin.configExplosionOnThrow.Value)
                {
                    if (__instance.DEIOJMDIMNM == "Barbed Bat")
                    {
                        int test = 1;
                        while (test <= FFKMIEMAJML.HIKHEJJKJAE)
                        {
                            DJEKCMMMFJM gmikimhfabp = FFKMIEMAJML.FJCOPECCEKN[test];
                            if (__instance.BEHMHIINOGM > gmikimhfabp.BEHMHIINOGM && __instance.BEHMHIINOGM < gmikimhfabp.CODHLHPJMGJ(3) + __instance.IADLOLCHIBF / 2f && __instance.PEANOKFAJJJ == 0)
                            {
                                int num = 0;
                                int num2 = gmikimhfabp.CNFBJAIKFNO();
                                if (num2 >= 3)
                                {
                                    num = gmikimhfabp.FFKIMJPLGHH(__instance.DCLLKPILCBP, __instance.FFEONFCEHDF, __instance.AEAMFLIMHGN);
                                }
                                else if (__instance.DLADNAFPGPJ(gmikimhfabp.DCLLKPILCBP, __instance.BEHMHIINOGM, gmikimhfabp.FFEONFCEHDF, 0f) > 0)
                                {
                                    if (num2 == 2)
                                    {
                                        num = 2;
                                    }
                                    else
                                    {
                                        num = 1;
                                    }
                                }
                                if (num2 < 0 && __instance.BEHMHIINOGM < gmikimhfabp.CODHLHPJMGJ(1) - 5f)
                                {
                                    num = 0;
                                }
                                if (num > 0)
                                {
                                    float num3 = (__instance.DCLLKPILCBP + gmikimhfabp.DCLLKPILCBP) / 2f;
                                    float num4 = (__instance.FFEONFCEHDF + gmikimhfabp.FFEONFCEHDF) / 2f;
                                    CLJNCLLMLAO.MPPNGEHNAJL(3, 2, new UnityEngine.Color(10f, 10f, 10f), Plugin.configSize.Value, null, num3, __instance.BEHMHIINOGM, num4, 0f, 0f, 0f, 1);
                                }
                            }
                            test++;
                        }
                    }
                }
            }
        }
    }
   
}