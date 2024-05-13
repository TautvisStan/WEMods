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

namespace MatchMusic
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.MatchMusic";
        public const string PluginName = "MatchMusic";
        public const string PluginVer = "1.0.1";

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


        public static GameObject MatchMusicObject = null;
        public static AudioSource MatchMusic = null;
        static bool canBePlayed = false;
        static int OldSong = -1;
        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);
            LoadAudioFiles();


        }
        [HarmonyPatch(typeof(CHLPMKEGJBJ), nameof(CHLPMKEGJBJ.ICGNAJFLAHL))]
        [HarmonyPostfix]
        private static void EnableMatchMusic()
        {
            MatchMusicObject = (UnityEngine.Object.Instantiate(NAEEIFNFBBO.JFHPHDKKECG("Sound", "Speaker")) as GameObject);
            MatchMusicObject.name = "Match Music";
            UnityEngine.Object.DontDestroyOnLoad(MatchMusicObject);
            MatchMusic = MatchMusicObject.GetComponent<AudioSource>();
            MatchMusic.loop = false;
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
            if (MatchMusicObject != null)
            {
                if (canBePlayed && !MatchMusic.isPlaying)
                {
                    PlayMusic();
                }


                if(canBePlayed && CHLPMKEGJBJ.OGCBMJIIKPP.isPlaying)
                {
                    MatchMusic.volume = 0;
                }
                if (canBePlayed && !CHLPMKEGJBJ.OGCBMJIIKPP.isPlaying)
                {
                    MatchMusic.volume = CHLPMKEGJBJ.FGFOLDGNLND * CHLPMKEGJBJ.FGFOLDGNLND;
                }
            }
        }
        static void LoadAudioFiles()
        {
            DirectoryInfo dir = new DirectoryInfo(PluginPath);
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

        public static void PlayMusic()
        {
            canBePlayed = true;
            if (MatchMusicObject == null) { return; }
            int song = UnityEngine.Random.RandomRangeInt(0, AudioClips.Count);
            if (song == OldSong)
            {
                song++;
                if (song >= AudioClips.Count)
                {
                    song = 0;
                }
            }

            MatchMusic.clip = AudioClips[song];
            MatchMusic.volume = CHLPMKEGJBJ.FGFOLDGNLND * CHLPMKEGJBJ.FGFOLDGNLND;
            MatchMusic.time = 0f;
            MatchMusic.Play();
            OldSong = song;
        }

        public static void StopMusic()
        {
            canBePlayed = false;
            if (MatchMusicObject != null)
            {
                if (MatchMusic.isPlaying)
                    MatchMusic.Stop();
            }
            OldSong = -1;
        }


        [HarmonyPatch(typeof(FFCEGMEAIBP), nameof(FFCEGMEAIBP.JMNBPEJAFEG))]
        [HarmonyPostfix]
        public static void Interfere()
        {
            
            PlayMusic();
        }
        [HarmonyPatch(typeof(FFCEGMEAIBP),nameof(FFCEGMEAIBP.DMJFCHKLEFH))]
        [HarmonyPostfix]
        public static void MatchStart()
        {

            PlayMusic();
        }

        [HarmonyPatch(typeof(FFCEGMEAIBP), nameof(FFCEGMEAIBP.EAAIHKLJFCM))]
        [HarmonyPostfix]
        public static void MatchRestart()
        {

            PlayMusic();
        }

        [HarmonyPatch(typeof(FFCEGMEAIBP), nameof(FFCEGMEAIBP.BAGEPNPJPLD))]
        [HarmonyPostfix]
        public static void MatchEnd()
        {

            StopMusic();
        }

        [HarmonyPatch(typeof(FFCEGMEAIBP), nameof(FFCEGMEAIBP.GEGCMDLMJAJ))]
        [HarmonyPrefix]
        public static void SceneChange()
        {

            StopMusic();
        }
    }
}