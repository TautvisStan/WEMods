using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static ReplicaTitleBelt.Plugin;

namespace ReplicaTitleBelt
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.ReplicaTitleBelt";
        public const string PluginName = "ReplicaTitleBelt";
        public const string PluginVer = "1.0.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        public class CustomBeltProp
        {
            public string name = "Replica Belt";
            public string texture = "";
            public float R = 0f;
            public float G = 0f;
            public float B = 0f;
        }
        public static Dictionary<string, Texture> customBeltTextures = new();
        public static Dictionary<int, CustomBeltProp> customBeltProps = new();
        private static readonly List<string> Extensions = new()
    {
        ".txt",
        ".json"
    };

        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);
        }

        private void OnEnable()
        {
            Harmony.PatchAll();
            LoadAllFiles();
            Logger.LogInfo($"Loaded {PluginName}!");
        }

        public static void LoadAllFiles()
        {
            DirectoryInfo dir = new DirectoryInfo(PluginPath);
            FileInfo[] files = dir.GetFiles("*")
                        .Where(f => Extensions.Contains(f.Extension.ToLower())).ToArray();
            foreach (FileInfo file in files)
            {
                if(file.Name != "manifest.txt")
                    LoadJsonFile(file.FullName);
            }
        }
        public static void LoadJsonFile(string path)
        {
            if (File.Exists(path))
            {
                using (StreamReader file = File.OpenText(path))
                {
                    CustomBeltProp weapon = new();
                    JsonSerializer serializer = new JsonSerializer();
                    try
                    {
                        weapon = (CustomBeltProp)serializer.Deserialize(file, typeof(CustomBeltProp));
                        JFLEBEBCGFA.JECOJHEMKFP++;
                        customBeltProps.Add(JFLEBEBCGFA.JECOJHEMKFP, weapon);
                        try
                        {
                            if (weapon.texture != "")
                            {
                                string text = PluginPath + "\\" + weapon.texture;
                                byte[] array = File.ReadAllBytes(text);
                                if (array != null)
                                {
                                    Texture2D texture2D = new Texture2D(1, 1);
                                    ImageConversion.LoadImage(texture2D, array);
                                    Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
                                    Texture2D texture = sprite.texture;
                                    customBeltTextures.Add(weapon.texture, texture);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Log.LogError("Failed to load replica belt texture " + weapon.texture);
                            Log.LogError(e);
                        }
                        Log.LogInfo("Added belt prop name " + weapon.name + " texture " + weapon.texture);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Failed to load replica belt data.");
                        Debug.LogError(e);
                    }
                    if (weapon == null)
                    {
                        Debug.LogError("Failed to load replica belt data.");
                    }
                }
            }
        }


        private void OnDisable()
        {
            Harmony.UnpatchSelf();
            Logger.LogInfo($"Unloaded {PluginName}!");
        }
        [HarmonyPatch(typeof(GDFKEAMIOAG), nameof(GDFKEAMIOAG.KMOINLEIEFP))]
        [HarmonyPostfix]
        public static void GDFKEAMIOAG_KMOINLEIEFP(GDFKEAMIOAG __instance)
        {
            CustomBeltProp weapon = new();
            if (customBeltProps.TryGetValue(__instance.BPJFLJPKKJK, out weapon))
            //  if (__instance.BPJFLJPKKJK == 69)
            {
                __instance.CMECDGMCMLC = weapon.name;
                __instance.NONJIHAJAKC = "Belt";
                __instance.CBBOMMJEMEF = new AudioClip[] { CHLPMKEGJBJ.OECAJKLOMIC };
                __instance.GMNHNDEEIBA = 111;
                __instance.OJNPFNEELLF = new Color(0.9f, 0.8f, 0.5f);
                __instance.NALEIJHPOHN = 0.2f;
                __instance.FBAMIOMCLKM = 2f;
                __instance.NIMHPNKOPAE = 3f;
                __instance.EIFCLCNIPKA = 0.25f;
                __instance.NJKGLFMAKNK = 0.5f;
                __instance.IKONDJPBOGP = 0;
                __instance.HNHEMLEAKOM = 150f;
                __instance.BEMKANDCFCP = 1f;
                __instance.CHKGDFAGEBI = 1000;
                __instance.KCANMKBKDEJ = 5;
                __instance.DAFHMKMKIFF = new Vector3(0f, __instance.NALEIJHPOHN + (float)__instance.PLFGKLGCOMD / 100f, 0f);
                __instance.ONNIHNBMCBD = new Vector3(-90f, 0f, 0f);
                __instance.IPEFDKPLIAA = new Vector3(-0.6f, -1.1f, 0.35f);
                __instance.AGNNECLKOCH = new Vector3(0f, 0f, 0f);
                __instance.JFNOICCHGMO = new Vector3(0.4f, -1.1f, -0.1f);
                __instance.IONAHCBOOIJ = new Vector3(0f, 90f, -7f);
                __instance.FJBNEAKPHDH = 94;
                __instance.FOEHOAJMLFN = 95;
                __instance.BCBNIBNNDKF = 10;
            }
        }
        [HarmonyPatch(typeof(GDFKEAMIOAG), nameof(GDFKEAMIOAG.ICGNAJFLAHL))]
        [HarmonyPostfix]
        public static void GDFKEAMIOAG_ICGNAJFLAHL(GDFKEAMIOAG __instance, int APCLJHNGGFM, int CDBLEDLMLEF, int CHMHJJNEMKB)
        {
            CustomBeltProp weapon = new();
            if (customBeltProps.TryGetValue(__instance.BPJFLJPKKJK, out weapon))
            {
                float num3 = weapon.R;
                float num4 = weapon.G;
                float num5 = weapon.B;
                Material[] array = new Material[2];
                int num6 = 0;
                array[num6] = new Material(PFKAPGFJKHH.BEKNPDFCADC);
                if (num3 > 0.2f || num4 > 0.2f || num5 > 0.2f)
                {
                    array[num6].SetTexture("_MainTex", NAEEIFNFBBO.JFHPHDKKECG("World/Belts", "Belt_Base_Light") as Texture);
                    PFKAPGFJKHH.DCHOONCENIP(array[num6], num3, num4, num5);
                }
                else
                {
                    array[num6].SetTexture("_MainTex", NAEEIFNFBBO.JFHPHDKKECG("World/Belts", "Belt_Base_Dark") as Texture);
                }
                array[num6].SetFloat("_Glossiness", 0.75f);
                num6 = 1;
                array[num6] = new Material(PFKAPGFJKHH.KAENFJPIIEG);
                Texture texture2 = null;
                customBeltTextures.TryGetValue(weapon.texture, out texture2);
                if (texture2 == null)
                {
                    texture2 = NAEEIFNFBBO.JFHPHDKKECG("World/Belts", "Fed01_Texture" + "01") as Texture;
                }
                array[num6].SetTexture("_MainTex", texture2);
                array[num6].SetFloat("_Glossiness", 0.75f);
                __instance.PCNHIIPBNEK.GetComponent<Renderer>().materials = array;
            }
        }
        //Prevent the custom belt from appearing randomly
        [HarmonyPatch(typeof(JFLEBEBCGFA), nameof(JFLEBEBCGFA.AGPMGEKLILB))]
        [HarmonyPostfix]
        public static void JFLEBEBCGFA_AGPMGEKLILB(ref int __result)
        {
            if (customBeltProps.ContainsKey(__result)) __result = JFLEBEBCGFA.AGPMGEKLILB();
        }

    }
}