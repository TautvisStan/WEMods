using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

namespace HealthCheats
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.HealthCheats";
        public const string PluginName = "HealthCheats";
        public const string PluginVer = "1.1.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        public static ConfigEntry<string> ConfigTarget;
        public static ConfigEntry<string> ConfigHMax;
        public static ConfigEntry<string> ConfigHMin;
        public static ConfigEntry<string> ConfigSMax;
        public static ConfigEntry<string> ConfigSMin;
        public static ConfigEntry<string> ConfigBMax;
        public static ConfigEntry<string> ConfigBMin;
        public static ConfigEntry<string> ConfigAMax;
        public static ConfigEntry<string> ConfigAMin;
        public static ConfigEntry<string> ConfigAtMax;
        public static ConfigEntry<string> ConfigAtMin;
        public static ConfigEntry<string> ConfigInMax;
        public static ConfigEntry<string> ConfigInMin;


        private void Awake()
        {
            Plugin.Log = base.Logger;
            
            PluginPath = Path.GetDirectoryName(Info.Location);

            ConfigTarget = Config.Bind("Controls",
             "ApplyToTarget",
             "LeftAlt",
             new ConfigDescription(
                "Applies the effect to target instead",
                new AcceptableValueList<string>("None", "Backspace", "Delete", "Tab", "Clear", "Return", "Pause", "Escape", "Space", "Quote", "Comma", "Minus", "Period", "Slash", "Alpha0", "Alpha1", "Alpha2", "Alpha3", "Alpha4", "Alpha5", "Alpha6", "Alpha7", "Alpha8", "Alpha9", "Semicolon", "Equals", "LeftBracket", "Backslash", "RightBracket", "BackQuote", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Delete", "Keypad0", "Keypad1", "Keypad2", "Keypad3", "Keypad4", "Keypad5", "Keypad6", "Keypad7", "Keypad8", "Keypad9", "KeypadPeriod", "KeypadDivide", "KeypadMultiply", "KeypadMinus", "KeypadPlus", "KeypadEnter", "KeypadEquals", "UpArrow", "DownArrow", "RightArrow", "LeftArrow", "Insert", "Home", "End", "PageUp", "PageDown", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "F13", "F14", "F15", "Numlock", "CapsLock", "ScrollLock", "RightShift", "LeftShift", "RightControl", "LeftControl", "RightAlt", "LeftAlt", "RightCommand", "RightApple", "LeftCommand", "LeftApple", "LeftWindows", "RightWindows", "Print", "Menu")));

            ConfigHMax = Config.Bind("Controls",
             "Max Health",
             "Q",
             new ConfigDescription(
                "Sets health to max",
                new AcceptableValueList<string>("None", "Backspace", "Delete", "Tab", "Clear", "Return", "Pause", "Escape", "Space", "Quote", "Comma", "Minus", "Period", "Slash", "Alpha0", "Alpha1", "Alpha2", "Alpha3", "Alpha4", "Alpha5", "Alpha6", "Alpha7", "Alpha8", "Alpha9", "Semicolon", "Equals", "LeftBracket", "Backslash", "RightBracket", "BackQuote", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Delete", "Keypad0", "Keypad1", "Keypad2", "Keypad3", "Keypad4", "Keypad5", "Keypad6", "Keypad7", "Keypad8", "Keypad9", "KeypadPeriod", "KeypadDivide", "KeypadMultiply", "KeypadMinus", "KeypadPlus", "KeypadEnter", "KeypadEquals", "UpArrow", "DownArrow", "RightArrow", "LeftArrow", "Insert", "Home", "End", "PageUp", "PageDown", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "F13", "F14", "F15", "Numlock", "CapsLock", "ScrollLock", "RightShift", "LeftShift", "RightControl", "LeftControl", "RightAlt", "LeftAlt", "RightCommand", "RightApple", "LeftCommand", "LeftApple", "LeftWindows", "RightWindows", "Print", "Menu")));

            ConfigHMin = Config.Bind("Controls",
             "0 Health",
             "W",
             new ConfigDescription(
                "Sets health to 0",
                new AcceptableValueList<string>("None", "Backspace", "Delete", "Tab", "Clear", "Return", "Pause", "Escape", "Space", "Quote", "Comma", "Minus", "Period", "Slash", "Alpha0", "Alpha1", "Alpha2", "Alpha3", "Alpha4", "Alpha5", "Alpha6", "Alpha7", "Alpha8", "Alpha9", "Semicolon", "Equals", "LeftBracket", "Backslash", "RightBracket", "BackQuote", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Delete", "Keypad0", "Keypad1", "Keypad2", "Keypad3", "Keypad4", "Keypad5", "Keypad6", "Keypad7", "Keypad8", "Keypad9", "KeypadPeriod", "KeypadDivide", "KeypadMultiply", "KeypadMinus", "KeypadPlus", "KeypadEnter", "KeypadEquals", "UpArrow", "DownArrow", "RightArrow", "LeftArrow", "Insert", "Home", "End", "PageUp", "PageDown", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "F13", "F14", "F15", "Numlock", "CapsLock", "ScrollLock", "RightShift", "LeftShift", "RightControl", "LeftControl", "RightAlt", "LeftAlt", "RightCommand", "RightApple", "LeftCommand", "LeftApple", "LeftWindows", "RightWindows", "Print", "Menu")));

            ConfigSMax = Config.Bind("Controls",
             "Max Stun",
             "E",
             new ConfigDescription(
                "Sets stun to max",
                new AcceptableValueList<string>("None", "Backspace", "Delete", "Tab", "Clear", "Return", "Pause", "Escape", "Space", "Quote", "Comma", "Minus", "Period", "Slash", "Alpha0", "Alpha1", "Alpha2", "Alpha3", "Alpha4", "Alpha5", "Alpha6", "Alpha7", "Alpha8", "Alpha9", "Semicolon", "Equals", "LeftBracket", "Backslash", "RightBracket", "BackQuote", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Delete", "Keypad0", "Keypad1", "Keypad2", "Keypad3", "Keypad4", "Keypad5", "Keypad6", "Keypad7", "Keypad8", "Keypad9", "KeypadPeriod", "KeypadDivide", "KeypadMultiply", "KeypadMinus", "KeypadPlus", "KeypadEnter", "KeypadEquals", "UpArrow", "DownArrow", "RightArrow", "LeftArrow", "Insert", "Home", "End", "PageUp", "PageDown", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "F13", "F14", "F15", "Numlock", "CapsLock", "ScrollLock", "RightShift", "LeftShift", "RightControl", "LeftControl", "RightAlt", "LeftAlt", "RightCommand", "RightApple", "LeftCommand", "LeftApple", "LeftWindows", "RightWindows", "Print", "Menu")));

            ConfigSMin = Config.Bind("Controls",
             "0 Stun",
             "R",
             new ConfigDescription(
                "Sets stun to 0",
                new AcceptableValueList<string>("None", "Backspace", "Delete", "Tab", "Clear", "Return", "Pause", "Escape", "Space", "Quote", "Comma", "Minus", "Period", "Slash", "Alpha0", "Alpha1", "Alpha2", "Alpha3", "Alpha4", "Alpha5", "Alpha6", "Alpha7", "Alpha8", "Alpha9", "Semicolon", "Equals", "LeftBracket", "Backslash", "RightBracket", "BackQuote", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Delete", "Keypad0", "Keypad1", "Keypad2", "Keypad3", "Keypad4", "Keypad5", "Keypad6", "Keypad7", "Keypad8", "Keypad9", "KeypadPeriod", "KeypadDivide", "KeypadMultiply", "KeypadMinus", "KeypadPlus", "KeypadEnter", "KeypadEquals", "UpArrow", "DownArrow", "RightArrow", "LeftArrow", "Insert", "Home", "End", "PageUp", "PageDown", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "F13", "F14", "F15", "Numlock", "CapsLock", "ScrollLock", "RightShift", "LeftShift", "RightControl", "LeftControl", "RightAlt", "LeftAlt", "RightCommand", "RightApple", "LeftCommand", "LeftApple", "LeftWindows", "RightWindows", "Print", "Menu")));

            ConfigBMax = Config.Bind("Controls",
             "Max Blindness",
             "T",
             new ConfigDescription(
                "Sets \"blindness\" to max",
                new AcceptableValueList<string>("None", "Backspace", "Delete", "Tab", "Clear", "Return", "Pause", "Escape", "Space", "Quote", "Comma", "Minus", "Period", "Slash", "Alpha0", "Alpha1", "Alpha2", "Alpha3", "Alpha4", "Alpha5", "Alpha6", "Alpha7", "Alpha8", "Alpha9", "Semicolon", "Equals", "LeftBracket", "Backslash", "RightBracket", "BackQuote", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Delete", "Keypad0", "Keypad1", "Keypad2", "Keypad3", "Keypad4", "Keypad5", "Keypad6", "Keypad7", "Keypad8", "Keypad9", "KeypadPeriod", "KeypadDivide", "KeypadMultiply", "KeypadMinus", "KeypadPlus", "KeypadEnter", "KeypadEquals", "UpArrow", "DownArrow", "RightArrow", "LeftArrow", "Insert", "Home", "End", "PageUp", "PageDown", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "F13", "F14", "F15", "Numlock", "CapsLock", "ScrollLock", "RightShift", "LeftShift", "RightControl", "LeftControl", "RightAlt", "LeftAlt", "RightCommand", "RightApple", "LeftCommand", "LeftApple", "LeftWindows", "RightWindows", "Print", "Menu")));

            ConfigBMin = Config.Bind("Controls",
             "0 Blindness",
             "Y",
             new ConfigDescription(
                "Sets \"blindness\" to 0",
                new AcceptableValueList<string>("None", "Backspace", "Delete", "Tab", "Clear", "Return", "Pause", "Escape", "Space", "Quote", "Comma", "Minus", "Period", "Slash", "Alpha0", "Alpha1", "Alpha2", "Alpha3", "Alpha4", "Alpha5", "Alpha6", "Alpha7", "Alpha8", "Alpha9", "Semicolon", "Equals", "LeftBracket", "Backslash", "RightBracket", "BackQuote", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Delete", "Keypad0", "Keypad1", "Keypad2", "Keypad3", "Keypad4", "Keypad5", "Keypad6", "Keypad7", "Keypad8", "Keypad9", "KeypadPeriod", "KeypadDivide", "KeypadMultiply", "KeypadMinus", "KeypadPlus", "KeypadEnter", "KeypadEquals", "UpArrow", "DownArrow", "RightArrow", "LeftArrow", "Insert", "Home", "End", "PageUp", "PageDown", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "F13", "F14", "F15", "Numlock", "CapsLock", "ScrollLock", "RightShift", "LeftShift", "RightControl", "LeftControl", "RightAlt", "LeftAlt", "RightCommand", "RightApple", "LeftCommand", "LeftApple", "LeftWindows", "RightWindows", "Print", "Menu")));

            ConfigAMax = Config.Bind("Controls",
             "Max Adrenaline",
             "D",
             new ConfigDescription(
                "Sets adrenaline to max",
                new AcceptableValueList<string>("None", "Backspace", "Delete", "Tab", "Clear", "Return", "Pause", "Escape", "Space", "Quote", "Comma", "Minus", "Period", "Slash", "Alpha0", "Alpha1", "Alpha2", "Alpha3", "Alpha4", "Alpha5", "Alpha6", "Alpha7", "Alpha8", "Alpha9", "Semicolon", "Equals", "LeftBracket", "Backslash", "RightBracket", "BackQuote", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Delete", "Keypad0", "Keypad1", "Keypad2", "Keypad3", "Keypad4", "Keypad5", "Keypad6", "Keypad7", "Keypad8", "Keypad9", "KeypadPeriod", "KeypadDivide", "KeypadMultiply", "KeypadMinus", "KeypadPlus", "KeypadEnter", "KeypadEquals", "UpArrow", "DownArrow", "RightArrow", "LeftArrow", "Insert", "Home", "End", "PageUp", "PageDown", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "F13", "F14", "F15", "Numlock", "CapsLock", "ScrollLock", "RightShift", "LeftShift", "RightControl", "LeftControl", "RightAlt", "LeftAlt", "RightCommand", "RightApple", "LeftCommand", "LeftApple", "LeftWindows", "RightWindows", "Print", "Menu")));

            ConfigAMin = Config.Bind("Controls",
             "0 Adrenaline",
             "F",
             new ConfigDescription(
                "Sets adrenaline to 0",
                new AcceptableValueList<string>("None", "Backspace", "Delete", "Tab", "Clear", "Return", "Pause", "Escape", "Space", "Quote", "Comma", "Minus", "Period", "Slash", "Alpha0", "Alpha1", "Alpha2", "Alpha3", "Alpha4", "Alpha5", "Alpha6", "Alpha7", "Alpha8", "Alpha9", "Semicolon", "Equals", "LeftBracket", "Backslash", "RightBracket", "BackQuote", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Delete", "Keypad0", "Keypad1", "Keypad2", "Keypad3", "Keypad4", "Keypad5", "Keypad6", "Keypad7", "Keypad8", "Keypad9", "KeypadPeriod", "KeypadDivide", "KeypadMultiply", "KeypadMinus", "KeypadPlus", "KeypadEnter", "KeypadEquals", "UpArrow", "DownArrow", "RightArrow", "LeftArrow", "Insert", "Home", "End", "PageUp", "PageDown", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "F13", "F14", "F15", "Numlock", "CapsLock", "ScrollLock", "RightShift", "LeftShift", "RightControl", "LeftControl", "RightAlt", "LeftAlt", "RightCommand", "RightApple", "LeftCommand", "LeftApple", "LeftWindows", "RightWindows", "Print", "Menu")));

            ConfigAtMax = Config.Bind("Controls",
             "Max Adrenaline Timer",
             "G",
             new ConfigDescription(
                "Sets adrenaline timer to max",
                new AcceptableValueList<string>("None", "Backspace", "Delete", "Tab", "Clear", "Return", "Pause", "Escape", "Space", "Quote", "Comma", "Minus", "Period", "Slash", "Alpha0", "Alpha1", "Alpha2", "Alpha3", "Alpha4", "Alpha5", "Alpha6", "Alpha7", "Alpha8", "Alpha9", "Semicolon", "Equals", "LeftBracket", "Backslash", "RightBracket", "BackQuote", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Delete", "Keypad0", "Keypad1", "Keypad2", "Keypad3", "Keypad4", "Keypad5", "Keypad6", "Keypad7", "Keypad8", "Keypad9", "KeypadPeriod", "KeypadDivide", "KeypadMultiply", "KeypadMinus", "KeypadPlus", "KeypadEnter", "KeypadEquals", "UpArrow", "DownArrow", "RightArrow", "LeftArrow", "Insert", "Home", "End", "PageUp", "PageDown", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "F13", "F14", "F15", "Numlock", "CapsLock", "ScrollLock", "RightShift", "LeftShift", "RightControl", "LeftControl", "RightAlt", "LeftAlt", "RightCommand", "RightApple", "LeftCommand", "LeftApple", "LeftWindows", "RightWindows", "Print", "Menu")));

            ConfigAtMin = Config.Bind("Controls",
             "Adrenaline Timer End",
             "H",
             new ConfigDescription(
                "Ends the adrenaline timer",
                new AcceptableValueList<string>("None", "Backspace", "Delete", "Tab", "Clear", "Return", "Pause", "Escape", "Space", "Quote", "Comma", "Minus", "Period", "Slash", "Alpha0", "Alpha1", "Alpha2", "Alpha3", "Alpha4", "Alpha5", "Alpha6", "Alpha7", "Alpha8", "Alpha9", "Semicolon", "Equals", "LeftBracket", "Backslash", "RightBracket", "BackQuote", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Delete", "Keypad0", "Keypad1", "Keypad2", "Keypad3", "Keypad4", "Keypad5", "Keypad6", "Keypad7", "Keypad8", "Keypad9", "KeypadPeriod", "KeypadDivide", "KeypadMultiply", "KeypadMinus", "KeypadPlus", "KeypadEnter", "KeypadEquals", "UpArrow", "DownArrow", "RightArrow", "LeftArrow", "Insert", "Home", "End", "PageUp", "PageDown", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "F13", "F14", "F15", "Numlock", "CapsLock", "ScrollLock", "RightShift", "LeftShift", "RightControl", "LeftControl", "RightAlt", "LeftAlt", "RightCommand", "RightApple", "LeftCommand", "LeftApple", "LeftWindows", "RightWindows", "Print", "Menu")));

            ConfigInMax = Config.Bind("Controls",
             "Injure the character",
             "J",
             new ConfigDescription(
                "Injures the character",
                new AcceptableValueList<string>("None", "Backspace", "Delete", "Tab", "Clear", "Return", "Pause", "Escape", "Space", "Quote", "Comma", "Minus", "Period", "Slash", "Alpha0", "Alpha1", "Alpha2", "Alpha3", "Alpha4", "Alpha5", "Alpha6", "Alpha7", "Alpha8", "Alpha9", "Semicolon", "Equals", "LeftBracket", "Backslash", "RightBracket", "BackQuote", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Delete", "Keypad0", "Keypad1", "Keypad2", "Keypad3", "Keypad4", "Keypad5", "Keypad6", "Keypad7", "Keypad8", "Keypad9", "KeypadPeriod", "KeypadDivide", "KeypadMultiply", "KeypadMinus", "KeypadPlus", "KeypadEnter", "KeypadEquals", "UpArrow", "DownArrow", "RightArrow", "LeftArrow", "Insert", "Home", "End", "PageUp", "PageDown", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "F13", "F14", "F15", "Numlock", "CapsLock", "ScrollLock", "RightShift", "LeftShift", "RightControl", "LeftControl", "RightAlt", "LeftAlt", "RightCommand", "RightApple", "LeftCommand", "LeftApple", "LeftWindows", "RightWindows", "Print", "Menu")));

            ConfigInMin = Config.Bind("Controls",
             "Heal the injury",
             "K",
             new ConfigDescription(
                "Removes the character injury",
                new AcceptableValueList<string>("None", "Backspace", "Delete", "Tab", "Clear", "Return", "Pause", "Escape", "Space", "Quote", "Comma", "Minus", "Period", "Slash", "Alpha0", "Alpha1", "Alpha2", "Alpha3", "Alpha4", "Alpha5", "Alpha6", "Alpha7", "Alpha8", "Alpha9", "Semicolon", "Equals", "LeftBracket", "Backslash", "RightBracket", "BackQuote", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Delete", "Keypad0", "Keypad1", "Keypad2", "Keypad3", "Keypad4", "Keypad5", "Keypad6", "Keypad7", "Keypad8", "Keypad9", "KeypadPeriod", "KeypadDivide", "KeypadMultiply", "KeypadMinus", "KeypadPlus", "KeypadEnter", "KeypadEquals", "UpArrow", "DownArrow", "RightArrow", "LeftArrow", "Insert", "Home", "End", "PageUp", "PageDown", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "F13", "F14", "F15", "Numlock", "CapsLock", "ScrollLock", "RightShift", "LeftShift", "RightControl", "LeftControl", "RightAlt", "LeftAlt", "RightCommand", "RightApple", "LeftCommand", "LeftApple", "LeftWindows", "RightWindows", "Print", "Menu")));



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
            try
            {
                if (Input.GetKey(Ulil.GetKeyCode(ConfigTarget.Value))) //target
                {
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigHMax.Value)))  //health max
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].PDMDFGNJCPN].LPEIDPCNMGK = int.MaxValue;
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].PDMDFGNJCPN].JAENLEMFEIC = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigHMin.Value))) //health 0
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].PDMDFGNJCPN].LPEIDPCNMGK = 0;
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].PDMDFGNJCPN].JAENLEMFEIC = 0;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigSMax.Value))) //stun max
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].PDMDFGNJCPN].CIKIJABJGOL = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigSMin.Value))) //stun 0
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].PDMDFGNJCPN].CIKIJABJGOL = 0;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigBMax.Value))) //blindness max
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].PDMDFGNJCPN].DEGCIAJNOMC = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigBMin.Value))) //blindness 0
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].PDMDFGNJCPN].DEGCIAJNOMC = 0;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigAMax.Value))) //adrenaline max
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].PDMDFGNJCPN].AEICBDHMALN = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigAMin.Value))) //adrenaline 0
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].PDMDFGNJCPN].AEICBDHMALN = 0;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigAtMax.Value))) //adr timer max
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].PDMDFGNJCPN].LPJAPJBIIFF = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigAtMin.Value))) //adr timer 0
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].PDMDFGNJCPN].LPJAPJBIIFF = 1;
                    }
                    if (Input.GetKeyDown(Ulil.GetKeyCode(ConfigInMax.Value))) //injure
                    {
                        Injure(AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].PDMDFGNJCPN]);
                    }
                    if (Input.GetKeyDown(Ulil.GetKeyCode(ConfigInMin.Value))) //heal
                    {
                        Heal(AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].PDMDFGNJCPN]);
                    }

                }
                else //player
                {
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigHMax.Value))) //health max
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].LPEIDPCNMGK = int.MaxValue;
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].JAENLEMFEIC = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigHMin.Value))) //health 0
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].LPEIDPCNMGK = 0;
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].JAENLEMFEIC = 0;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigSMax.Value))) //stun max
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].CIKIJABJGOL = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigSMin.Value))) //stun 0
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].CIKIJABJGOL = 0;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigBMax.Value))) //blindness max
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].DEGCIAJNOMC = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigBMin.Value))) //blindness 0
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].DEGCIAJNOMC = 0;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigAMax.Value))) //adrenaline max
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].AEICBDHMALN = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigAMin.Value))) //adrenaline 0
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].AEICBDHMALN = 0;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigAtMax.Value))) //adr timer max
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].LPJAPJBIIFF = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigAtMin.Value))) //adr timer 0
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].LPJAPJBIIFF = 1;
                    }
                    if (Input.GetKeyDown(Ulil.GetKeyCode(ConfigInMax.Value))) //injure
                    {
                        Injure(AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN]);
                    }
                    if (Input.GetKeyDown(Ulil.GetKeyCode(ConfigInMin.Value))) //heal
                    {
                        Heal(AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN]);
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
        public static void Injure(GMIKIMHFABP instance)
        {
            //GMIKIMHFABP instance = AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[2].PDMDFGNJCPN];
            instance.PKKAFJHMKDE = UnityEngine.Random.Range(1, 31);
           // instance.IPNKFGHIDJP.injury = instance.PKKAFJHMKDE;
            if (ONACPDNNNMM.JJAJNKLJHCL == 0 && instance.IPNKFGHIDJP.injuryTime < 2)
            {
                instance.IPNKFGHIDJP.injuryTime = 2;
            }
            JKPIHABGBGP.BDFEDMELBOL(instance.BJANPGIFKHD, JKPIHABGBGP.PHDFFBOFBKI[JGKBBDPDIBC.OCMIPAODMHH(1, 2, 0)], -0.1f, 1f);
            instance.CACNKHNHAAL(JGKBBDPDIBC.OCMIPAODMHH(2, 3, 0), 1f);
            if (ONACPDNNNMM.IDNGKPOBMFO == 16)
            {
                JKPIHABGBGP.HPICKPAJOKP(0, JGKBBDPDIBC.JOGCPNBFFNJ(3, 20), 1f);
                ONACPDNNNMM.EFEKHAFNJDM *= 0.8f;
            }
            else
            {
                JKPIHABGBGP.HPICKPAJOKP(0, -1, 1f);
                JKPIHABGBGP.HPICKPAJOKP(0, JGKBBDPDIBC.OCMIPAODMHH(16, 20, 0), 0f);
                ONACPDNNNMM.EFEKHAFNJDM += instance.MGHOMFFCEGK[1] * (instance.MGHOMFFCEGK[1] / 10f) * instance.FLGHPCBAJFK(0);
                if (ONACPDNNNMM.BAGBHKFPKGP == 16)
                {
                    ONACPDNNNMM.EFEKHAFNJDM += instance.MGHOMFFCEGK[1] * (instance.MGHOMFFCEGK[1] / 10f) * instance.FLGHPCBAJFK(0);
                }
            }
            ONACPDNNNMM.DIAADODHFDC += 250f * instance.FLGHPCBAJFK(0);
            instance.LPEIDPCNMGK /= 2f;
            instance.AEICBDHMALN /= 2f;
            if (ONACPDNNNMM.JJAJNKLJHCL != 0)
            {
                for (int i = 1; i <= AMJONEKIAID.MOGFLKPBLIK; i++)
                {
                    if (AMJONEKIAID.NCPIJJFEDFL[i].MMDNKLMAOEF == 3 || AMJONEKIAID.NCPIJJFEDFL[i].MMDNKLMAOEF == 0)
                    {
                        AMJONEKIAID.NCPIJJFEDFL[i].OEBCDJGHNEH = 48;
                    }
                }
            }
            Characters.DPJLHKNBEKO(AMJONEKIAID.NCPIJJFEDFL[instance.EAGPOMCFONI].NLMPKIAICOP, instance.NLMPKIAICOP);
            AMJONEKIAID.OGANAMIKGDJ(instance.EAGPOMCFONI, instance.NMKACNOOPPC, 1);
        }
        public static void Heal(GMIKIMHFABP instance)
        {
            instance.PKKAFJHMKDE = 0;
            instance.IPNKFGHIDJP.injuryTime = 0;
            instance.IPNKFGHIDJP.injury = 0;
        }
    }
        public static class Ulil
        {
            public static KeyCode GetKeyCode(string name)
            {
                return (KeyCode)System.Enum.Parse(typeof(KeyCode), name, true);
            }
        }
}
//target                    AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].PDMDFGNJCPN].
//self                     AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].