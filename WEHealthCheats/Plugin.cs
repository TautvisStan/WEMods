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
        public const string PluginVer = "1.2.1";

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
        public static ConfigEntry<string> ConfigKOMax;
        public static ConfigEntry<string> ConfigKOMin;

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

            ConfigKOMax = Config.Bind("Controls",
             "Knock out a target",
             "C",
             new ConfigDescription(
                "Instantly knock out (and DQ) a target",
                new AcceptableValueList<string>("None", "Backspace", "Delete", "Tab", "Clear", "Return", "Pause", "Escape", "Space", "Quote", "Comma", "Minus", "Period", "Slash", "Alpha0", "Alpha1", "Alpha2", "Alpha3", "Alpha4", "Alpha5", "Alpha6", "Alpha7", "Alpha8", "Alpha9", "Semicolon", "Equals", "LeftBracket", "Backslash", "RightBracket", "BackQuote", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Delete", "Keypad0", "Keypad1", "Keypad2", "Keypad3", "Keypad4", "Keypad5", "Keypad6", "Keypad7", "Keypad8", "Keypad9", "KeypadPeriod", "KeypadDivide", "KeypadMultiply", "KeypadMinus", "KeypadPlus", "KeypadEnter", "KeypadEquals", "UpArrow", "DownArrow", "RightArrow", "LeftArrow", "Insert", "Home", "End", "PageUp", "PageDown", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "F13", "F14", "F15", "Numlock", "CapsLock", "ScrollLock", "RightShift", "LeftShift", "RightControl", "LeftControl", "RightAlt", "LeftAlt", "RightCommand", "RightApple", "LeftCommand", "LeftApple", "LeftWindows", "RightWindows", "Print", "Menu")));

            ConfigKOMin = Config.Bind("Controls",
             "Recover target from knockout",
             "V",
             new ConfigDescription(
                "Recover target from knockout",
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
                        NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].NNMDEFLLNBF].HLGALFAGDGC = int.MaxValue;
                        NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].NNMDEFLLNBF].OIHGGHEDIFF = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigHMin.Value))) //health 0
                    {
                        NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].NNMDEFLLNBF].HLGALFAGDGC = 0;
                        NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].NNMDEFLLNBF].OIHGGHEDIFF = 0;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigSMax.Value))) //stun max
                    {
                        NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].NNMDEFLLNBF].OKPAGLBJIOH = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigSMin.Value))) //stun 0
                    {
                        NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].NNMDEFLLNBF].OKPAGLBJIOH = 0;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigBMax.Value))) //blindness max
                    {
                        NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].NNMDEFLLNBF].FLOPBFFLLDE = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigBMin.Value))) //blindness 0
                    {
                        NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].NNMDEFLLNBF].FLOPBFFLLDE = 0;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigAMax.Value))) //adrenaline max
                    {
                        NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].NNMDEFLLNBF].BBBGPIILOBB = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigAMin.Value))) //adrenaline 0
                    {
                        NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].NNMDEFLLNBF].BBBGPIILOBB = 0;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigAtMax.Value))) //adr timer max
                    {
                        NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].NNMDEFLLNBF].LLGHFGNMCGF = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigAtMin.Value))) //adr timer 0
                    {
                        NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].NNMDEFLLNBF].LLGHFGNMCGF = 1;
                    }
                    if (Input.GetKeyDown(Ulil.GetKeyCode(ConfigInMax.Value))) //injure
                    {
                        Injure(NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].NNMDEFLLNBF]);
                    }
                    if (Input.GetKeyDown(Ulil.GetKeyCode(ConfigInMin.Value))) //heal
                    {
                        Heal(NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].NNMDEFLLNBF]);
                    }
                    if (Input.GetKeyDown(Ulil.GetKeyCode(ConfigKOMax.Value))) //KO
                    {
                        KnockOut(NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].NNMDEFLLNBF]);
                    }
                    if (Input.GetKeyDown(Ulil.GetKeyCode(ConfigKOMin.Value))) //recover
                    {
                        Recover(NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].NNMDEFLLNBF]);
                    }

                }
                else //player
                {
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigHMax.Value))) //health max
                    {
                        NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].HLGALFAGDGC = int.MaxValue;
                        NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].OIHGGHEDIFF = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigHMin.Value))) //health 0
                    {
                        NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].HLGALFAGDGC = 0;
                        NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].OIHGGHEDIFF = 0;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigSMax.Value))) //stun max
                    {
                        NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].OKPAGLBJIOH = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigSMin.Value))) //stun 0
                    {
                        NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].OKPAGLBJIOH = 0;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigBMax.Value))) //blindness max
                    {
                        NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].FLOPBFFLLDE = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigBMin.Value))) //blindness 0
                    {
                        NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].FLOPBFFLLDE = 0;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigAMax.Value))) //adrenaline max
                    {
                        NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].BBBGPIILOBB = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigAMin.Value))) //adrenaline 0
                    {
                        NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].BBBGPIILOBB = 0;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigAtMax.Value))) //adr timer max
                    {
                        NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].LLGHFGNMCGF = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigAtMin.Value))) //adr timer 0
                    {
                        NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].LLGHFGNMCGF = 1;
                    }
                    if (Input.GetKeyDown(Ulil.GetKeyCode(ConfigInMax.Value))) //injure
                    {
                        Injure(NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF]);
                    }
                    if (Input.GetKeyDown(Ulil.GetKeyCode(ConfigInMin.Value))) //heal
                    {
                        Heal(NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF]);
                    }
                    if (Input.GetKeyDown(Ulil.GetKeyCode(ConfigKOMax.Value))) //KO
                    {
                        KnockOut(NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF]);
                    }
                    if (Input.GetKeyDown(Ulil.GetKeyCode(ConfigKOMin.Value))) //recover
                    {
                        Recover(NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF]);
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
        public static void Injure(DFOGOCNBECG instance)
        {
            //DFOGOCNBECG instance = NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[2].NNMDEFLLNBF];
            instance.PGJEOKAEPCL = UnityEngine.Random.Range(1, 31);
            // instance.EMDMDLNJFKP.injury = instance.AIFCLDHKEJN;
            if (FFCEGMEAIBP.LOBDMDPMFLK == 0 && instance.EMDMDLNJFKP.injuryTime < 2)
            {
                instance.EMDMDLNJFKP.injuryTime = 2;
            }
            CHLPMKEGJBJ.BPLLANFDDDP(instance.GPGOFIFBCLP, CHLPMKEGJBJ.KEMDEGPNJAD[NAEEIFNFBBO.PMEEFNOLAGF(1, 2, 0)], -0.1f, 1f);
            instance.KMGCIKMAJCJ(NAEEIFNFBBO.PMEEFNOLAGF(2, 3, 0), 1f);
            if (FFCEGMEAIBP.FEAOFHKANPP == 16)
            {
                CHLPMKEGJBJ.KIKKPCJGDLM(0, NAEEIFNFBBO.CFPJBJFFJFH(3, 20), 1f);
                FFCEGMEAIBP.MBJFIEPNHPP *= 0.8f;
            }
            else
            {
                CHLPMKEGJBJ.KIKKPCJGDLM(0, -1, 1f);
                CHLPMKEGJBJ.KIKKPCJGDLM(0, NAEEIFNFBBO.PMEEFNOLAGF(16, 20, 0), 0f);
                FFCEGMEAIBP.MBJFIEPNHPP += instance.HNFHLLJOFKI[1] * (instance.HNFHLLJOFKI[1] / 10f) * instance.DFKNBACDFGM(0);
                if (FFCEGMEAIBP.NHMKFGOLHJA == 16)
                {
                    FFCEGMEAIBP.MBJFIEPNHPP += instance.HNFHLLJOFKI[1] * (instance.HNFHLLJOFKI[1] / 10f) * instance.DFKNBACDFGM(0);
                }
            }
            FFCEGMEAIBP.IJOIMLGJION += 250f * instance.DFKNBACDFGM(0);
            instance.HLGALFAGDGC /= 2f;
            instance.BBBGPIILOBB /= 2f;
            if (FFCEGMEAIBP.LOBDMDPMFLK != 0)
            {
                for (int i = 1; i <= NJBJIIIACEP.NBBBLJDBLNM; i++)
                {
                    if (NJBJIIIACEP.OAAMGFLINOB[i].FIEMGOLBHIO == 3 || NJBJIIIACEP.OAAMGFLINOB[i].FIEMGOLBHIO == 0)
                    {
                        NJBJIIIACEP.OAAMGFLINOB[i].PFHOHKJMLLN = 48;
                    }
                }
            }
            Characters.LPGPAKHJMMA(NJBJIIIACEP.OAAMGFLINOB[instance.JNNBBJKLEFK].GOOKPABIPBC, instance.GOOKPABIPBC);
            NJBJIIIACEP.PKGACKAGENN(instance.JNNBBJKLEFK, instance.PLFGKLGCOMD, 1);
        }
        public static void Heal(DFOGOCNBECG instance)
        {
            instance.PGJEOKAEPCL = 0;
            instance.EMDMDLNJFKP.injuryTime = 0;
            instance.EMDMDLNJFKP.injury = 0;
        }

        public static void KnockOut(DFOGOCNBECG instance)
        {
            instance.DMEDPMIPBAO = 1;
            if (FFCEGMEAIBP.LOBDMDPMFLK > 0 && instance.FIEMGOLBHIO == 1 && instance.MGPDGDCIBGC == 0)
            {
                FFCEGMEAIBP.NCAAOLGAGCG(instance.JNNBBJKLEFK, instance.PLFGKLGCOMD);
            }
            instance.FIEMGOLBHIO = 0;
            instance.MGPDGDCIBGC = 1f;


            FFCEGMEAIBP.JPBHIEOKODO = FFCEGMEAIBP.BCENJCEONEB(1);
            if (FFCEGMEAIBP.JPBHIEOKODO == 0)
            {
                FFCEGMEAIBP.CADLONHABMC = 0;
                if (FFCEGMEAIBP.GDKCEGBINCM >= 1 && FFCEGMEAIBP.GDKCEGBINCM <= 2)
                {
                    FFCEGMEAIBP.GDKCEGBINCM = 0;
                }
            }
        }
        public static void Recover(DFOGOCNBECG instance)
        {
            instance.DMEDPMIPBAO = 0;
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
//target                    NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].NNMDEFLLNBF].
//self                     NJBJIIIACEP.OAAMGFLINOB[NJBJIIIACEP.DCAFAIGGFCC[1].NNMDEFLLNBF].