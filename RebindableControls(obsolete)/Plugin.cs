using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using UnityEngine;

namespace RebindableControls
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.RebindableControls";
        public const string PluginName = "RebindableControls";
        public const string PluginVer = "1.0.2";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;
        public static ConfigEntry<KeyCode> ConfigUp;
        public static ConfigEntry<KeyCode> ConfigDown;
        public static ConfigEntry<KeyCode> ConfigRight;
        public static ConfigEntry<KeyCode> ConfigLeft;

        public static ConfigEntry<KeyCode> ConfigS;
        public static ConfigEntry<KeyCode> ConfigX;
        public static ConfigEntry<KeyCode> ConfigZ;
        public static ConfigEntry<KeyCode> ConfigA;
        public static ConfigEntry<KeyCode> ConfigSpace;
        public static ConfigEntry<KeyCode> ConfigShift;
        public static ConfigEntry<KeyCode> ConfigControl;
        public static ConfigEntry<KeyCode> ConfigTab;
        public static ConfigEntry<KeyCode> ConfigJoin;


        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);
            ConfigUp = Config.Bind("General",
             "Up Button",
             KeyCode.UpArrow,
             "Move up button");
            ConfigDown = Config.Bind("General",
             "Down Button",
             KeyCode.DownArrow,
             "Move down button");
            ConfigLeft = Config.Bind("General",
             "Left Button",
             KeyCode.LeftArrow,
             "Move left button");
            ConfigRight = Config.Bind("General",
             "Right Button",
             KeyCode.RightArrow,
             "Move right button");
            ConfigA = Config.Bind("General",
             "Attack Button",
             KeyCode.A,
             "Attack button");
            ConfigS = Config.Bind("General",
             "Grapple Button",
             KeyCode.S,
             "Grapple button");
            ConfigZ = Config.Bind("General",
             "Run Button",
             KeyCode.Z,
             "Run button");
            ConfigX = Config.Bind("General",
             "Pick Up Button",
             KeyCode.X,
             "Pick up button");
            ConfigSpace = Config.Bind("General",
             "Taunt Button",
             KeyCode.Space,
             "Taunt button");
            ConfigShift = Config.Bind("General",
             "Focus (Shift) Button",
             KeyCode.LeftShift,
             "Change focus (shift) button");
            ConfigControl = Config.Bind("General",
             "Focus (Control) Button",
             KeyCode.LeftControl,
             "Change focus (control) button");
            ConfigTab = Config.Bind("General",
             "Change Control (TAB) Button",
             KeyCode.Tab,
             "Change control (TAB) button");
            ConfigJoin = Config.Bind("General",
             "Join Button",
             KeyCode.None,
             "\"Join\" button that let's you join a game in progress");

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

        [HarmonyPatch(typeof(BJMGCKGNCHO))]
        public static class BJMGCKGNCHO_Patch
        {
            [HarmonyPostfix]
            [HarmonyPatch("NCOEPCFFBJA")]
            public static void NCOEPCFFBJA_Patch(BJMGCKGNCHO __instance)
            {
                if (__instance.PLFGKLGCOMD == 0 && __instance.BPJFLJPKKJK == 1 && __instance.AHBNKMMMGFI > 0)
                {
                    __instance.JLBOEDNDIPI = 0f;
                    __instance.MGJCMCPCPDN = 0f;
                    __instance.IOIJFFLMBCH[1] = 0;
                    __instance.IOIJFFLMBCH[2] = 0;
                    __instance.IOIJFFLMBCH[3] = 0;
                    __instance.IOIJFFLMBCH[4] = 0;
                    __instance.IOIJFFLMBCH[5] = 0;
                    __instance.IOIJFFLMBCH[6] = 0;
                    __instance.IMBKMMOCBBF = 0;
                    if (Input.GetKey(Plugin.ConfigJoin.Value))
                    {
                        __instance.JIIMLBLGKAL(0);
                    }
                    if (Input.GetKey(Plugin.ConfigUp.Value))
                    {
                        __instance.JLBOEDNDIPI = 1f;
                    }
                    if (Input.GetKey(Plugin.ConfigDown.Value))
                    {
                        __instance.JLBOEDNDIPI = -1f;
                    }
                    if (Input.GetKey(Plugin.ConfigRight.Value))
                    {
                        __instance.MGJCMCPCPDN = 1f;
                    }
                    if (Input.GetKey(Plugin.ConfigLeft.Value))
                    {
                        __instance.MGJCMCPCPDN = -1f;
                    }
                    if (Input.GetKey(Plugin.ConfigS.Value))
                    {
                        __instance.IOIJFFLMBCH[1] = 1;
                    }
                    if (Input.GetKey(Plugin.ConfigX.Value))
                    {
                        __instance.IOIJFFLMBCH[2] = 1;
                    }
                    if (Input.GetKey(Plugin.ConfigZ.Value))
                    {
                        __instance.IOIJFFLMBCH[3] = 1;
                    }
                    if (Input.GetKey(Plugin.ConfigA.Value))
                    {
                        __instance.IOIJFFLMBCH[4] = 1;
                    }
                    if (Input.GetKey(Plugin.ConfigSpace.Value))
                    {
                        __instance.IOIJFFLMBCH[5] = 1;
                    }
                    if (Input.GetKey(Plugin.ConfigShift.Value))
                    {
                        __instance.IOIJFFLMBCH[6] = -1;
                    }
                    if (Input.GetKey(Plugin.ConfigControl.Value))
                    {
                        __instance.IOIJFFLMBCH[6] = 1;
                    }
                    if (Input.GetKey(Plugin.ConfigTab.Value))
                    {
                        __instance.IMBKMMOCBBF = 1f;
                    }
                    __instance.IMBKMMOCBBF = __instance.MGJCMCPCPDN;
                    __instance.PNLIFOBMMGG = __instance.JLBOEDNDIPI;
                }
            }
        }
    }
    
}