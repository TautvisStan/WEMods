using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MultiBind
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.MultiBind";
        public const string PluginName = "MultiBind";
        public const string PluginVer = "1.1.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        public static int[] KeyboardInstance = new int[4] { 0, -1, -1, -1 };
        public static bool ControllerMode = false;
        public static int ControllerModeInt = 1;

        public static ConfigEntry<string> ConfigAdd;
        public static ConfigEntry<string> ConfigRemove;
        public static ConfigEntry<string> ConfigControllerMode;

        public class ControlScheme
        {
            public ConfigEntry<string> ConfigUp { get; set; }
            public ConfigEntry<string> ConfigDown { get; set; }
            public ConfigEntry<string> ConfigRight { get; set; }
            public ConfigEntry<string> ConfigLeft { get; set; }

            public ConfigEntry<string> ConfigS { get; set; }
            public ConfigEntry<string> ConfigX { get; set; }
            public ConfigEntry<string> ConfigZ { get; set; }
            public ConfigEntry<string> ConfigA { get; set; }
            public ConfigEntry<string> ConfigSpace { get; set; }
            public ConfigEntry<string> ConfigShift { get; set; }
            public ConfigEntry<string> ConfigControl { get; set; }
            public ConfigEntry<string> ConfigTab { get; set; }
            public ConfigEntry<string> ConfigLeftTrigger { get; set; }
            public ConfigEntry<string> ConfigJoin { get; set; }


        }
        public static ControlScheme[] Player = new ControlScheme[4] { new(), new(), new(), new() };
        public AcceptableValueList<string> KeyboardButtons = new AcceptableValueList<string>("None", "Backspace", "Delete", "Tab", "Clear", "Return", "Pause", "Escape", "Space", "Quote", "Comma", "Minus", "Period", "Slash", "Alpha0", "Alpha1", "Alpha2", "Alpha3", "Alpha4", "Alpha5", "Alpha6", "Alpha7", "Alpha8", "Alpha9", "Semicolon", "Equals", "LeftBracket", "Backslash", "RightBracket", "BackQuote", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Delete", "Keypad0", "Keypad1", "Keypad2", "Keypad3", "Keypad4", "Keypad5", "Keypad6", "Keypad7", "Keypad8", "Keypad9", "KeypadPeriod", "KeypadDivide", "KeypadMultiply", "KeypadMinus", "KeypadPlus", "KeypadEnter", "KeypadEquals", "UpArrow", "DownArrow", "RightArrow", "LeftArrow", "Insert", "Home", "End", "PageUp", "PageDown", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "F13", "F14", "F15", "Numlock", "CapsLock", "ScrollLock", "RightShift", "LeftShift", "RightControl", "LeftControl", "RightAlt", "LeftAlt", "RightCommand", "RightApple", "LeftCommand", "LeftApple", "LeftWindows", "RightWindows", "Print", "Menu");

        private void SetupControls(ControlScheme controlScheme, string player)
        {
            controlScheme.ConfigUp = Config.Bind(player,
             "Up Button (" + player + ")",
             "UpArrow",
             new ConfigDescription(
                "Move up button",
               KeyboardButtons));
            controlScheme.ConfigDown = Config.Bind(player,
             "Down Button (" + player + ")",
             "DownArrow",
             new ConfigDescription(
                    "Move down button",
               KeyboardButtons));
            controlScheme.ConfigLeft = Config.Bind(player,
             "Left Button (" + player + ")",
             "LeftArrow",
             new ConfigDescription(
                    "Move left button",
               KeyboardButtons));
            controlScheme.ConfigRight = Config.Bind(player,
             "Right Button (" + player + ")",
             "RightArrow",
             new ConfigDescription(
                    "Move right button",
               KeyboardButtons));
            controlScheme.ConfigA = Config.Bind(player,
             "Attack Button (" + player + ") / West Button",
             "A",
             new ConfigDescription(
                    "Attack button",
               KeyboardButtons));
            controlScheme.ConfigS = Config.Bind(player,
             "Grapple Button (" + player + ") / North Button",
             "S",
             new ConfigDescription(
                    "Grapple button",
               KeyboardButtons));
            controlScheme.ConfigZ = Config.Bind(player,
             "Run Button (" + player + ") / South Button",
             "Z",
             new ConfigDescription(
                    "Run button",
               KeyboardButtons));
            controlScheme.ConfigX = Config.Bind(player,
             "Pick Up Button (" + player + ") / East Button",
             "X",
             new ConfigDescription(
                    "Pick up button",
               KeyboardButtons));
            controlScheme.ConfigSpace = Config.Bind(player,
             "Taunt Button (" + player + ")",
             "Space",
             new ConfigDescription(
                    "Taunt button",
               KeyboardButtons));
            controlScheme.ConfigShift = Config.Bind(player,
             "Focus (Shift) Button (" + player + ") / Left Shoulder Button",
             "LeftShift",
             new ConfigDescription(
                    "Change focus (shift) button",
               KeyboardButtons));
            controlScheme.ConfigControl = Config.Bind(player,
             "Focus (Control) Button (" + player + ") / Right Shoulder Button",
             "LeftControl",
             new ConfigDescription(
                    "Change focus (control) button",
               KeyboardButtons));
            controlScheme.ConfigTab = Config.Bind(player,
             "Change Control (TAB) Button (" + player + ") / Right Trigger",
             "Tab",
             new ConfigDescription(
                    "Left Trigger Button",
               KeyboardButtons));
            controlScheme.ConfigLeftTrigger = Config.Bind(player,
             "LeftTrigger (" + player + ") / Left Trigger",
             "None",
             new ConfigDescription(
                    "Change the left trigger button",
               KeyboardButtons));
            controlScheme.ConfigJoin = Config.Bind(player,
             "Join Button (" + player + ")",
             "None",
             new ConfigDescription(
                    "\"Join\" button that let's you join a game in progress",
               KeyboardButtons));
        }
        private void Awake()
        {
            Plugin.Log = base.Logger;
            PluginPath = Path.GetDirectoryName(Info.Location);
            ConfigAdd = Config.Bind("General",
             "AddPlayer Button",
             "KeypadPlus",
             new ConfigDescription(
                    "Button to add a new \"fake\" keyboard",
               KeyboardButtons));
            ConfigRemove = Config.Bind("General",
             "RemovePlayer Button",
             "KeypadMinus",
             new ConfigDescription(
                    "Button to remove a new \"fake\" keyboard",
               KeyboardButtons));
            ConfigControllerMode = Config.Bind("General",
             "ControllerMode",
             "KeypadEnter",
             new ConfigDescription(
                    "Will toggle a controller mode so you can use the keyboard in the main ",
               KeyboardButtons));
            SetupControls(Player[0], "Main Keyboard Player");
            SetupControls(Player[1], "Additional Player 1");
            SetupControls(Player[2], "Additional Player 2");
            SetupControls(Player[3], "Additional Player 3");

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
                if (Input.GetKeyDown(Ulil.GetKeyCode(ConfigRemove.Value)))
                {
                    int i;
                    for (i = 1; i <= 3; i++)
                    {
                        if (KeyboardInstance[i] > MFCAJFKKFFE.DANGIDINEGE)
                        {
                            KeyboardInstance[i] = -1;
                        }
                    }
                    for (i = 3; i > 0; i--)
                    {
                        if (KeyboardInstance[i] != -1)
                        {
                            MFCAJFKKFFE.DANGIDINEGE--;
                            KeyboardInstance[i] = -1;
                            break;
                        }
                    }
                }
                if (Input.GetKeyDown(Ulil.GetKeyCode(ConfigAdd.Value)))
                {
                    int i;
                    for (i = 1; i <= 3; i++)
                    {
                        if (KeyboardInstance[i] > MFCAJFKKFFE.DANGIDINEGE)
                        {
                            KeyboardInstance[i] = -1;
                        }
                    }
                    for (i = 1; i <= 3; i++)
                    {
                        if (KeyboardInstance[i] == -1)
                        {
                            int instance = MFCAJFKKFFE.DANGIDINEGE + 1;
                            MFCAJFKKFFE.FBOPLHBCBFI[instance] = new IMBAMKCPLIF();
                            MFCAJFKKFFE.FBOPLHBCBFI[instance].DHBIELODIAN = i;  //playernumber
                            KeyboardInstance[i] = instance;
                            MFCAJFKKFFE.FBOPLHBCBFI[instance].FKPIGOJCEAK = 1;
                            MFCAJFKKFFE.FBOPLHBCBFI[instance].FNIDHNNCLBB = 0;
                            MFCAJFKKFFE.FBOPLHBCBFI[instance].DEIOJMDIMNM = "Virtual";
                            if (LFNJDEGJLLJ.KGCBLLFLIJO <= 1)
                            {
                                MFCAJFKKFFE.FBOPLHBCBFI[instance].FNIDHNNCLBB = ControllerModeInt;
                                MFCAJFKKFFE.FBOPLHBCBFI[instance].DEIOJMDIMNM = "FakeKeyboard";
                            }
                            MFCAJFKKFFE.DANGIDINEGE++;
                            break;
                        }
                    }
                }
                if (Input.GetKeyDown(Ulil.GetKeyCode(ConfigControllerMode.Value)))
                {
                    ControllerMode = !ControllerMode;
                    if (ControllerMode)
                    {
                        ControllerModeInt = 3;
                        BCJPMAHPHEP.FNIDHNNCLBB = 0;
                    }
                    else
                    {
                        ControllerModeInt = 1;
                        BCJPMAHPHEP.FNIDHNNCLBB = 1;
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        if (KeyboardInstance[i] != -1)
                        {
                            MFCAJFKKFFE.FBOPLHBCBFI[KeyboardInstance[i]].FNIDHNNCLBB = ControllerModeInt;
                        }
                    }
                }
            }
        }
        public static void HandleControls(IMBAMKCPLIF __instance, ControlScheme scheme)
        {
            __instance.JOPPDHFINKD = Mathf.MoveTowards(__instance.JOPPDHFINKD, 0f, 1f * BCKHHMIMAEN.PGMACJDMLPP);
            if (JJDCNALMPCI.AAAIDOOHBCM == 50 && __instance.FKPIGOJCEAK > 0 && __instance.FLJEOICJAGC > 0)
            {
                __instance.IIHGPGDGENN++;
            }
            else
            {
                __instance.IIHGPGDGENN = 0;
            }


            __instance.PEPIFGLNPDE = 0f;
            __instance.LOGLAAGLFAH = 0f;
            __instance.HNGCFDLDGBF[1] = 0;
            __instance.HNGCFDLDGBF[2] = 0;
            __instance.HNGCFDLDGBF[3] = 0;
            __instance.HNGCFDLDGBF[4] = 0;
            __instance.HNGCFDLDGBF[5] = 0;
            __instance.HNGCFDLDGBF[6] = 0;
            __instance.HGLNBKOFGDO = 0;
            __instance.HKPDEHMCKIO = 0;
            __instance.LICAPOFIJHI = 0;
            __instance.LLCHNBJDIGO = 0;

            if (Input.GetKey(Ulil.GetKeyCode(scheme.ConfigJoin.Value)))
            {
                __instance.MLCEAOMCNFK(0);
            }
            if (Input.GetKey(Ulil.GetKeyCode(scheme.ConfigUp.Value)))
            {
                __instance.PEPIFGLNPDE = 1f;
            }
            if (Input.GetKey(Ulil.GetKeyCode(scheme.ConfigDown.Value)))
            {
                __instance.PEPIFGLNPDE = -1f;
            }
            if (Input.GetKey(Ulil.GetKeyCode(scheme.ConfigRight.Value)))
            {
                __instance.LOGLAAGLFAH = 1f;
            }
            if (Input.GetKey(Ulil.GetKeyCode(scheme.ConfigLeft.Value)))
            {
                __instance.LOGLAAGLFAH = -1f;
            }
            if (Input.GetKey(Ulil.GetKeyCode(scheme.ConfigS.Value)))  //North
            {
                __instance.HNGCFDLDGBF[1] = 1;
            }
            if (Input.GetKey(Ulil.GetKeyCode(scheme.ConfigX.Value))) //East
            {
                __instance.HNGCFDLDGBF[2] = 1;
            }
            if (Input.GetKey(Ulil.GetKeyCode(scheme.ConfigZ.Value)))  //South
            {
                __instance.HNGCFDLDGBF[3] = 1;
            }
            if (Input.GetKey(Ulil.GetKeyCode(scheme.ConfigA.Value)))  //West
            {
                __instance.HNGCFDLDGBF[4] = 1;
            }
            if (Input.GetKey(Ulil.GetKeyCode(scheme.ConfigSpace.Value)))
            {
                __instance.HNGCFDLDGBF[5] = 1;
            }
            if (Input.GetKey(Ulil.GetKeyCode(scheme.ConfigShift.Value)))  //LShoulder
            {
                __instance.HNGCFDLDGBF[6] = -1;
                __instance.HGLNBKOFGDO = 1;
            }
            if (Input.GetKey(Ulil.GetKeyCode(scheme.ConfigControl.Value))) //RShoulder
            {
                __instance.HNGCFDLDGBF[6] = 1;
                __instance.HKPDEHMCKIO = 1;
            }
            if (Input.GetKey(Ulil.GetKeyCode(scheme.ConfigTab.Value)))  //Right Trigger
            {
                __instance.LICAPOFIJHI = 1f;
            }
            if (Input.GetKey(Ulil.GetKeyCode(scheme.ConfigLeftTrigger.Value)))  //Left Trigger
            {
                __instance.LLCHNBJDIGO = 1f;
            }
            __instance.FHCIFKEGOOH = __instance.LOGLAAGLFAH;
            __instance.PPMGGOHBOLM = __instance.PEPIFGLNPDE;




            if (__instance.LLCHNBJDIGO < 0.5f)
            {
                __instance.LLCHNBJDIGO = 0f;
            }
            if (__instance.LICAPOFIJHI < 0.5f)
            {
                __instance.LICAPOFIJHI = 0f;
            }
            if (__instance.LLCHNBJDIGO >= 1f && __instance.LICAPOFIJHI < 1f)
            {
                __instance.ENAIIPFAJPE = -1f;
            }
            if (__instance.LICAPOFIJHI >= 1f && __instance.LLCHNBJDIGO < 1f)
            {
                __instance.ENAIIPFAJPE = 1f;
            }
            if (__instance.LLCHNBJDIGO >= 1f && __instance.LICAPOFIJHI >= 1f)
            {
                if (__instance.ENAIIPFAJPE == 1f)
                {
                    __instance.ENAIIPFAJPE = -2f;
                }
                if (__instance.ENAIIPFAJPE == -1f)
                {
                    __instance.ENAIIPFAJPE = 2f;
                }
            }
            if (__instance.LLCHNBJDIGO < 1f && __instance.LICAPOFIJHI < 1f)
            {
                __instance.ENAIIPFAJPE = 0f;
            }
            if (LFNJDEGJLLJ.KGCBLLFLIJO == 1 && __instance.DIHFDCNCJFP() != 0f)
            {
                Cursor.visible = false;
            }
            if (__instance.DHBIELODIAN > 0 && __instance.FKPIGOJCEAK > 0 && MFCAJFKKFFE.OMPNDJNOOIF != __instance.DHBIELODIAN)
            {
                __instance.ENEPHALHOAF();
            }
        }
        public static class Ulil
        {
            public static KeyCode GetKeyCode(string name)
            {
                return (KeyCode)System.Enum.Parse(typeof(KeyCode), name, true);
            }
        }
        [HarmonyPatch(typeof(IMBAMKCPLIF))]
        public static class IMBAMKCPLIF_Patch
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(IMBAMKCPLIF.PAOEHLEJKIJ))]
            public static bool PAOEHLEJKIJ_Patch(IMBAMKCPLIF __instance)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (__instance.DHBIELODIAN == Plugin.KeyboardInstance[i] && __instance.FNIDHNNCLBB == ControllerModeInt && __instance.FKPIGOJCEAK > 0)
                    {
                        Plugin.HandleControls(__instance, Plugin.Player[i]);
                        return false;
                    }
                }
                return true;
            }

        }
    }
}