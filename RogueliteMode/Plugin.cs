//note disable playable check; disable additional controllers -> go through all chars, set them all to ai;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Mono.Cecil.Cil;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RogueliteMode
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.RogueliteMode";
        public const string PluginName = "RogueliteMode";
        public const string PluginVer = "0.0.1";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        static int RogueliteButton;
        static int RogueliteModeNum = 123654;
        public static int SelectedChar = 0;

  /*      static int[] origArray0;
        static int[] origArray1;
        static int[] origArray2;
        static int[] origArray3;
        static DFOGOCNBECG[] origArray4;*/

        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);



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

        //adding new button to the main menu
        [HarmonyPatch(typeof(LIPNHOMGGHF), nameof(LIPNHOMGGHF.ICGNAJFLAHL))]
        [HarmonyPostfix]
        public static void ICGNAJFLAHL_Patch()
        {
            if (LIPNHOMGGHF.FAKHAFKOBPB == 1)
            {
                if (LIPNHOMGGHF.ODOAPLMOJPD == 1)
                {
                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "Roguelite Mode", 0f, 0f, 1.5f, 1.5f);
                    RogueliteButton = LIPNHOMGGHF.HOAOLPGEBKJ;
                }
            }
        }


        //Setting up the main menu button redirect
        [HarmonyPatch(typeof(Scene_Titles), nameof(Scene_Titles.Update))]
        [HarmonyPostfix]
        public static void Scene_Titles_Update_Patch()
        {
            if (LIPNHOMGGHF.PIEMLEPEDFN >= 5 && LIPNHOMGGHF.ODOAPLMOJPD == 1 && NAEEIFNFBBO.EKFJCKLKELN != 1)
            {
                if (LIPNHOMGGHF.NNMDEFLLNBF == RogueliteButton)
                {
                    NAEEIFNFBBO.CBMHGKFFHJE = RogueliteModeNum;
                    LIPNHOMGGHF.BCKLOCJPIMD = RogueliteModeNum;
                    LIPNHOMGGHF.PMIIOCMHEAE(11);
                }
            }
        }




        //Setting up the char select back button redirect
        [HarmonyPatch(typeof(Scene_Select_Char), nameof(Scene_Select_Char.Update))]
        [HarmonyPostfix]
        public static void Scene_Select_Char_Update_Patch()
        {
            if (LIPNHOMGGHF.PIEMLEPEDFN <= -5)
            {
                if (LIPNHOMGGHF.BCKLOCJPIMD == RogueliteModeNum)
                {
                    LIPNHOMGGHF.PMIIOCMHEAE(1);
                }
            }
        }


        //ending after player loses
        [HarmonyPatch(typeof(FFCEGMEAIBP), nameof(FFCEGMEAIBP.NCAAOLGAGCG))]
        [HarmonyPostfix]
        public static void FFCEGMEAIBP_NCAAOLGAGCG_Patch(int KJELLNJFNGO, int GKNIAFAOLJK)
        {
            if(NAEEIFNFBBO.CBMHGKFFHJE == RogueliteModeNum || LIPNHOMGGHF.BCKLOCJPIMD == RogueliteModeNum)
            {
                if(GKNIAFAOLJK == 1) //TODO Replace with selected char id
                {
                    FFCEGMEAIBP.BAGEPNPJPLD(KJELLNJFNGO);
                }
            }
        }

        //Scene nav stuff
        [HarmonyPatch(typeof(LIPNHOMGGHF), nameof(LIPNHOMGGHF.PMIIOCMHEAE))]
        [HarmonyPrefix]
        public static void LIPNHOMGGHF_PMIIOCMHEAE_Patch(ref int KBEAJEIMNMI)
        {
            //Doing stuff when going match setup -> game
            if(SceneManager.GetActiveScene().name == "Match_Setup" && NAEEIFNFBBO.CBMHGKFFHJE == RogueliteModeNum && KBEAJEIMNMI == 50)
            {
                NAEEIFNFBBO.CBMHGKFFHJE = 0;
                LIPNHOMGGHF.BCKLOCJPIMD = RogueliteModeNum;
                AddOpponents(SelectedChar);
                Debug.LogWarning(NJBJIIIACEP.NBBBLJDBLNM);
                return;
            }

            //Doing stuff when exiting match setup
            if (SceneManager.GetActiveScene().name == "Match_Setup" && NAEEIFNFBBO.CBMHGKFFHJE == RogueliteModeNum && KBEAJEIMNMI != 50)
            {
                NAEEIFNFBBO.CBMHGKFFHJE = 0;
                LIPNHOMGGHF.BCKLOCJPIMD = 0;
                KBEAJEIMNMI = 1;
                return;
            }

            //Doing stuff when exiting match
            if (SceneManager.GetActiveScene().name == "Game" && LIPNHOMGGHF.BCKLOCJPIMD == RogueliteModeNum)
            {
                NAEEIFNFBBO.CBMHGKFFHJE = RogueliteModeNum;
                LIPNHOMGGHF.BCKLOCJPIMD = 0;
                NJBJIIIACEP.NBBBLJDBLNM = 1;
                FFCEGMEAIBP.EHIDHAPMAKG = 1;
                KBEAJEIMNMI = 1;
                return;
            }
        }

        //disable interference and simulate buttons;
        [HarmonyPatch(typeof(LIPNHOMGGHF), nameof(LIPNHOMGGHF.OPBBKBAJJHA))]
        [HarmonyPostfix]
        public static void LIPNHOMGGHF_OPBBKBAJJHA_Patch()
        {
            if (NAEEIFNFBBO.CBMHGKFFHJE == RogueliteModeNum || LIPNHOMGGHF.BCKLOCJPIMD == RogueliteModeNum)
            {
                LIPNHOMGGHF.FKANHDIMMBJ[3].AHBNKMMMGFI = 0;
                LIPNHOMGGHF.FKANHDIMMBJ[4].AHBNKMMMGFI = 0;
            }
        }
        //disable tabs in the match setup
        [HarmonyPatch(typeof(Scene_Match_Setup), nameof(Scene_Match_Setup.Update))]
        [HarmonyPostfix]
        public static void Scene_Match_Setup_Update_Patch()
        {
            if (NAEEIFNFBBO.CBMHGKFFHJE == RogueliteModeNum)
            {
                if (LIPNHOMGGHF.CHLJMEPFJOK == 2 || LIPNHOMGGHF.CHLJMEPFJOK == 4)
                {
                    for (int i = 1; i <= LIPNHOMGGHF.HOAOLPGEBKJ; i++)
                    {
                        LIPNHOMGGHF.FKANHDIMMBJ[i].AHBNKMMMGFI = 0;
                    }
                }
            }
        }
        //disable adding/removing characters in the menu

        //Set up the selected character
        public static void RemoveOpponents()
        {
            Scene_Match_Setup code = UnityEngine.GameObject.Find("Code").GetComponent<Scene_Match_Setup>();
            for (int i = NJBJIIIACEP.NBBBLJDBLNM; i > 0; i--)
            {
                code.RemoveCast(i);
            }
        }
        //set up opponents
        public static void AddOpponents(int playerid)
        {
            int[] chars = new int[Characters.no_chars];
            for (int i = 1; i <= chars.Length; i++)
                chars[i-1] = i;
            var rng = new System.Random();
            rng.Shuffle(chars);
            int j = 2;
            for (int i = 1; i <= NAEEIFNFBBO.ILLMCDIFFON - 1; i++)
            {
                if (chars[i-1] != playerid)
                {
                    AddCharacter(chars[i - 1], 0);
                    j++;
                }
            }

        }

        [HarmonyPatch(typeof(Scene_Match_Setup), nameof(Scene_Match_Setup.Start))]
        [HarmonyPostfix]
        public static void Scene_Match_Setup_Start_Postfix(Scene_Match_Setup __instance)
        {
            if (NAEEIFNFBBO.CBMHGKFFHJE == RogueliteModeNum || LIPNHOMGGHF.BCKLOCJPIMD == RogueliteModeNum)
            {
                RemoveOpponents();
                AddCharacter(SelectedChar, 0);
            }
        }
        public static void AddCharacter(int id, int team)
        {
            Scene_Match_Setup code = UnityEngine.GameObject.Find("Code").GetComponent<Scene_Match_Setup>();
            code.n = 0;
            code.v = 1;
            while (code.v <= NJBJIIIACEP.NBBBLJDBLNM)
            {
                if (NJBJIIIACEP.OAAMGFLINOB[code.v].AHBNKMMMGFI <= 0f)
                {
                    Debug.Log("Overwriting slot " + code.v.ToString() + " to add new");
                    code.n = code.v;
                    break;
                }
                code.v++;
            }
            if (code.n == 0 && FFCEGMEAIBP.EHIDHAPMAKG < NAEEIFNFBBO.ILLMCDIFFON && FFCEGMEAIBP.EHIDHAPMAKG < NJBJIIIACEP.KLDJKHPCDHM)
            {
                code.n = FFCEGMEAIBP.EHIDHAPMAKG + 1;
                Debug.Log("Increasing cast to " + FFCEGMEAIBP.EHIDHAPMAKG.ToString() + " to add new");
            }
            CHLPMKEGJBJ.DNNPEAOCDOG(CHLPMKEGJBJ.DOIEFBGOCPA, 1f, 1f);
            CHLPMKEGJBJ.DNNPEAOCDOG(CHLPMKEGJBJ.JMBMELDCIDA[1], 1f, 0.75f);
            if (FFCEGMEAIBP.EHIDHAPMAKG < 1)
            {
                FFCEGMEAIBP.EHIDHAPMAKG = 1;
                Debug.Log("Increasing castSize from 0 to 1");
            }
            if (code.n > FFCEGMEAIBP.EHIDHAPMAKG)
            {
                FFCEGMEAIBP.EHIDHAPMAKG = code.n;
                Debug.Log("Increasing castSize to " + code.n.ToString());
            }
            int num = id;
            int num2 = 1;
            int num3 = team;
            FFCEGMEAIBP.NMMABDGIJNC[code.n] = num;
            FFCEGMEAIBP.DJCDPNPLICD[code.n] = num2;
            FFCEGMEAIBP.EKKIPMFPMEE[code.n] = num3;
            int num4 = 0;
            int num5;
            do
            {
                FFCEGMEAIBP.AJMAFHIBCGJ[code.n] = (float)UnityEngine.Random.Range(-20, 20) * World.ringSize;
                if (num3 == 1)
                {
                    FFCEGMEAIBP.AJMAFHIBCGJ[code.n] = (float)UnityEngine.Random.Range(-20, -5) * World.ringSize;
                }
                if (num3 == 2)
                {
                    FFCEGMEAIBP.AJMAFHIBCGJ[code.n] = (float)UnityEngine.Random.Range(5, 20) * World.ringSize;
                }
                FFCEGMEAIBP.MHHLHMDOFBP[code.n] = (float)UnityEngine.Random.Range(-20, 20) * World.ringSize;
                num4++;
                num5 = 1;
                if (Mathf.Abs(FFCEGMEAIBP.AJMAFHIBCGJ[code.n]) < 5f && FFCEGMEAIBP.MHHLHMDOFBP[code.n] > 0f && NAEEIFNFBBO.BNCCMMLOIML > 0)
                {
                    if (num2 != 3)
                    {
                        num5 = 0;
                    }
                }
                else if (num2 == 3)
                {
                    num5 = 0;
                }
                for (int i = 1; i <= NJBJIIIACEP.NBBBLJDBLNM; i++)
                {
                    if (NJBJIIIACEP.OAAMGFLINOB[i].AHBNKMMMGFI > 0f && NAEEIFNFBBO.FHPCDHIGILG(FFCEGMEAIBP.AJMAFHIBCGJ[code.n], FFCEGMEAIBP.MHHLHMDOFBP[code.n], NJBJIIIACEP.OAAMGFLINOB[i].NJDGEELLAKG, NJBJIIIACEP.OAAMGFLINOB[i].BMFDFFLPBOJ) < 5f)
                    {
                        num5 = 0;
                    }
                }
            }
            while (num4 < 100 && num5 == 0);
            Debug.Log("Assigning character " + FFCEGMEAIBP.NMMABDGIJNC[code.n].ToString() + " to slot " + code.n.ToString());
            if (code.n <= NJBJIIIACEP.NBBBLJDBLNM)
            {
                NJBJIIIACEP.IIACAIINEKD(code.n, id, 1);
            }
            else
            {
                NJBJIIIACEP.DFLLBNMHHIH(id, 1);
            }

        }
        //Setting up the char selection button redirect
        [HarmonyPatch(typeof(Scene_Select_Char), nameof(Scene_Select_Char.EGPFEGLDMJM))]
        [HarmonyPostfix]
        public static void Scene_Select_Char_EGPFEGLDMJM_Patch(Scene_Select_Char __instance, int GOOKPABIPBC)
        {
            if (LIPNHOMGGHF.BCKLOCJPIMD == RogueliteModeNum)
            {
                SelectedChar = GOOKPABIPBC;

                FFCEGMEAIBP.JELMGJMKKEK(22);


                   FFCEGMEAIBP.LCCCCENGFOK = 4;
                   FFCEGMEAIBP.JPBHIEOKODO = 0;
                   FFCEGMEAIBP.BPJFLJPKKJK = 5;
                   FFCEGMEAIBP.CADLONHABMC = 2;
                   FFCEGMEAIBP.OLJFOJOLLOM = -1;
                   FFCEGMEAIBP.LGHMLHICAFL = 2;
                   FFCEGMEAIBP.DOLNEDHNKMM = 0;
                   FFCEGMEAIBP.GDKCEGBINCM = 2;
                   FFCEGMEAIBP.NBAFIEALMHN = 0;
                   FFCEGMEAIBP.JMBGHDFADHN = -1;

                FFCEGMEAIBP.OHBEGHIIHJB = 0;
                FFCEGMEAIBP.LOBDMDPMFLK = 1;

              //  NAEEIFNFBBO.CBMHGKFFHJE = 0;//?????????
                FFCEGMEAIBP.EBMPAEBEMNE = 0;
                FFCEGMEAIBP.AEKLGCEFIHM = 0;


                //insert sound here
                LIPNHOMGGHF.PMIIOCMHEAE(14);

            }
        }
        public static void Resize<T>(ref T[] original, int newsize)
        {
            T[] newArray = new T[newsize];
            int copysize;
            if (original.Length > newsize)
            {
                copysize = newsize;
            }
            else
            {
                copysize = original.Length;
            }
            Array.Copy(original, newArray, copysize);
            original = newArray;
        }
    }
    static class RandomExtensions
    {
        public static void Shuffle<T>(this System.Random rng, T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }
}