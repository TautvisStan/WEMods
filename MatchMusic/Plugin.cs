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
        public const string PluginVer = "1.0.4";

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
            foreach (string modPath in Directory.GetDirectories(Path.Combine(Paths.BepInExRootPath, "plugins")))
            {
                ScanFolder(modPath);
            }


        }
        static void ScanFolder(string path)
        {
            bool found = false;
            if (path.Contains("MatchMusic"))
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
 //                   Log.LogWarning("PLAYING NEXT SONG BECAUSE canBePlayed=" + canBePlayed + " & isPlaying=" + MatchMusic.isPlaying);
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

           /* int i = 0;
            foreach(AudioClip clip in AudioClips)
            {
  //              Log.LogWarning("Song " + i + ": " + clip.name + ", length " + clip.length);
                i++;
            }*/
  //          Log.LogWarning("-----------------------------------------");
        }

        public static void PlayMusic()
        {
 //           Log.LogWarning("TRYING TO PLAY NEXT SONG");
  //          Log.LogWarning("OLD SONG WAS " + OldSong);
            if (AudioClips.Count != 0)
            {
                canBePlayed = true;
                if (MatchMusicObject == null) { return; }
 //               Log.LogWarning("ROLLING NEXT SONG BETWEEN 0 AND " + AudioClips.Count);
                int song = UnityEngine.Random.RandomRangeInt(0, AudioClips.Count);
 //               Log.LogWarning("ROLLED NEXT SONG " + song);
                if (song == OldSong)
                {
 //                   Log.LogWarning("INCREASING NEXT SONG BECAUSE OLD SONG WAS " + OldSong);
                    song++;
                    if (song >= AudioClips.Count)
                    {
  //                      Log.LogWarning("SETTING NEXT SONG TO 0 BECAUSE SONG= " + song + " AND THERE IS TOTAL OF " + AudioClips.Count);
                        song = 0;
                    }
                }

                MatchMusic.clip = AudioClips[song];
                MatchMusic.volume = CHLPMKEGJBJ.FGFOLDGNLND * CHLPMKEGJBJ.FGFOLDGNLND;
                MatchMusic.time = 0f;
                MatchMusic.Play();
                OldSong = song;
 //               Log.LogWarning("PLAYING SONG " + song + ", " + MatchMusic.clip.name+ ", length " + MatchMusic.clip.length);
            }
  //          Log.LogWarning("--------------------");
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
  //          Log.LogWarning("PLAYING SONG ON INTERFERE");
            PlayMusic();
        }
        [HarmonyPatch(typeof(FFCEGMEAIBP),nameof(FFCEGMEAIBP.DMJFCHKLEFH))]
        [HarmonyPostfix]
        public static void MatchStart()
        {
            int num = 1;
            if (World.ringShape > 0)
            {
                int i = 1;
                while (i <= NJBJIIIACEP.NBBBLJDBLNM)
                {
                    DFOGOCNBECG dfogocnbecg = NJBJIIIACEP.OAAMGFLINOB[i];
                    if (dfogocnbecg.AHBNKMMMGFI > 0f && dfogocnbecg.KGELHDKDHFM() > 0 && (dfogocnbecg.EKOHAKPAOGN != World.ringGround || (dfogocnbecg.NELODEMHJHN >= -148 && dfogocnbecg.NELODEMHJHN <= -131)))
                    {
                        num = 0;
                        if (dfogocnbecg.OJAJENJLBMF < 0)
                        {
                            while (GIMNNPMAKNJ.MDLFHNCMFDO(dfogocnbecg.MOIMCJOBJME, dfogocnbecg.CEKNDFGOILP, 0f) == 0)
                            {
                                dfogocnbecg.MOIMCJOBJME = UnityEngine.Random.Range(-25f, 25f) * World.ringSize;
                                dfogocnbecg.CEKNDFGOILP = UnityEngine.Random.Range(-25f, 25f) * World.ringSize;
                                dfogocnbecg.PPLFGLIJPDJ = 0;
                                dfogocnbecg.JIEOJJEHDDC = 0;
                                dfogocnbecg.HNKLBKNDHMF = 0f;
                                dfogocnbecg.FNKIIDCODDL = 0f;
                            }
                        }
                    }
                    i++;
                }
            }
            if (num > 0 || FFCEGMEAIBP.CBIPLGLDCAG == 1)
            {
  //              Log.LogWarning("PLAYING SONG ON MATCH START");
                PlayMusic();
            }

        }

        [HarmonyPatch(typeof(FFCEGMEAIBP), nameof(FFCEGMEAIBP.EAAIHKLJFCM))]
        [HarmonyPostfix]
        public static void MatchRestart()
        {
  //          Log.LogWarning("PLAYING SONG ON RESTART");
            PlayMusic();
        }

        [HarmonyPatch(typeof(FFCEGMEAIBP), nameof(FFCEGMEAIBP.BAGEPNPJPLD))]
        [HarmonyPostfix]
        public static void MatchEnd()
        {
  //          Log.LogWarning("STOPPING SONG ON MATCH END");
            StopMusic();
        }

        [HarmonyPatch(typeof(FFCEGMEAIBP), nameof(FFCEGMEAIBP.GEGCMDLMJAJ))]
        [HarmonyPrefix]
        public static void SceneChange()
        {
  //          Log.LogWarning("STOPPING SONG ON SCENE CHANGE");
            StopMusic();
        }
    }
}