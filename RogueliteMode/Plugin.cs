using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Slobberknocker
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.Slobberknocker";
        public const string PluginName = "Slobberknocker";
        public const string PluginVer = "0.9.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        static int SlobberknockerButton;
        static int SlobberknockerNum = -123654;
        public static int SelectedChar = 0;

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
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "Slobberknocker Mode", 0f, 0f, 1.5f, 1.5f);
                    SlobberknockerButton = LIPNHOMGGHF.HOAOLPGEBKJ;
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
                if (LIPNHOMGGHF.NNMDEFLLNBF == SlobberknockerButton)
                {
                    NAEEIFNFBBO.CBMHGKFFHJE = SlobberknockerNum;
                    LIPNHOMGGHF.BCKLOCJPIMD = SlobberknockerNum;
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
                if (LIPNHOMGGHF.BCKLOCJPIMD == SlobberknockerNum)
                {
                    NAEEIFNFBBO.CBMHGKFFHJE = 0;
                    LIPNHOMGGHF.BCKLOCJPIMD = 0;
                    LIPNHOMGGHF.PMIIOCMHEAE(1);
                }
            }
        }


        //ending after player loses
        [HarmonyPatch(typeof(FFCEGMEAIBP), nameof(FFCEGMEAIBP.NCAAOLGAGCG))]
        [HarmonyPostfix]
        public static void FFCEGMEAIBP_NCAAOLGAGCG_Patch(int KJELLNJFNGO, int GKNIAFAOLJK)
        {
            if(NAEEIFNFBBO.CBMHGKFFHJE == SlobberknockerNum || LIPNHOMGGHF.BCKLOCJPIMD == SlobberknockerNum)
            {
                if (NJBJIIIACEP.OAAMGFLINOB[GKNIAFAOLJK].GOOKPABIPBC == SelectedChar) 
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
            if(SceneManager.GetActiveScene().name == "Match_Setup" && NAEEIFNFBBO.CBMHGKFFHJE == SlobberknockerNum && KBEAJEIMNMI == 50)
            {
                NAEEIFNFBBO.CBMHGKFFHJE = 0;
                LIPNHOMGGHF.BCKLOCJPIMD = SlobberknockerNum;
                AddOpponents(SelectedChar);
                return;
            }

            //Doing stuff when exiting match setup
            if (SceneManager.GetActiveScene().name == "Match_Setup" && NAEEIFNFBBO.CBMHGKFFHJE == SlobberknockerNum && KBEAJEIMNMI != 50)
            {
                NAEEIFNFBBO.CBMHGKFFHJE = 0;
                LIPNHOMGGHF.BCKLOCJPIMD = 0;
                KBEAJEIMNMI = 1;
                return;
            }

            //Doing stuff when exiting match
            if (SceneManager.GetActiveScene().name == "Game" && LIPNHOMGGHF.BCKLOCJPIMD == SlobberknockerNum)
            {
                NAEEIFNFBBO.CBMHGKFFHJE = 0;
                LIPNHOMGGHF.BCKLOCJPIMD = 0;
          //      NJBJIIIACEP.NBBBLJDBLNM = 1;
           //     FFCEGMEAIBP.EHIDHAPMAKG = 1;
                KBEAJEIMNMI = 1;
                return;
            }

        }
        //disable interference and simulate buttons;
        [HarmonyPatch(typeof(LIPNHOMGGHF), nameof(LIPNHOMGGHF.OPBBKBAJJHA))]
        [HarmonyPostfix]
        public static void LIPNHOMGGHF_OPBBKBAJJHA_Patch()
        {
            if (NAEEIFNFBBO.CBMHGKFFHJE == SlobberknockerNum || LIPNHOMGGHF.BCKLOCJPIMD == SlobberknockerNum)
            {
                if (LIPNHOMGGHF.ODOAPLMOJPD == 0)
                {
                    LIPNHOMGGHF.FKANHDIMMBJ[3].AHBNKMMMGFI = 0;
                    LIPNHOMGGHF.FKANHDIMMBJ[4].AHBNKMMMGFI = 0;
                }
            }
        }
        //disable tabs in the match setup
        [HarmonyPatch(typeof(Scene_Match_Setup), nameof(Scene_Match_Setup.Update))]
        [HarmonyPostfix]
        public static void Scene_Match_Setup_Update_Patch()
        {
            if (NAEEIFNFBBO.CBMHGKFFHJE == SlobberknockerNum)
            {
                if (LIPNHOMGGHF.CHLJMEPFJOK == 2 || LIPNHOMGGHF.CHLJMEPFJOK == 4)
                {
                    for (int i = 1; i <= LIPNHOMGGHF.HOAOLPGEBKJ; i++)
                    {
                        LIPNHOMGGHF.FKANHDIMMBJ[i].AHBNKMMMGFI = 0;
                    }
                }
                if (LIPNHOMGGHF.CHLJMEPFJOK == 3)
                {
                    NJBJIIIACEP.OAAMGFLINOB[1].AHBNKMMMGFI = 0;
                }
                else
                {
                    NJBJIIIACEP.OAAMGFLINOB[1].AHBNKMMMGFI = 1;
                }
            }
        }
        //Set up the selected character
        public static void RemoveOpponents()
        {
            for (int i = NJBJIIIACEP.NBBBLJDBLNM; i > 0; i--)
            {
                RemoveCharacter(i);
            }
        }
        [HarmonyPatch(typeof(Scene_Match_Setup), nameof(Scene_Match_Setup.Start))]
        [HarmonyPostfix]
        public static void Scene_Match_Setup_Start_Postfix(Scene_Match_Setup __instance)
        {
            if (NAEEIFNFBBO.CBMHGKFFHJE == SlobberknockerNum || LIPNHOMGGHF.BCKLOCJPIMD == SlobberknockerNum)
            {
                RemoveOpponents();
                AddCharacter(SelectedChar, 0);
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
            int j = 0;
            for (int i = 1; i <= NAEEIFNFBBO.ILLMCDIFFON - 1; i++)
            {
                if (chars[j] == playerid)
                {
                    j++;
                }
                int n = AddCharacter(chars[j], 0);
                FFCEGMEAIBP.COIGEGPKLCP[NJBJIIIACEP.OAAMGFLINOB[n].PLFGKLGCOMD] = -1;
                j++;

            }

        }
        //disable cast tab buttons
        [HarmonyPatch(typeof(Scene_Match_Setup), nameof(Scene_Match_Setup.AddRandom))]
        [HarmonyPrefix]
        public static bool Scene_Match_Setup_AddRandom(Scene_Match_Setup __instance)
        {
            if (NAEEIFNFBBO.CBMHGKFFHJE == SlobberknockerNum || LIPNHOMGGHF.BCKLOCJPIMD == SlobberknockerNum)
            {
                return false;
            }
            return true;
        }
        [HarmonyPatch(typeof(Scene_Match_Setup), nameof(Scene_Match_Setup.RemoveCast))]
        [HarmonyPrefix]
        public static bool Scene_Match_Setup_RemoveCast(Scene_Match_Setup __instance,  int GKNIAFAOLJK)
        {
            if (NAEEIFNFBBO.CBMHGKFFHJE == SlobberknockerNum || LIPNHOMGGHF.BCKLOCJPIMD == SlobberknockerNum)
            {
                return false;
            }
            return true;
        }
        public static void RemoveCharacter(int id)
        {
            Scene_Match_Setup code = UnityEngine.GameObject.Find("Code").GetComponent<Scene_Match_Setup>();
            Debug.Log("Attempting to remove P" + id.ToString());
            if (id == 0)
            {
                for (int i = 1; i <= NJBJIIIACEP.NBBBLJDBLNM; i++)
                {
                    if (NJBJIIIACEP.OAAMGFLINOB[i].AHBNKMMMGFI > 0f)
                    {
                        id = i;
                    }
                }
            }
            if (id > 0)
            {
                Debug.Log("Deactivating P" + id.ToString());
            //    CHLPMKEGJBJ.DNNPEAOCDOG(CHLPMKEGJBJ.AKLKPBJBEBK, 1f, 1f);
                DFOGOCNBECG dfogocnbecg = NJBJIIIACEP.OAAMGFLINOB[id];
                dfogocnbecg.AHBNKMMMGFI = -1f;
                dfogocnbecg.PCNHIIPBNEK[0].SetActive(false);
                if (dfogocnbecg.IFOCOECLBAF != null)
                {
                    dfogocnbecg.IFOCOECLBAF.SetActive(false);
                }
                FFCEGMEAIBP.NMMABDGIJNC[id] = -id;
                if (code.castFoc == id)
                {
                    code.castFoc = 0;
                }
                if (code.moveFoc == id)
                {
                    code.moveFoc = 0;
                }
                for (int j = 0; j <= HKJOAJOKOIJ.NGCNKGDDKGF; j++)
                {
                    if (HKJOAJOKOIJ.NAADDLFFIHG[j].AHBNKMMMGFI > 0 && HKJOAJOKOIJ.NAADDLFFIHG[j].FOAPDJMIFGP == id)
                    {
                        HKJOAJOKOIJ.NAADDLFFIHG[j].FOAPDJMIFGP = 0;
                    }
                }
            }
        }
        public static int AddCharacter(int id, int team)
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
        //    CHLPMKEGJBJ.DNNPEAOCDOG(CHLPMKEGJBJ.DOIEFBGOCPA, 1f, 1f);
         //   CHLPMKEGJBJ.DNNPEAOCDOG(CHLPMKEGJBJ.JMBMELDCIDA[1], 1f, 0.75f);
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
            return code.n;

        }


/*        //fixing the character controls
        [HarmonyPatch(typeof(Scene_Game), nameof(Scene_Game.Start))]
        [HarmonyPostfix]
        public static void Scene_Game_Start()
        {
            if (NAEEIFNFBBO.CBMHGKFFHJE == SlobberknockerNum || LIPNHOMGGHF.BCKLOCJPIMD == SlobberknockerNum)
            {
                for (int i = 1; i < NJBJIIIACEP.OAAMGFLINOB.Length; i++)
                {
                    UnityEngine.Debug.LogWarning(i + " " + NJBJIIIACEP.OAAMGFLINOB[i].EMDMDLNJFKP.name + " " + NJBJIIIACEP.OAAMGFLINOB[i].NLOOBNDGIKO.BPJFLJPKKJK + " " + NJBJIIIACEP.OAAMGFLINOB[i].NLOOBNDGIKO.CMECDGMCMLC);
                    if (NJBJIIIACEP.OAAMGFLINOB[i].GOOKPABIPBC == SelectedChar)
                    {

                       // NJBJIIIACEP.OAAMGFLINOB[i].NLOOBNDGIKO.FOAPDJMIFGP = 1;
                    }
                    else
                    {
                              Debug.LogWarning("Disabling " + i + " " + NJBJIIIACEP.OAAMGFLINOB[i].EMDMDLNJFKP.name + " " + NJBJIIIACEP.OAAMGFLINOB[i].NLOOBNDGIKO.BPJFLJPKKJK + " " + NJBJIIIACEP.OAAMGFLINOB[i].NLOOBNDGIKO.CMECDGMCMLC);

                              NJBJIIIACEP.OAAMGFLINOB[i].NLOOBNDGIKO.FOAPDJMIFGP = 0;
                              NJBJIIIACEP.OAAMGFLINOB[i].OJAJENJLBMF = -1;
                              NJBJIIIACEP.OAAMGFLINOB[i].NLOOBNDGIKO = HKJOAJOKOIJ.IPDFOJEMPMM;
                        NJBJIIIACEP.OAAMGFLINOB[i].OJAJENJLBMF = -1;
                        FFCEGMEAIBP.COIGEGPKLCP[NJBJIIIACEP.OAAMGFLINOB[i].PLFGKLGCOMD] = -1;
                    }
                }

            }
        }*/


        //Setting up the char selection button redirect
        [HarmonyPatch(typeof(Scene_Select_Char), nameof(Scene_Select_Char.EGPFEGLDMJM))]
        [HarmonyPostfix]
        public static void Scene_Select_Char_EGPFEGLDMJM_Patch(Scene_Select_Char __instance, int GOOKPABIPBC)
        {
            if (LIPNHOMGGHF.BCKLOCJPIMD == SlobberknockerNum)
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