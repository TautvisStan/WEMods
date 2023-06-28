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
        public const string PluginVer = "1.0.0";

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
    }
    [HarmonyPatch(typeof(GMIKIMHFABP))]
    public class GMIKIMHFABPPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("ABIDABBEJMO")]
        static void Postfix(GMIKIMHFABP __instance, int BCIDGGGOALA, int EPBLHHBCDEA, float AHLIFAAOOIE, float MAEAJHAIKPP, float MEHJAALLLFO, float JOEIIJHGPIK)
        {
            if (BCIDGGGOALA > 0)
            {
                var x = AHLIFAAOOIE;
                var y = MAEAJHAIKPP;
                var z = MEHJAALLLFO;
                HGBIFNCNACK hgbifncnack = GCOCDCCEALD.IAKLPJIDIGC[BCIDGGGOALA];
                if (hgbifncnack.FLAJNJNPDCB == "Barbed Bat")
                {
                    if (AHLIFAAOOIE == 0f && MAEAJHAIKPP == 0f && MEHJAALLLFO == 0f)
                    {
                        x = hgbifncnack.PPFFBIPHOEE;
                        y = hgbifncnack.EDHBIOFAKNL;
                        z = hgbifncnack.OIHBMKLFEBJ;
                    }
                    EFIBMNEKJFB.COMPLDGIDLF(3, 2, new UnityEngine.Color(10f, 10f, 10f), Plugin.configSize.Value, null, x, y, z, 0f, 0f, 0f, 1);
                }
            }
        }
    }
    [HarmonyPatch(typeof(HGBIFNCNACK))]
    public class HGBIFNCNACKPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("BNEPKIKILOB")]
        static void Postfix(HGBIFNCNACK __instance)
        {
            if (Plugin.configExplosionOnThrow.Value)
            {
                if (__instance.FLAJNJNPDCB == "Barbed Bat")
                {
                    int test = 1;
                    while (test <= AMJONEKIAID.MOGFLKPBLIK)
                    {
                        GMIKIMHFABP gmikimhfabp = AMJONEKIAID.NCPIJJFEDFL[test];
                        if (__instance.EDHBIOFAKNL > gmikimhfabp.EDHBIOFAKNL && __instance.EDHBIOFAKNL < gmikimhfabp.CPKMGEBANCB(3) + __instance.GDDJHEAGPLH / 2f && __instance.JDENBNFMLGJ == 0)
                        {
                            int num = 0;
                            int num2 = gmikimhfabp.BGBOCAMEIFN();
                            if (num2 >= 3)
                            {
                                num = gmikimhfabp.KBBGONBBJEL(__instance.PPFFBIPHOEE, __instance.OIHBMKLFEBJ, __instance.KCIBKFDHPPD);
                            }
                            else if (__instance.IJICAMIHPFF(gmikimhfabp.PPFFBIPHOEE, __instance.EDHBIOFAKNL, gmikimhfabp.OIHBMKLFEBJ, 0f) > 0)
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
                            if (num2 < 0 && __instance.EDHBIOFAKNL < gmikimhfabp.CPKMGEBANCB(1) - 5f)
                            {
                                num = 0;
                            }
                            if (num > 0)
                            {
                                float num3 = (__instance.PPFFBIPHOEE + gmikimhfabp.PPFFBIPHOEE) / 2f;
                                float num4 = (__instance.OIHBMKLFEBJ + gmikimhfabp.OIHBMKLFEBJ) / 2f;
                                EFIBMNEKJFB.COMPLDGIDLF(3, 2, new UnityEngine.Color(10f, 10f, 10f), Plugin.configSize.Value, null, num3, __instance.EDHBIOFAKNL, num4, 0f, 0f, 0f, 1);
                            }
                        }
                        test++;
                    }
                }
            }
        }
    }
}