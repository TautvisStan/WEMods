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
        public const string PluginVer = "0.0.5";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);
        internal static string PluginPath;
        public static GameObject SteamObject;
        public static SteamLobby steamLobby = null;
        public static SteamP2PNetworking steamNetworking = null;
        public static int MPLobbyPage { get; set; } = CollectibleCards2.Plugin.CardsMenuPage + 10;
        public static int GameplayPage { get; set; } = CollectibleCards2.Plugin.CardsMenuPage + 11;
        public static bool Initialized = false;
        public static bool CallbacksAlreadyDone = false;
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
            DontDestroyOnLoad(SteamObject);
            SteamObject.SetActive(false);
        }
        public static void InitCardGameNetworking()
        {

            SteamObject.SetActive(true);

        }
        public static void EndCardGameNetworking()
        {
            if(steamLobby.currentLobbyID != CSteamID.Nil)
            {
                steamLobby.LeaveLobby();
            }
            SteamObject.SetActive(false);
        }
        private void Update()
        {
            
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