using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using BepInEx.Configuration;
using WECCL.API;

namespace FontReplacer
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.FontReplacer";
        public const string PluginName = "FontReplacer";
        public const string PluginVer = "0.8.2";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;


        public static List<Font> Fonts = new();
        public static Dictionary<string, Font> FontsDict = new();

        public static ConfigEntry<string> OSFontName;

        public static int CurrentFont = -1;


        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);

            foreach (string modPath in Directory.GetDirectories(Path.Combine(Paths.BepInExRootPath, "plugins")))
            {
                ScanFolder(modPath);
            }
            OSFontName = Config.Bind("General",
                 "OS font name",
                 "",
                 "Enter the font name that's installed on your OS");

            Buttons.RegisterCustomButton(this, "Apply OS font", () =>
            {
                try
                {
                    Fonts[0] = Font.CreateDynamicFontFromOSFont(OSFontName.Value, 10);
                    CurrentFont = 0;
                    return "Font applied!";
                }
                catch (Exception e)
                {
                    CurrentFont = -1;
                    return "Failed to apply: " + e.Message;
                }
            });

            if(OSFontName.Value != "")
            {
                Fonts[0] = Font.CreateDynamicFontFromOSFont(OSFontName.Value, 10);
                CurrentFont = 0;
            }

        }
        static void ScanFolder(string path)
        {
          //  Debug.LogWarning("SCANNING " + path);
            bool found = false;
            if (path.ToLower().Contains("fonts"))
            {
                LoadFontBundles(path);
                found = true;
            }
            if (!found)
            {
                foreach (string subDir in Directory.GetDirectories(path))
                {
                    ScanFolder(subDir);
                }
            }
        }

        public static void LoadFontBundles(string path)
        {
          //  Debug.LogWarning("LOADINF ROM " + path);
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] files = dir.GetFiles("*");

            foreach (FileInfo file in files) 
            {
                if (file.Name.StartsWith("asset_"))
                {
                    foreach (Font font in ObtainFontsFromFile(file.FullName))
                        Fonts.Add(font);
                }
            }
        }

        public static List<Font> ObtainFontsFromFile(string filename)
        {
       //     Debug.LogWarning("FILE " + filename);
            List<Font> fonts = new();
            AssetBundle assetBundle = new();
            try
            {
                assetBundle = AssetBundle.LoadFromFile(filename);
            }
            catch
            {
                Debug.LogWarning("FAILED " + filename);
                return fonts;
            }
            if (assetBundle == null) return fonts;
            foreach (string s in assetBundle.GetAllAssetNames())
            {
                try
                {
                    fonts.Add(assetBundle.LoadAsset<Font>(s));
    //                Debug.LogWarning("ADDED " + fonts[fonts.Count - 1].name);
                }
                catch
                {
                    Log.LogWarning("Failed to load font asset " + s + "");
                }
            }
            return fonts;

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

        [HarmonyPatch(typeof(Scene_Atlas),nameof(Scene_Atlas.Start))]
        [HarmonyPostfix]
        public static void Scene_Atlas_Start()
        {
            ChangeFont();
        }
        [HarmonyPatch(typeof(Scene_Calendar), nameof(Scene_Calendar.Start))]
        [HarmonyPostfix]
        public static void Scene_Calendar_Start()
        {
            ChangeFont();
        }
        [HarmonyPatch(typeof(Scene_Calibration), nameof(Scene_Calibration.Start))]
        [HarmonyPostfix]
        public static void Scene_Calibration_Start()
        {
            ChangeFont();
        }
        [HarmonyPatch(typeof(Scene_Card), nameof(Scene_Card.Start))]
        [HarmonyPostfix]
        public static void Scene_Card_Start()
        {
            ChangeFont();
        }
        [HarmonyPatch(typeof(Scene_Charts), nameof(Scene_Charts.Start))]
        [HarmonyPostfix]
        public static void Scene_Charts_Start()
        {
            ChangeFont();
        }
        [HarmonyPatch(typeof(Scene_Controllers), nameof(Scene_Controllers.Start))]
        [HarmonyPostfix]
        public static void Scene_Controllers_Start()
        {
            ChangeFont();
        }
        [HarmonyPatch(typeof(Scene_Database), nameof(Scene_Database.Start))]
        [HarmonyPostfix]
        public static void Scene_Database_Start()
        {
            ChangeFont();
        }
        [HarmonyPatch(typeof(Scene_Editor), nameof(Scene_Editor.Start))]
        [HarmonyPostfix]
        public static void Scene_Editor_Start()
        {
            ChangeFont();
        }
        [HarmonyPatch(typeof(Scene_Finances), nameof(Scene_Finances.Start))]
        [HarmonyPostfix]
        public static void Scene_Finances_Start()
        {
            ChangeFont();
        }
        [HarmonyPatch(typeof(Scene_Game), nameof(Scene_Game.Start))]
        [HarmonyPostfix]
        public static void Scene_Game_Start()
        {
            ChangeFont();
        }
        [HarmonyPatch(typeof(Scene_Loading), nameof(Scene_Loading.Start))]
        [HarmonyPostfix]
        public static void Scene_Loading_Start()
        {
            ChangeFont();
        }
        [HarmonyPatch(typeof(Scene_Map), nameof(Scene_Map.Start))]
        [HarmonyPostfix]
        public static void Scene_Map_Start()
        {
            ChangeFont();
        }
        [HarmonyPatch(typeof(Scene_Match_Setup), nameof(Scene_Match_Setup.Start))]
        [HarmonyPostfix]
        public static void Scene_Match_Setup_Start()
        {
            ChangeFont();
        }
        [HarmonyPatch(typeof(Scene_Meeting), nameof(Scene_Meeting.Start))]
        [HarmonyPostfix]
        public static void Scene_Meeting_Start()
        {
            ChangeFont();
        }
        [HarmonyPatch(typeof(Scene_News), nameof(Scene_News.Start))]
        [HarmonyPostfix]
        public static void Scene_News_Start()
        {
            ChangeFont();
        }
        [HarmonyPatch(typeof(Scene_NextWeek), nameof(Scene_NextWeek.Start))]
        [HarmonyPostfix]
        public static void Scene_NextWeek_Start()
        {
            ChangeFont();
        }
        [HarmonyPatch(typeof(Scene_Options), nameof(Scene_Options.Start))]
        [HarmonyPostfix]
        public static void Scene_Options_Start()
        {
            ChangeFont();
        }
        [HarmonyPatch(typeof(Scene_Roster_Editor), nameof(Scene_Roster_Editor.Start))]
        [HarmonyPostfix]
        public static void Scene_Roster_Editor_Start()
        {
            ChangeFont();
        }
        [HarmonyPatch(typeof(Scene_Save), nameof(Scene_Save.Start))]
        [HarmonyPostfix]
        public static void Scene_Save_Start()
        {
            ChangeFont();
        }
        [HarmonyPatch(typeof(Scene_Select_Char), nameof(Scene_Select_Char.Start))]
        [HarmonyPostfix]
        public static void Scene_Select_Char_Start()
        {
            ChangeFont();
        }
        [HarmonyPatch(typeof(Scene_Titles), nameof(Scene_Titles.Start))]
        [HarmonyPostfix]
        public static void Scene_Titles_Start()
        {
            ChangeFont();
        }

        [HarmonyPatch(typeof(LIPNHOMGGHF), nameof(LIPNHOMGGHF.OPBBKBAJJHA))]
        [HarmonyPostfix]
        public static void LIPNHOMGGHF_OPBBKBAJJHA_Postfix()
        {
            if(HKJOAJOKOIJ.NDNHBEDJFIJ != null)
                ChangeFont(HKJOAJOKOIJ.NDNHBEDJFIJ);
        }
        [HarmonyPatch(typeof(AKFIIKOMPLL), nameof(AKFIIKOMPLL.ICGNAJFLAHL))]
        [HarmonyPostfix]
        public static void AKFIIKOMPLL_ICGNAJFLAHL_Postfix(AKFIIKOMPLL __instance, int CHMHJJNEMKB, string NMKKHDOGOGA, float DPBNKMPJJOJ, float NKEMECHAEEJ, float BGPLCHIKEAK, float JOIPMMGOLFI)
        {
            ChangeFont(__instance.JGHBIPNIHBK);
        }

        public static void ChangeFont()
        {
            if (CurrentFont != -1)
            {

                Text[] TextObjs = LIPNHOMGGHF.JPABICKOAEO.GetComponentsInChildren<Text>(true);//GameObject.FindObjectsOfType(typeof(Text), true) as Text[];
                foreach (Text t in TextObjs)
                {
                    t.font = Fonts[CurrentFont];
                }
            }
        }
        public static void ChangeFont(GameObject obj)
        {
            if (CurrentFont != -1)
            {

                Text[] TextObjs = obj.GetComponentsInChildren<Text>();//GameObject.FindObjectsOfType(typeof(Text), true) as Text[];
                foreach (Text t in TextObjs)
                {
                    t.font = Fonts[CurrentFont];
                }
            }
        }

        /*   private void test()
           {
               AssetBundle assetBundle = AssetBundle.LoadFromFile("C:\\Program Files (x86)\\Steam\\steamapps\\common\\Wrestling Empire\\BepInEx\\plugins\\pricedown");
               foreach (string s in assetBundle.GetAllAssetNames())
               {
                   f = assetBundle.LoadAsset<Font>(s);
               }


               UnityEngine.UI.Text[] hinges = GameObject.FindObjectsOfType(typeof(UnityEngine.UI.Text)) as UnityEngine.UI.Text[];

               foreach (UnityEngine.UI.Text t in hinges)
               {
                   t.font = f;
               }
           }*/



    }
}