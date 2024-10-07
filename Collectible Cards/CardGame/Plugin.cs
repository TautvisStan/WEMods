using BepInEx;
using BepInEx.Logging;
using CollectibleCards2;
using HarmonyLib;
using Steamworks;
using System.IO;
using UnityEngine;

namespace CardGame
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.CardGame";
        public const string PluginName = "CardGame";
        public const string PluginVer = "0.0.1";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);
        internal static string PluginPath;
        public GameObject SteamObject;
        public SteamLobby steamLobby = null;
        public SteamP2PNetworking steamNetworking = null;

        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);
        }
        private void Start()
        {
            SteamObject = Instantiate(new GameObject("CardGameSteamObject"));
            steamLobby = SteamObject.AddComponent<SteamLobby>();
            steamNetworking = SteamObject.AddComponent<SteamP2PNetworking>();
            GameObject.DontDestroyOnLoad(SteamObject);
        }
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Return) && SteamLobby.currentLobbyID == CSteamID.Nil)
            {
                steamLobby.StartLobby();
            }
            if(Input.GetKeyDown(KeyCode.Return) && SteamLobby.currentLobbyID != CSteamID.Nil)
            {
                SteamP2PNetworking.SEND_CARD(CardMenu.Cards[CardMenu.DisplayedCardIndex - 1]);
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
    }
}