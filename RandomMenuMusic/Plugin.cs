using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace MenuMusicRandomizer
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.MenuMusicRandomizer";
        public const string PluginName = "MenuMusicRandomizer";
        public const string PluginVer = "1.0.3";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        public static List<AudioClip> AudioClips = new();
        private static readonly List<string> AudioExtensions = new()
    {
        ".ogg",
        ".wav",
        ".mp3",
        ".aif",
        ".aiff",
        ".mod",
        ".xm",
        ".it",
        ".s3m"
    };
        public static ConfigEntry<bool> UseVanilla;
        private static int OldSong;
        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);
            UseVanilla = Config.Bind("General", "Use Vanilla Songs", true, "Disabling this will prevent vanilla songs (\"Theme00.ogg\") from playing. If there are no custom songs nothing will be played");
            foreach (string modPath in Directory.GetDirectories(Path.Combine(Paths.BepInExRootPath, "plugins")))
            {
                ScanFolder(modPath);
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
        private void Update()
        {
            if (CHLPMKEGJBJ.LMLOGJBBKFH == -1 && SceneManager.GetActiveScene().name != "Game" && !CHLPMKEGJBJ.OGCBMJIIKPP.isPlaying)
            {
                SetupMenuSong(-1, CHLPMKEGJBJ.OGCBMJIIKPP.pitch, 1);
            }
        }
        static void ScanFolder(string path)
        {
            bool found = false;
            if(path.Contains("MenuMusic"))
            {
                LoadAudioFiles(path);
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
        static void LoadAudioFiles(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] files = dir.GetFiles("*")
                        .Where(f => AudioExtensions.Contains(f.Extension.ToLower())).ToArray();
            foreach (FileInfo file in files)
            {
                string fileName = file.Name;
                try
                {
                    AudioClip clip;
                        UnityWebRequest wr = new(file.FullName);
                        wr.downloadHandler = new DownloadHandlerAudioClip(file.Name, AudioType.UNKNOWN);
                        wr.SendWebRequest();
                        while (!wr.isDone) { }

                        clip = DownloadHandlerAudioClip.GetContent(wr);
                        wr.Dispose();
                        clip.name = fileName;
                    AudioClips.Add(clip);
                }
                catch (Exception e)
                {
                    Log.LogError(e);
                }

                GC.Collect();
            }
        }
        [HarmonyPatch(typeof(CHLPMKEGJBJ), nameof(CHLPMKEGJBJ.DGAGLLKPNJB))]
        [HarmonyPostfix]
        static void CHLPMKEGJBJ_DGAGLLKPNJB_Prefix(int JJNMLJBGPAH, float MCJHGEHEPMD, float CDNNGHGFALM)
        {
            if (CDNNGHGFALM == 0f) return;
            if (JJNMLJBGPAH >= 0f)
            {
                CHLPMKEGJBJ.OGCBMJIIKPP.loop = true;
                return;
            }
            else
            {
                SetupMenuSong(JJNMLJBGPAH, MCJHGEHEPMD, CDNNGHGFALM);
                return;
            }

        }
        public static void SetupMenuSong(int JJNMLJBGPAH, float MCJHGEHEPMD, float CDNNGHGFALM)
        {
            CHLPMKEGJBJ.OGCBMJIIKPP.loop = false;
            if (!UseVanilla.Value && AudioClips.Count == 0)
            {
                CHLPMKEGJBJ.DGAGLLKPNJB(0, 0, 0);
                return;
            }
            int song;
            if (!UseVanilla.Value) song = UnityEngine.Random.RandomRangeInt(0, AudioClips.Count);
            else song = UnityEngine.Random.RandomRangeInt(-1, AudioClips.Count);
            if (song == OldSong)
            {
                song++;
                if (song >= AudioClips.Count)
                {
                    song = -1;
                    if (!UseVanilla.Value) song++;
                }
            }
            if (song == -1)
            {
                if (CHLPMKEGJBJ.OOFPHCHKOBE[0] == null)
                {
                    CHLPMKEGJBJ.OOFPHCHKOBE[0] = NAEEIFNFBBO.JFHPHDKKECG("Music", "Theme00") as AudioClip;
                }
                CHLPMKEGJBJ.OGCBMJIIKPP.clip = CHLPMKEGJBJ.OOFPHCHKOBE[0];
            }
            else
            {
                CHLPMKEGJBJ.OGCBMJIIKPP.clip = AudioClips[song];
            }
            CHLPMKEGJBJ.LMLOGJBBKFH = -1;
            CHLPMKEGJBJ.OGCBMJIIKPP.time = 0f;
            CHLPMKEGJBJ.OGCBMJIIKPP.Play();
            CHLPMKEGJBJ.CNNKEACKKCD = JJNMLJBGPAH;
            CHLPMKEGJBJ.OGCBMJIIKPP.pitch = MCJHGEHEPMD;
            CHLPMKEGJBJ.GFJDCNMOMKD = 0;
            OldSong = song;
        }

    }
}