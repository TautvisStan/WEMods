using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BetterStrikesOnly
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.BetterStrikesOnly";
        public const string PluginName = "BetterStrikesOnly";
        public const string PluginVer = "1.0.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        public static List<int> DefaultAllowedMovesFront = new();

        public static HashSet<int> CachedBannedMovesFront = new();
        public static HashSet<int> CachedBannedMovesBack = new();
        public static HashSet<int> CachedAllowedMoves = new();

        public static HashSet<int> CachedBannedMovesHead = new();
        public static HashSet<int> CachedBannedMovesLegs = new();

        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);


            DefaultAllowedMovesFront.Add(650);
            DefaultAllowedMovesFront.Add(208);

            CachedAllowedMoves.Add(0);
            CachedBannedMovesLegs.Add(356);
            foreach (int move in DefaultAllowedMovesFront)
            {
                CachedAllowedMoves.Add(move);
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

        [HarmonyPatch(typeof(DFOGOCNBECG))]
        public static class DFOGOCNBECGPatch
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(DFOGOCNBECG.JOMGNBHDNBB))]
            public static void JOMGNBHDNBBPrefix(DFOGOCNBECG __instance, DFOGOCNBECG ELPIOHCPOIJ, ref int NOLKIINBMHA, ref float EFDKBFOEAGO)
            {
                if (FFCEGMEAIBP.PDNEOPFBGJF == 1 && FFCEGMEAIBP.LOBDMDPMFLK >= 2 && SceneManager.GetActiveScene().name == "Game" && __instance.FIEMGOLBHIO == 1 && ELPIOHCPOIJ.FIEMGOLBHIO == 1)
                {
                    
                    int performingMove = NOLKIINBMHA;
                    if (CachedAllowedMoves.Contains(NOLKIINBMHA)) return;
                    if (CachedBannedMovesFront.Contains(NOLKIINBMHA))
                    {
                        EFDKBFOEAGO = 0;
                        int randomindex = UnityEngine.Random.Range(0, DefaultAllowedMovesFront.Count);
                        NOLKIINBMHA = DefaultAllowedMovesFront[randomindex];
                        return;
                    }
                    if (CachedBannedMovesBack.Contains(NOLKIINBMHA))
                    {
                        EFDKBFOEAGO = 0;
                        NOLKIINBMHA = 0;
                        return;
                    }
                    if(CachedBannedMovesHead.Contains(NOLKIINBMHA))
                    {
                        EFDKBFOEAGO = 0;
                        NOLKIINBMHA = 414;
                        return;
                    }
                    if (CachedBannedMovesLegs.Contains(NOLKIINBMHA))
                    {
                        EFDKBFOEAGO = 0;
                        NOLKIINBMHA = 452;
                        return;
                    }
                    //front
                    if (Array.Exists(MBLIOKEDHHB.APGCICKNCFO, move => move == performingMove))
                    {
                        EFDKBFOEAGO = 0;
                        CachedBannedMovesFront.Add(NOLKIINBMHA);
                        int randomindex = UnityEngine.Random.Range(0, DefaultAllowedMovesFront.Count);
                        NOLKIINBMHA = DefaultAllowedMovesFront[randomindex];
                        return;
                    }
                    //back
                    if (Array.Exists(MBLIOKEDHHB.FJMDHGDJGMJ, move => move == performingMove))
                    {
                        EFDKBFOEAGO = 0;
                        CachedBannedMovesBack.Add(NOLKIINBMHA);
                        NOLKIINBMHA = 0;
                        return;
                    }

                    //ground grapples
                    if (Array.Exists(MBLIOKEDHHB.PLJFOJLMFNL, move => move == performingMove))
                    {
                        EFDKBFOEAGO = 0;
                        CachedBannedMovesHead.Add(NOLKIINBMHA);
                        NOLKIINBMHA = 414;
                        return;
                    }
                    if (Array.Exists(MBLIOKEDHHB.GNKONKHDGFD, move => move == performingMove))
                    {
                        EFDKBFOEAGO = 0;
                        CachedBannedMovesLegs.Add(NOLKIINBMHA);
                        NOLKIINBMHA = 452;
                        return;
                    }
                    CachedAllowedMoves.Add(NOLKIINBMHA);
                    return;
                }
                
                else
                {
                    return;
                }
            }
        }
    }
}