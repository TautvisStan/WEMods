using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace HealthCheats
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.HealthCheats";
        public const string PluginName = "HealthCheats";
        public const string PluginVer = "1.2.0";

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
                        FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].CJGHFHCHDNN].BGFIMKNFOBF = int.MaxValue;
                        FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].CJGHFHCHDNN].OODIGDBHDHC = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigHMin.Value))) //health 0
                    {
                        FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].CJGHFHCHDNN].BGFIMKNFOBF = 0;
                        FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].CJGHFHCHDNN].OODIGDBHDHC = 0;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigSMax.Value))) //stun max
                    {
                        FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].CJGHFHCHDNN].EEPJILNNONC = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigSMin.Value))) //stun 0
                    {
                        FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].CJGHFHCHDNN].EEPJILNNONC = 0;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigBMax.Value))) //blindness max
                    {
                        FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].CJGHFHCHDNN].AEAKGBCLAEL = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigBMin.Value))) //blindness 0
                    {
                        FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].CJGHFHCHDNN].AEAKGBCLAEL = 0;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigAMax.Value))) //adrenaline max
                    {
                        FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].CJGHFHCHDNN].HBCNKIKAIKM = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigAMin.Value))) //adrenaline 0
                    {
                        FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].CJGHFHCHDNN].HBCNKIKAIKM = 0;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigAtMax.Value))) //adr timer max
                    {
                        FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].CJGHFHCHDNN].CFPAAMEFEOL = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigAtMin.Value))) //adr timer 0
                    {
                        FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].CJGHFHCHDNN].CFPAAMEFEOL = 1;
                    }
                    if (Input.GetKeyDown(Ulil.GetKeyCode(ConfigInMax.Value))) //injure
                    {
                        Injure(FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].CJGHFHCHDNN]);
                    }
                    if (Input.GetKeyDown(Ulil.GetKeyCode(ConfigInMin.Value))) //heal
                    {
                        Heal(FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].CJGHFHCHDNN]);
                    }
                    if (Input.GetKeyDown(Ulil.GetKeyCode(ConfigKOMax.Value))) //KO
                    {
                        KnockOut(FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].CJGHFHCHDNN]);
                    }
                    if (Input.GetKeyDown(Ulil.GetKeyCode(ConfigKOMin.Value))) //recover
                    {
                        Recover(FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].CJGHFHCHDNN]);
                    }

                }
                else //player
                {
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigHMax.Value))) //health max
                    {
                        FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].BGFIMKNFOBF = int.MaxValue;
                        FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].OODIGDBHDHC = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigHMin.Value))) //health 0
                    {
                        FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].BGFIMKNFOBF = 0;
                        FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].OODIGDBHDHC = 0;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigSMax.Value))) //stun max
                    {
                        FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].EEPJILNNONC = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigSMin.Value))) //stun 0
                    {
                        FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].EEPJILNNONC = 0;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigBMax.Value))) //blindness max
                    {
                        FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].AEAKGBCLAEL = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigBMin.Value))) //blindness 0
                    {
                        FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].AEAKGBCLAEL = 0;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigAMax.Value))) //adrenaline max
                    {
                        FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].HBCNKIKAIKM = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigAMin.Value))) //adrenaline 0
                    {
                        FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].HBCNKIKAIKM = 0;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigAtMax.Value))) //adr timer max
                    {
                        FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].CFPAAMEFEOL = int.MaxValue;
                    }
                    if (Input.GetKey(Ulil.GetKeyCode(ConfigAtMin.Value))) //adr timer 0
                    {
                        FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].CFPAAMEFEOL = 1;
                    }
                    if (Input.GetKeyDown(Ulil.GetKeyCode(ConfigInMax.Value))) //injure
                    {
                        Injure(FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN]);
                    }
                    if (Input.GetKeyDown(Ulil.GetKeyCode(ConfigInMin.Value))) //heal
                    {
                        Heal(FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN]);
                    }
                    if (Input.GetKeyDown(Ulil.GetKeyCode(ConfigKOMax.Value))) //KO
                    {
                        KnockOut(FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN]);
                    }
                    if (Input.GetKeyDown(Ulil.GetKeyCode(ConfigKOMin.Value))) //recover
                    {
                        Recover(FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN]);
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
        public static void Injure(DJEKCMMMFJM instance)
        {
            //DJEKCMMMFJM instance = FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[2].CJGHFHCHDNN];
            instance.HLALEJPPKBO = UnityEngine.Random.Range(1, 31);
            // instance.LLEGGMCIALJ.injury = instance.MGCGBGHEBFJ;
            if (PHECEOMIMND.IPAFPBPKIKP == 0 && instance.LLEGGMCIALJ.injuryTime < 2)
            {
                instance.LLEGGMCIALJ.injuryTime = 2;
            }
            IKPECOJMCAB.OBLNONIKENE(instance.MLDLMDCFHOM, IKPECOJMCAB.MGOGPDAECEM[LFNJDEGJLLJ.NBNFJOFFMHO(1, 2, 0)], -0.1f, 1f);
            instance.IIMFKLDIKOA(LFNJDEGJLLJ.NBNFJOFFMHO(2, 3, 0), 1f);
            if (PHECEOMIMND.CKILNIEAAPD == 16)
            {
                IKPECOJMCAB.GNILNDHIFEG(0, LFNJDEGJLLJ.ICJAEBKBCNH(3, 20), 1f);
                PHECEOMIMND.FGMJCPFIIEE *= 0.8f;
            }
            else
            {
                IKPECOJMCAB.GNILNDHIFEG(0, -1, 1f);
                IKPECOJMCAB.GNILNDHIFEG(0, LFNJDEGJLLJ.NBNFJOFFMHO(16, 20, 0), 0f);
                PHECEOMIMND.FGMJCPFIIEE += instance.IDLOAACGLKA[1] * (instance.IDLOAACGLKA[1] / 10f) * instance.BKJENLNOFOD(0);
                if (PHECEOMIMND.DEIKILLNJOI == 16)
                {
                    PHECEOMIMND.FGMJCPFIIEE += instance.IDLOAACGLKA[1] * (instance.IDLOAACGLKA[1] / 10f) * instance.BKJENLNOFOD(0);
                }
            }
            PHECEOMIMND.HBBLNFPONMG += 250f * instance.BKJENLNOFOD(0);
            instance.BGFIMKNFOBF /= 2f;
            instance.HBCNKIKAIKM /= 2f;
            if (PHECEOMIMND.IPAFPBPKIKP != 0)
            {
                for (int i = 1; i <= FFKMIEMAJML.HIKHEJJKJAE; i++)
                {
                    if (FFKMIEMAJML.FJCOPECCEKN[i].FOPIBFHEBHM == 3 || FFKMIEMAJML.FJCOPECCEKN[i].FOPIBFHEBHM == 0)
                    {
                        FFKMIEMAJML.FJCOPECCEKN[i].AKDIGFNDMFP = 48;
                    }
                }
            }
            Characters.PLAKMCOFJDH(FFKMIEMAJML.FJCOPECCEKN[instance.CCCLFMLNPIK].ALFACADGNDC, instance.ALFACADGNDC);
            FFKMIEMAJML.OJCAIJPIDCG(instance.CCCLFMLNPIK, instance.DHBIELODIAN, 1);
        }
        public static void Heal(DJEKCMMMFJM instance)
        {
            instance.HLALEJPPKBO = 0;
            instance.LLEGGMCIALJ.injuryTime = 0;
            instance.LLEGGMCIALJ.injury = 0;
        }

        public static void KnockOut(DJEKCMMMFJM instance)
        {
            instance.EFEPIJKLOEF = 1;
            if (PHECEOMIMND.IPAFPBPKIKP > 0 && instance.FOPIBFHEBHM == 1 && instance.MBDGNNMBEPH == 0)
            {
                PHECEOMIMND.MOPIODHDBNK(instance.CCCLFMLNPIK, instance.DHBIELODIAN);
            }
            instance.FOPIBFHEBHM = 0;
            instance.MBDGNNMBEPH = 1f;


            PHECEOMIMND.NNJEDKEFIGL = PHECEOMIMND.JACHPAHIGPP(1);
            if (PHECEOMIMND.NNJEDKEFIGL == 0)
            {
                PHECEOMIMND.ACABDNDNJPN = 0;
                if (PHECEOMIMND.ONIAMCHKGEL >= 1 && PHECEOMIMND.ONIAMCHKGEL <= 2)
                {
                    PHECEOMIMND.ONIAMCHKGEL = 0;
                }
            }
        }
        public static void Recover(DJEKCMMMFJM instance)
        {
            instance.EFEPIJKLOEF = 0;
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
//target                    FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].CJGHFHCHDNN].
//self                     FFKMIEMAJML.FJCOPECCEKN[FFKMIEMAJML.ODPPBDDAIGI[1].CJGHFHCHDNN].