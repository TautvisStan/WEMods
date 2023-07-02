using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KeyboardSharing
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.KeyboardSharing";
        public const string PluginName = "KeyboardSharing";
        public const string PluginVer = "1.0.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;


        public static int KeyboardInstance1 = -1;
        public static int KeyboardInstance2 = -1;
        public static int KeyboardInstance3 = -1;

        public static ConfigEntry<KeyCode> ConfigAdd;
        public static ConfigEntry<KeyCode> ConfigRemove;
        public static ConfigEntry<KeyCode> ConfigUp1;
        public static ConfigEntry<KeyCode> ConfigDown1;
        public static ConfigEntry<KeyCode> ConfigRight1;
        public static ConfigEntry<KeyCode> ConfigLeft1;

        public static ConfigEntry<KeyCode> ConfigS1;
        public static ConfigEntry<KeyCode> ConfigX1;
        public static ConfigEntry<KeyCode> ConfigZ1;
        public static ConfigEntry<KeyCode> ConfigA1;
        public static ConfigEntry<KeyCode> ConfigSpace1;
        public static ConfigEntry<KeyCode> ConfigShift1;
        public static ConfigEntry<KeyCode> ConfigControl1;
        public static ConfigEntry<KeyCode> ConfigTab1;
        public static ConfigEntry<KeyCode> ConfigJoin1;

        public static ConfigEntry<KeyCode> ConfigUp2;
        public static ConfigEntry<KeyCode> ConfigDown2;
        public static ConfigEntry<KeyCode> ConfigRight2;
        public static ConfigEntry<KeyCode> ConfigLeft2;

        public static ConfigEntry<KeyCode> ConfigS2;
        public static ConfigEntry<KeyCode> ConfigX2;
        public static ConfigEntry<KeyCode> ConfigZ2;
        public static ConfigEntry<KeyCode> ConfigA2;
        public static ConfigEntry<KeyCode> ConfigSpace2;
        public static ConfigEntry<KeyCode> ConfigShift2;
        public static ConfigEntry<KeyCode> ConfigControl2;
        public static ConfigEntry<KeyCode> ConfigTab2;
        public static ConfigEntry<KeyCode> ConfigJoin2;

        public static ConfigEntry<KeyCode> ConfigUp3;
        public static ConfigEntry<KeyCode> ConfigDown3;
        public static ConfigEntry<KeyCode> ConfigRight3;
        public static ConfigEntry<KeyCode> ConfigLeft3;

        public static ConfigEntry<KeyCode> ConfigS3;
        public static ConfigEntry<KeyCode> ConfigX3;
        public static ConfigEntry<KeyCode> ConfigZ3;
        public static ConfigEntry<KeyCode> ConfigA3;
        public static ConfigEntry<KeyCode> ConfigSpace3;
        public static ConfigEntry<KeyCode> ConfigShift3;
        public static ConfigEntry<KeyCode> ConfigControl3;
        public static ConfigEntry<KeyCode> ConfigTab3;
        public static ConfigEntry<KeyCode> ConfigJoin3;

        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);
            ConfigAdd = Config.Bind("General",
             "AddPlayer Button",
             KeyCode.KeypadPlus,
             "Button to add a new \"fake\" keyboard");
            ConfigRemove = Config.Bind("General",
             "RemovePlayer Button",
             KeyCode.KeypadMinus,
             "Button to remove a new \"fake\" keyboard");

            ConfigUp1 = Config.Bind("Additional player 1",
             "Up Button (additional player 1)",
             KeyCode.UpArrow,
             "Move up button");
            ConfigDown1 = Config.Bind("Additional player 1",
             "Down Button (additional player 1)",
             KeyCode.DownArrow,
             "Move down button");
            ConfigLeft1 = Config.Bind("Additional player 1",
             "Left Button (additional player 1)",
             KeyCode.LeftArrow,
             "Move left button");
            ConfigRight1 = Config.Bind("Additional player 1",
             "Right Button (additional player 1)",
             KeyCode.RightArrow,
             "Move right button");
            ConfigA1 = Config.Bind("Additional player 1",
             "Attack Button (additional player 1)",
             KeyCode.A,
             "Attack button");
            ConfigS1 = Config.Bind("Additional player 1",
             "Grapple Button (additional player 1)",
             KeyCode.S,
             "Grapple button");
            ConfigZ1 = Config.Bind("Additional player 1",
             "Run Button (additional player 1)",
             KeyCode.Z,
             "Run button");
            ConfigX1 = Config.Bind("Additional player 1",
             "Pick Up Button (additional player 1)",
             KeyCode.X,
             "Pick up button");
            ConfigSpace1 = Config.Bind("Additional player 1",
             "Taunt Button (additional player 1)",
             KeyCode.Space,
             "Taunt button");
            ConfigShift1 = Config.Bind("Additional player 1",
             "Focus (Shift) Button (additional player 1)",
             KeyCode.LeftShift,
             "Change focus (shift) button");
            ConfigControl1 = Config.Bind("Additional player 1",
             "Focus (Control) Button (additional player 1)",
             KeyCode.LeftControl,
             "Change focus (control) button");
            ConfigTab1 = Config.Bind("Additional player 1",
             "Change Control (TAB) Button (additional player 1)",
             KeyCode.Tab,
             "Change control (TAB) button");
            ConfigJoin1 = Config.Bind("Additional player 1",
             "Join Button (additional player 1)",
             KeyCode.None,
             "\"Join\" button that let's you join a game in progress");

            ConfigUp2 = Config.Bind("Additional player 2",
             "Up Button (additional player 2)",
             KeyCode.UpArrow,
             "Move up button");
            ConfigDown2 = Config.Bind("Additional player 2",
             "Down Button (additional player 2)",
             KeyCode.DownArrow,
             "Move down button");
            ConfigLeft2 = Config.Bind("Additional player 2",
             "Left Button (additional player 2)",
             KeyCode.LeftArrow,
             "Move left button");
            ConfigRight2 = Config.Bind("Additional player 2",
             "Right Button (additional player 2)",
             KeyCode.RightArrow,
             "Move right button");
            ConfigA2 = Config.Bind("Additional player 2",
             "Attack Button (additional player 2)",
             KeyCode.A,
             "Attack button");
            ConfigS2 = Config.Bind("Additional player 2",
             "Grapple Button (additional player 2)",
             KeyCode.S,
             "Grapple button");
            ConfigZ2 = Config.Bind("Additional player 2",
             "Run Button (additional player 2)",
             KeyCode.Z,
             "Run button");
            ConfigX2 = Config.Bind("Additional player 2",
             "Pick Up Button (additional player 2)",
             KeyCode.X,
             "Pick up button");
            ConfigSpace2 = Config.Bind("Additional player 2",
             "Taunt Button (additional player 2)",
             KeyCode.Space,
             "Taunt button");
            ConfigShift2 = Config.Bind("Additional player 2",
             "Focus (Shift) Button (additional player 2)",
             KeyCode.LeftShift,
             "Change focus (shift) button");
            ConfigControl2 = Config.Bind("Additional player 2",
             "Focus (Control) Button (additional player 2)",
             KeyCode.LeftControl,
             "Change focus (control) button");
            ConfigTab2 = Config.Bind("Additional player 2",
             "Change Control (TAB) Button (additional player 2)",
             KeyCode.Tab,
             "Change control (TAB) button");
            ConfigJoin2 = Config.Bind("Additional player 2",
             "Join Button (additional player 2)",
             KeyCode.None,
             "\"Join\" button that let's you join a game in progress");

            ConfigUp3 = Config.Bind("Additional player 3",
             "Up Button (additional player 3)",
             KeyCode.UpArrow,
             "Move up button");
            ConfigDown3 = Config.Bind("Additional player 3",
             "Down Button (additional player 3)",
             KeyCode.DownArrow,
             "Move down button");
            ConfigLeft3 = Config.Bind("Additional player 3",
             "Left Button (additional player 3)",
             KeyCode.LeftArrow,
             "Move left button");
            ConfigRight3 = Config.Bind("Additional player 3",
             "Right Button (additional player 3)",
             KeyCode.RightArrow,
             "Move right button");
            ConfigA3 = Config.Bind("Additional player 3",
             "Attack Button (additional player 3)",
             KeyCode.A,
             "Attack button");
            ConfigS3 = Config.Bind("Additional player 3",
             "Grapple Button (additional player 3)",
             KeyCode.S,
             "Grapple button");
            ConfigZ3 = Config.Bind("Additional player 3",
             "Run Button (additional player 3)",
             KeyCode.Z,
             "Run button");
            ConfigX3 = Config.Bind("Additional player 3",
             "Pick Up Button (additional player 3)",
             KeyCode.X,
             "Pick up button");
            ConfigSpace3 = Config.Bind("Additional player 3",
             "Taunt Button (additional player 3)",
             KeyCode.Space,
             "Taunt button");
            ConfigShift3 = Config.Bind("Additional player 3",
             "Focus (Shift) Button (additional player 3)",
             KeyCode.LeftShift,
             "Change focus (shift) button");
            ConfigControl3 = Config.Bind("Additional player 3",
             "Focus (Control) Button (additional player 3)",
             KeyCode.LeftControl,
             "Change focus (control) button");
            ConfigTab3 = Config.Bind("Additional player 3",
             "Change Control (TAB) Button (additional player 3)",
             KeyCode.Tab,
             "Change control (TAB) button");
            ConfigJoin3 = Config.Bind("Additional player 3",
             "Join Button (additional player 3)",
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
        private void Update()
        {
            if (SceneManager.GetActiveScene().name == "Titles")
            {
                if (Input.GetKeyDown(ConfigRemove.Value))
                {
                    if (KeyboardInstance1 > IODAJCMGILB.LGCFNINPKIJ)
                    {
                        KeyboardInstance1 = -1;
                    }
                    if (KeyboardInstance2 > IODAJCMGILB.LGCFNINPKIJ)
                    {
                        KeyboardInstance2 = -1;
                    }
                    if (KeyboardInstance3 > IODAJCMGILB.LGCFNINPKIJ)
                    {
                        KeyboardInstance3 = -1;
                    }
                    bool removed = false;
                    if (KeyboardInstance3 != -1)
                    {
                        IODAJCMGILB.LGCFNINPKIJ--;
                        KeyboardInstance3 = -1;
                        removed = true;
                    }
                    if (KeyboardInstance2 != -1 && !removed)
                    {
                        IODAJCMGILB.LGCFNINPKIJ--;
                        KeyboardInstance2 = -1;
                        removed = true;
                    }
                    if (KeyboardInstance1 != -1 && !removed)
                    {
                        IODAJCMGILB.LGCFNINPKIJ--;
                        KeyboardInstance1 = -1;
                        removed = true;
                    }
                }
                if (Input.GetKeyDown(ConfigAdd.Value))
                {
                    bool added = false;
                    if (KeyboardInstance1 > IODAJCMGILB.LGCFNINPKIJ)
                    {
                        KeyboardInstance1 = -1;
                    }
                    if (KeyboardInstance2 > IODAJCMGILB.LGCFNINPKIJ)
                    {
                        KeyboardInstance2 = -1;
                    }
                    if (KeyboardInstance3 > IODAJCMGILB.LGCFNINPKIJ)
                    {
                        KeyboardInstance3 = -1;
                    }
                    if (KeyboardInstance1 == -1)
                    {
                            int i = IODAJCMGILB.LGCFNINPKIJ + 1;
                            IODAJCMGILB.LEGPGICAAJI[i] = new KDOHFMKNHOB();
                            IODAJCMGILB.LEGPGICAAJI[i].NMKACNOOPPC = 1;  //playernumber
                            KeyboardInstance1 = i;
                            IODAJCMGILB.LEGPGICAAJI[i].FLFDNLEILGC = 1;
                            IODAJCMGILB.LEGPGICAAJI[i].AHPNDLJNCFK = 0;
                            IODAJCMGILB.LEGPGICAAJI[i].FLAJNJNPDCB = "Virtual";
                            if (JGKBBDPDIBC.GICLAJCKKBK <= 1)
                            {
                                IODAJCMGILB.LEGPGICAAJI[i].AHPNDLJNCFK = 1;
                                IODAJCMGILB.LEGPGICAAJI[i].FLAJNJNPDCB = "FakeKeyboard";
                            }
                        IODAJCMGILB.LGCFNINPKIJ++;
                        added = true;
                    }
                    if (KeyboardInstance2 == -1 && !added)
                    {
                            int i = IODAJCMGILB.LGCFNINPKIJ + 1;
                            IODAJCMGILB.LEGPGICAAJI[i] = new KDOHFMKNHOB();
                            IODAJCMGILB.LEGPGICAAJI[i].NMKACNOOPPC = 2;  //playernumber
                            KeyboardInstance2 = i;
                            IODAJCMGILB.LEGPGICAAJI[i].FLFDNLEILGC = 1;
                            IODAJCMGILB.LEGPGICAAJI[i].AHPNDLJNCFK = 0;
                            IODAJCMGILB.LEGPGICAAJI[i].FLAJNJNPDCB = "Virtual";
                            if (JGKBBDPDIBC.GICLAJCKKBK <= 1)
                            {
                                IODAJCMGILB.LEGPGICAAJI[i].AHPNDLJNCFK = 1;
                                IODAJCMGILB.LEGPGICAAJI[i].FLAJNJNPDCB = "FakeKeyboard";
                            }
                        IODAJCMGILB.LGCFNINPKIJ++;
                        added = true;
                    }
                    if (KeyboardInstance3 == -1 && !added)
                    {
                            int i = IODAJCMGILB.LGCFNINPKIJ + 1;
                            IODAJCMGILB.LEGPGICAAJI[i] = new KDOHFMKNHOB();
                            IODAJCMGILB.LEGPGICAAJI[i].NMKACNOOPPC = 3;  //playernumber
                            KeyboardInstance3 = i;
                            IODAJCMGILB.LEGPGICAAJI[i].FLFDNLEILGC = 1;
                            IODAJCMGILB.LEGPGICAAJI[i].AHPNDLJNCFK = 0;
                            IODAJCMGILB.LEGPGICAAJI[i].FLAJNJNPDCB = "Virtual";
                            if (JGKBBDPDIBC.GICLAJCKKBK <= 1)
                            {
                                IODAJCMGILB.LEGPGICAAJI[i].AHPNDLJNCFK = 1;
                                IODAJCMGILB.LEGPGICAAJI[i].FLAJNJNPDCB = "FakeKeyboard";
                            }
                        IODAJCMGILB.LGCFNINPKIJ++;
                        added = true;
                    }
                }
            }
        }
    }
    [HarmonyPatch(typeof(KDOHFMKNHOB))]
    public static class KDOHFMKNHOB_Patch
    {
        [HarmonyPostfix]
        [HarmonyPatch("GCGDPDLEHPH")]
        public static void GCGDPDLEHPH_Patch(KDOHFMKNHOB __instance)
        { 
            if (__instance.NMKACNOOPPC == Plugin.KeyboardInstance1 && __instance.AHPNDLJNCFK == 1 && __instance.FLFDNLEILGC > 0) //P2
            {
                __instance.NEBLDBJGDAI = 0f;
                __instance.KHMFMDHIDPH = 0f;
                __instance.HFEHAADPDHP[1] = 0;
                __instance.HFEHAADPDHP[2] = 0;
                __instance.HFEHAADPDHP[3] = 0;
                __instance.HFEHAADPDHP[4] = 0;
                __instance.HFEHAADPDHP[5] = 0;
                __instance.HFEHAADPDHP[6] = 0;
                __instance.NLCCPJBCLHD = 0;
                if (Input.GetKey(Plugin.ConfigJoin1.Value))
                {
                    __instance.BBEPHLPOHNL(0);
                }
                if (Input.GetKey(Plugin.ConfigUp1.Value))
                    {
                        __instance.NEBLDBJGDAI = 1f;
                    }
                    if (Input.GetKey(Plugin.ConfigDown1.Value))
                    {
                        __instance.NEBLDBJGDAI = -1f;
                    }
                    if (Input.GetKey(Plugin.ConfigRight1.Value))
                    {
                        __instance.KHMFMDHIDPH = 1f;
                    }
                    if (Input.GetKey(Plugin.ConfigLeft1.Value))
                    {
                        __instance.KHMFMDHIDPH = -1f;
                    }
                    if (Input.GetKey(Plugin.ConfigS1.Value))
                    {
                        __instance.HFEHAADPDHP[1] = 1;
                    }
                    if (Input.GetKey(Plugin.ConfigX1.Value))
                    {
                        __instance.HFEHAADPDHP[2] = 1;
                    }
                    if (Input.GetKey(Plugin.ConfigZ1.Value))
                    {
                        __instance.HFEHAADPDHP[3] = 1;
                    }
                    if (Input.GetKey(Plugin.ConfigA1.Value))
                    {
                        __instance.HFEHAADPDHP[4] = 1;
                    }
                    if (Input.GetKey(Plugin.ConfigSpace1.Value))
                    {
                        __instance.HFEHAADPDHP[5] = 1;
                    }
                    if (Input.GetKey(Plugin.ConfigShift1.Value))
                    {
                        __instance.HFEHAADPDHP[6] = -1;
                    }
                    if (Input.GetKey(Plugin.ConfigControl1.Value))
                    {
                        __instance.HFEHAADPDHP[6] = 1;
                    }
                    if (Input.GetKey(Plugin.ConfigTab1.Value))
                    {
                        __instance.NLCCPJBCLHD = 1f;
                    }
                    __instance.IKGCLCIAOOL = __instance.KHMFMDHIDPH;
                    __instance.ONCJKLOEGEF = __instance.NEBLDBJGDAI;
            }
            if (__instance.NMKACNOOPPC == Plugin.KeyboardInstance2 && __instance.AHPNDLJNCFK == 1 && __instance.FLFDNLEILGC > 0) //P3
            {
                __instance.NEBLDBJGDAI = 0f;
                __instance.KHMFMDHIDPH = 0f;
                __instance.HFEHAADPDHP[1] = 0;
                __instance.HFEHAADPDHP[2] = 0;
                __instance.HFEHAADPDHP[3] = 0;
                __instance.HFEHAADPDHP[4] = 0;
                __instance.HFEHAADPDHP[5] = 0;
                __instance.HFEHAADPDHP[6] = 0;
                __instance.NLCCPJBCLHD = 0;

                if (Input.GetKey(Plugin.ConfigJoin2.Value))
                {
                    __instance.BBEPHLPOHNL(0);
                }
                if (Input.GetKey(Plugin.ConfigUp2.Value))
                {
                    __instance.NEBLDBJGDAI = 1f;
                }
                if (Input.GetKey(Plugin.ConfigDown2.Value))
                {
                    __instance.NEBLDBJGDAI = -1f;
                }
                if (Input.GetKey(Plugin.ConfigRight2.Value))
                {
                    __instance.KHMFMDHIDPH = 1f;
                }
                if (Input.GetKey(Plugin.ConfigLeft2.Value))
                {
                    __instance.KHMFMDHIDPH = -1f;
                }
                if (Input.GetKey(Plugin.ConfigS2.Value))
                {
                    __instance.HFEHAADPDHP[1] = 1;
                }
                if (Input.GetKey(Plugin.ConfigX2.Value))
                {
                    __instance.HFEHAADPDHP[2] = 1;
                }
                if (Input.GetKey(Plugin.ConfigZ2.Value))
                {
                    __instance.HFEHAADPDHP[3] = 1;
                }
                if (Input.GetKey(Plugin.ConfigA2.Value))
                {
                    __instance.HFEHAADPDHP[4] = 1;
                }
                if (Input.GetKey(Plugin.ConfigSpace2.Value))
                {
                    __instance.HFEHAADPDHP[5] = 1;
                }
                if (Input.GetKey(Plugin.ConfigShift2.Value))
                {
                    __instance.HFEHAADPDHP[6] = -1;
                }
                if (Input.GetKey(Plugin.ConfigControl2.Value))
                {
                    __instance.HFEHAADPDHP[6] = 1;
                }
                if (Input.GetKey(Plugin.ConfigTab2.Value))
                {
                    __instance.NLCCPJBCLHD = 1f;
                }
                __instance.IKGCLCIAOOL = __instance.KHMFMDHIDPH;
                __instance.ONCJKLOEGEF = __instance.NEBLDBJGDAI;
            }
            if (__instance.NMKACNOOPPC == Plugin.KeyboardInstance3 && __instance.AHPNDLJNCFK == 1 && __instance.FLFDNLEILGC > 0) //P3
            {
                __instance.NEBLDBJGDAI = 0f;
                __instance.KHMFMDHIDPH = 0f;
                __instance.HFEHAADPDHP[1] = 0;
                __instance.HFEHAADPDHP[2] = 0;
                __instance.HFEHAADPDHP[3] = 0;
                __instance.HFEHAADPDHP[4] = 0;
                __instance.HFEHAADPDHP[5] = 0;
                __instance.HFEHAADPDHP[6] = 0;
                __instance.NLCCPJBCLHD = 0;
                if (Input.GetKey(Plugin.ConfigJoin3.Value))
                {
                    __instance.BBEPHLPOHNL(0);
                }
                if (Input.GetKey(Plugin.ConfigUp3.Value))
                {
                    __instance.NEBLDBJGDAI = 1f;
                }
                if (Input.GetKey(Plugin.ConfigDown3.Value))
                {
                    __instance.NEBLDBJGDAI = -1f;
                }
                if (Input.GetKey(Plugin.ConfigRight3.Value))
                {
                    __instance.KHMFMDHIDPH = 1f;
                }
                if (Input.GetKey(Plugin.ConfigLeft3.Value))
                {
                    __instance.KHMFMDHIDPH = -1f;
                }
                if (Input.GetKey(Plugin.ConfigS3.Value))
                {
                    __instance.HFEHAADPDHP[1] = 1;
                }
                if (Input.GetKey(Plugin.ConfigX3.Value))
                {
                    __instance.HFEHAADPDHP[2] = 1;
                }
                if (Input.GetKey(Plugin.ConfigZ3.Value))
                {
                    __instance.HFEHAADPDHP[3] = 1;
                }
                if (Input.GetKey(Plugin.ConfigA3.Value))
                {
                    __instance.HFEHAADPDHP[4] = 1;
                }
                if (Input.GetKey(Plugin.ConfigSpace3.Value))
                {
                    __instance.HFEHAADPDHP[5] = 1;
                }
                if (Input.GetKey(Plugin.ConfigShift3.Value))
                {
                    __instance.HFEHAADPDHP[6] = -1;
                }
                if (Input.GetKey(Plugin.ConfigControl3.Value))
                {
                    __instance.HFEHAADPDHP[6] = 1;
                }
                if (Input.GetKey(Plugin.ConfigTab3.Value))
                {
                    __instance.NLCCPJBCLHD = 1f;
                }
                __instance.IKGCLCIAOOL = __instance.KHMFMDHIDPH;
                __instance.ONCJKLOEGEF = __instance.NEBLDBJGDAI;
            }
        }
    }
}