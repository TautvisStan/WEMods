using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using WECCL.Patches;
using BepInEx.Configuration;

namespace PromotionEditorUnlock
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.OverallDisplay";
        public const string PluginName = "OverallDisplay";
        public const string PluginVer = "1.0.1";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;
        public static GameObject OvrText;

        public static int OverallSortingNumber = -987789;

        public static float Xnum1 = 10f;
        public static float Xnum2 = 85f;
        public static float Ynum = 12f;

        public static ConfigEntry<bool> CalculateMainStatsOnly;

        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);

            CalculateMainStatsOnly = Config.Bind("General", "Calculate Main Stats Only", false, "If enabled, the overall will only be calcuated using Strength, Skill, Agility and Stamina. If disabled, the vanilla overall calculation will be used instead");
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

        public static void SetUpGameObjects()
        {
            GameObject name = Characters.gProfile.transform.Find("Stat01/Title").gameObject;
            OvrText = UnityEngine.GameObject.Instantiate(name, name.transform.parent.parent);
            OvrText.transform.localPosition = new Vector3(-259, 90, 0);
            OvrText.GetComponent<UnityEngine.UI.Text>().text = "Overall: xx";
        }
        [HarmonyPatch(typeof(Characters), nameof(Characters.IDGMJPFGHPA))]  //setup the game objects
        [HarmonyPostfix]
        static void Characters_IDGMJPFGHPA()
        {
            SetUpGameObjects();
        }
        [HarmonyPatch(typeof(Characters), nameof(Characters.ACAEDFNBMEP))]
        [HarmonyPostfix]
        static void Characters_ACAEDFNBMEP(int GOOKPABIPBC, int IDHFOGNOIFC = 1)
        {
            UpdateOVRText(Characters.c[GOOKPABIPBC]);
        }
        static void UpdateOVRText(Character character)
        {
            OvrText.GetComponent<UnityEngine.UI.Text>().text = "Overall: " + CalculateOverall(character).ToString("0");
            if(LIPNHOMGGHF.IOMOGCLAPFD == OverallSortingNumber)
            {
                OvrText.GetComponent<UnityEngine.UI.Text>().color = new Color(1f, 0.9f, 0f);
            }
            else
            {
                OvrText.GetComponent<UnityEngine.UI.Text>().color = new Color(1f, 1f, 1f);
            }
        }
        [HarmonyPatch(typeof(Characters), nameof(Characters.MKFNIFJNLEK))]  //Custom sorting
        [HarmonyPostfix]
        static void Characters_MKFNIFJNLEK_Postfix(int DLMLPINGCBA, int GMJKGKDFHOH, int ADKBAGHAIGH = 10)
        {
            if (DLMLPINGCBA == OverallSortingNumber)
            {
                if (GMJKGKDFHOH == WECCL.Content.VanillaCounts.Data.NoFeds + 1)
                {
                    List<Character> charList = new();
                    foreach (int i in LIPNHOMGGHF.JJKLBHGFJNF)
                    {
                        if (i != 0)
                            charList.Add(Characters.c[i]);
                    }
                    LIPNHOMGGHF.JJKLBHGFJNF = (from x in charList.OrderByDescending((Character x) => CalculateOverall(x))
                                               select x.id).Prepend(0).ToArray<int>();
                }
                else
                {
                    LIPNHOMGGHF.JGNMFJLONMA = ADKBAGHAIGH;
                    LIPNHOMGGHF.JJKLBHGFJNF = new int[LIPNHOMGGHF.JGNMFJLONMA + 1];
                    LIPNHOMGGHF.ILEGPMAAJAJ = new int[Characters.no_chars + 1];
                    for (int i = 1; i <= LIPNHOMGGHF.JGNMFJLONMA; i++)
                    {
                        int num = 0;
                        float num2 = -1f;
                        for (int j = 1; j <= Characters.no_chars; j++)
                        {
                            if (LIPNHOMGGHF.ILEGPMAAJAJ[j] == 0 && ((GMJKGKDFHOH >= 0 && GMJKGKDFHOH <= Characters.no_feds && Characters.c[j].fed == GMJKGKDFHOH) || (GMJKGKDFHOH > Characters.no_feds && Characters.c[j].fed >= 1 && Characters.c[j].fed < Characters.no_feds)))
                            {
                                if (CalculateOverall(Characters.c[j]) > num2)
                                {
                                    num = j;
                                    num2 = CalculateOverall(Characters.c[j]);
                                }
                            }
                        }
                        if (num > 0)
                        {
                            LIPNHOMGGHF.JJKLBHGFJNF[i] = num;
                            LIPNHOMGGHF.ILEGPMAAJAJ[num] = i;
                        }
                    }
                }
            }
        }
        [HarmonyPatch(typeof(LIPNHOMGGHF), nameof(LIPNHOMGGHF.ICGNAJFLAHL))]  //reset when moving with controller
        [HarmonyPrefix]
        static void LIPNHOMGGHF_ICGNAJFLAHL(int IPCCBDAFNMC = 0)
        {
            if (LIPNHOMGGHF.IOMOGCLAPFD == OverallSortingNumber+1)
            {
                LIPNHOMGGHF.IOMOGCLAPFD = 0;
            }
        }
        [HarmonyPatch(typeof(Scene_Select_Char), nameof(Scene_Select_Char.Update))]  //click on the text
        [HarmonyPostfix]
        static void Scene_Select_Char_Update_Postfix()
        { 
            if (LIPNHOMGGHF.OMNBAENKANN == 0)
            {
                if (HKJOAJOKOIJ.EOOBMIDCKIF == 5f)
                {
                    if (HKJOAJOKOIJ.GMCCPOAIBHC > OvrText.transform.position.x - Xnum1 * NAEEIFNFBBO.IADPBBEPJKF && HKJOAJOKOIJ.GMCCPOAIBHC < OvrText.transform.position.x + Xnum2 * NAEEIFNFBBO.IADPBBEPJKF && Mathf.Abs(HKJOAJOKOIJ.MINFPCEENFN - OvrText.transform.position.y) <= Ynum * NAEEIFNFBBO.PNHIGOBEEBB)
                    {
                        if (LIPNHOMGGHF.IOMOGCLAPFD != OverallSortingNumber)
                        {
                            LIPNHOMGGHF.IOMOGCLAPFD = OverallSortingNumber;
                        }
                        else
                        {
                            LIPNHOMGGHF.IOMOGCLAPFD = 0;
                        }
                        CHLPMKEGJBJ.DNNPEAOCDOG(CHLPMKEGJBJ.CPJAPKAKGEE[1], 0f, 1f);
                        LIPNHOMGGHF.ICGNAJFLAHL(0);
                        LIPNHOMGGHF.CMOMBJMMOBK = 5f;
                        HKJOAJOKOIJ.LMADDGDMBGB = 5f;
                        Characters.ACAEDFNBMEP(Characters.foc, 0);                        
                    }
                }
            }
        }
        [HarmonyPatch(typeof(AKFIIKOMPLL), nameof(AKFIIKOMPLL.INCMCAGJLPE))]  //scaling the button bar
        [HarmonyPostfix]
        static void AKFIIKOMPLL_INCMCAGJLPE(AKFIIKOMPLL __instance, int GOOKPABIPBC)
        {
            if (LIPNHOMGGHF.IOMOGCLAPFD == OverallSortingNumber)
            {
                __instance.JBCKJGIJFGK.transform.localScale = new Vector3(CalculateOverall(Characters.c[GOOKPABIPBC]) /100, 1f, 1f);
            }
        }
        public static float CalculateOverall(Character c)
        {
            if(CalculateMainStatsOnly.Value == true)
            {
                return (c.stat[2] + c.stat[3] + c.stat[4] + c.stat[5]) /4;
            }
            else
            {
                return c.FOOLOEEKEAJ(0);
            }
        }
    }


}

//LIPNHOMGGHF.IOMOGCLAPFD = -1;
//Characters.c[Characters.foc].FOOLOEEKEAJ(0)