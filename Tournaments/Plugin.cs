//TODO  optional bye system when draws; 

using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using WECCL.API;

namespace Tournaments
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.Tournaments";
        public const string PluginName = "Tournaments";
        public const string PluginVer = "1.0.2";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        static int TournamentButton;
        static readonly int TournamentNum = -789987;
        public static CustomCard TournamentCard { get; set; } = null;
        public static int OldMenu { get; set; }
        public static int OldDate { get; set; }

        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);


            Buttons.RegisterCustomButton(this, "Reset the Card", () =>
            {
                TournamentCard = null;
                return "Card reset!";
            }, true);
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

        public static bool InCustomMode()
        {
            return NAEEIFNFBBO.CBMHGKFFHJE == TournamentNum || LIPNHOMGGHF.BCKLOCJPIMD == TournamentNum;
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
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "Tournament", -200f, -175f, 1.5f, 1.5f);
                    TournamentButton = LIPNHOMGGHF.HOAOLPGEBKJ;
                    LIPNHOMGGHF.DADOHOENFJJ();
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
                if (LIPNHOMGGHF.NNMDEFLLNBF == TournamentButton)
                {
                    NAEEIFNFBBO.CBMHGKFFHJE = TournamentNum;
                    LIPNHOMGGHF.BCKLOCJPIMD = TournamentNum;
                    OldDate = Progress.date;
                    Progress.date = 0;
                    Progress.focDate = 0;

                    LIPNHOMGGHF.PMIIOCMHEAE(12); //card
                }
            }
        }
        //Scene nav stuff
        [HarmonyPatch(typeof(LIPNHOMGGHF), nameof(LIPNHOMGGHF.PMIIOCMHEAE))]
        [HarmonyPrefix]
        public static void LIPNHOMGGHF_PMIIOCMHEAE_Patch(ref int KBEAJEIMNMI)
        {
            OldMenu = LIPNHOMGGHF.FAKHAFKOBPB;
            //Doing stuff when going match setup -> game
            if (SceneManager.GetActiveScene().name == "Match_Setup" && NAEEIFNFBBO.CBMHGKFFHJE == TournamentNum && KBEAJEIMNMI == 50)
            {
                NAEEIFNFBBO.CBMHGKFFHJE = 0;
                LIPNHOMGGHF.BCKLOCJPIMD = TournamentNum;
                FFCEGMEAIBP.EBMPAEBEMNE = 0;
                return;
            }

            //Doing stuff when exiting match setup
            if (SceneManager.GetActiveScene().name == "Match_Setup" && NAEEIFNFBBO.CBMHGKFFHJE == TournamentNum && KBEAJEIMNMI != 50)
            {
                if (KBEAJEIMNMI != 11)    //char select
                {
                    NAEEIFNFBBO.CBMHGKFFHJE = TournamentNum;
                    LIPNHOMGGHF.BCKLOCJPIMD = TournamentNum;
                    KBEAJEIMNMI = 12;   //card
                }
                else
                {

                }
                return;
            }
            //Doing stuff when exiting card
            if (SceneManager.GetActiveScene().name == "Card" && NAEEIFNFBBO.CBMHGKFFHJE == TournamentNum && KBEAJEIMNMI != 14)
            {
                NAEEIFNFBBO.CBMHGKFFHJE = 0;
                LIPNHOMGGHF.BCKLOCJPIMD = 0;
                Progress.date = OldDate;
                Progress.focDate = OldDate;
                KBEAJEIMNMI = 1;   //titles
                return;
            }

            //Doing stuff when exiting match
            if (SceneManager.GetActiveScene().name == "Game" && LIPNHOMGGHF.BCKLOCJPIMD == TournamentNum)
            {
                CustomSegment segment = TournamentCard.segment[TournamentCard.CurrentSegment] as CustomSegment;
                if (FFCEGMEAIBP.LOBDMDPMFLK != 1)
                {
                    FFCEGMEAIBP.CPAEBBDOGNN = TournamentCard.CurrentSegment;
                    FFCEGMEAIBP.PJFOEJPCLFB();
                    TournamentCard.segment[TournamentCard.CurrentSegment].result = IMNHOCBFGHJ.CIIDDMMENME();
                    segment.UpdateMatchTitle();

                    List<int> winners = new();
                    if (FFCEGMEAIBP.OOODPHNGHGD == 0)   //nobody wins, everyone goes further
                    {
                        for (int i = 1; i <= NJBJIIIACEP.NBBBLJDBLNM; i++)
                        {
                            DFOGOCNBECG dfogocnbecg = NJBJIIIACEP.OAAMGFLINOB[i];
                            if (dfogocnbecg.FIEMGOLBHIO == 1)
                            {
                                winners.Add(dfogocnbecg.GOOKPABIPBC);
                            }
                        }
                    }
                    else
                    {
                        if (FFCEGMEAIBP.OLJFOJOLLOM < 1)    //solo win
                        {
                            winners.Add(NJBJIIIACEP.OAAMGFLINOB[FFCEGMEAIBP.OOODPHNGHGD].GOOKPABIPBC);
                        }
                        else        //team win
                        {
                            for (int i = 1; i <= NJBJIIIACEP.NBBBLJDBLNM; i++)
                            {
                                DFOGOCNBECG dfogocnbecg = NJBJIIIACEP.OAAMGFLINOB[i];
                                if (dfogocnbecg.FIEMGOLBHIO == 1 && dfogocnbecg.LBCFAJGDKJP == NJBJIIIACEP.OAAMGFLINOB[FFCEGMEAIBP.OOODPHNGHGD].LBCFAJGDKJP)
                                {
                                    winners.Add(dfogocnbecg.GOOKPABIPBC);
                                }
                            }
                        }
                    }


                    segment.SendFurther(winners);

                }
                NAEEIFNFBBO.CBMHGKFFHJE = TournamentNum;
                LIPNHOMGGHF.BCKLOCJPIMD = TournamentNum;
                KBEAJEIMNMI = 12;  //card redirect
                return;
            }

        }


        //replacing the vanilla game card class with my own custom class
        [HarmonyPatch(typeof(Card), nameof(Card.PIMGMPBCODM))]
        [HarmonyPrefix]
        public static bool Card_PIMGMPBCODM(ref Card __instance, int DCODOPDDJOA, int EDCMPAAEHFO = 6)
        {
            if (InCustomMode())
            {
                if (TournamentCard == null)
                {
                    __instance = new CustomCard();
                }
                else
                {
                    __instance = TournamentCard;
                }
                TournamentCard = (CustomCard)__instance;
                FFCEGMEAIBP.IOBFNECDOIH = __instance;
                return false;
            }
            return true;
        }
        //Repositioning stuff in the card screen
        [HarmonyPatch(typeof(Scene_Card), nameof(Scene_Card.Start))]
        [HarmonyPostfix]
        public static void Scene_Card_Start(Scene_Card __instance)
        {
            if (InCustomMode())
            {
                __instance.gManual.SetActive(false);        //removing unneeded buttons
                FFCEGMEAIBP.EJBBMLKPNFK[11].SetActive(false);
                FFCEGMEAIBP.EJBBMLKPNFK[4].SetActive(false);
                FFCEGMEAIBP.EJBBMLKPNFK[5].SetActive(false);
                FFCEGMEAIBP.EJBBMLKPNFK[6].SetActive(false);
                Vector3 winnerPos = FFCEGMEAIBP.EJBBMLKPNFK[1].transform.localPosition;
                FFCEGMEAIBP.EJBBMLKPNFK[1].transform.localPosition = new Vector3(FFCEGMEAIBP.EJBBMLKPNFK[1].transform.localPosition.x, FFCEGMEAIBP.EJBBMLKPNFK[2].transform.localPosition.y, FFCEGMEAIBP.EJBBMLKPNFK[1].transform.localPosition.z);

                FFCEGMEAIBP.EJBBMLKPNFK[2].transform.localPosition = new Vector3(FFCEGMEAIBP.EJBBMLKPNFK[2].transform.localPosition.x, FFCEGMEAIBP.EJBBMLKPNFK[4].transform.localPosition.y, FFCEGMEAIBP.EJBBMLKPNFK[2].transform.localPosition.z);
                FFCEGMEAIBP.EJBBMLKPNFK[3].transform.localPosition = new Vector3(FFCEGMEAIBP.EJBBMLKPNFK[3].transform.localPosition.x, FFCEGMEAIBP.EJBBMLKPNFK[4].transform.localPosition.y, FFCEGMEAIBP.EJBBMLKPNFK[3].transform.localPosition.z);


                FFCEGMEAIBP.EJBBMLKPNFK[4].transform.localPosition = new Vector3(1000f, 0f, 0f);
                FFCEGMEAIBP.EJBBMLKPNFK[5].transform.localPosition = new Vector3(0f, -1000f, 0f);
                FFCEGMEAIBP.EJBBMLKPNFK[6].transform.localPosition = new Vector3(-1000f, 0f, 0f);

                LIPNHOMGGHF.DADOHOENFJJ();



                TournamentCard.WinnerObj = GameObject.Instantiate(__instance.gDate, FFCEGMEAIBP.EJBBMLKPNFK[1].transform.parent);
                TournamentCard.WinnerObj.transform.localPosition = winnerPos;
                TournamentCard.WinnerObj.GetComponent<Text>().text = "Winner: " + TournamentCard.Winner;

            }
        }
        //Redirecting to the match setup
        [HarmonyPatch(typeof(Scene_Card), nameof(Scene_Card.Update))]
        [HarmonyPostfix]
        public static void Scene_Card_Update(Scene_Card __instance)
        {
            if (InCustomMode())
            {
                if(LIPNHOMGGHF.PIEMLEPEDFN >= 15)  //moving into match setup
                {
                    TournamentCard.CurrentSegment = LIPNHOMGGHF.NNMDEFLLNBF;
                    LIPNHOMGGHF.PMIIOCMHEAE(14);
                }
            }
        }
        //Fixing the segment display for ready matches
        static bool InDescribeSegmentMethod { get; set; } = false;
        [HarmonyPatch(typeof(Scene_Card), nameof(Scene_Card.DescribeSegment))]
        [HarmonyPrefix]
        public static void Scene_Card_DescribeSegmentPRE(Scene_Card __instance, int CPAEBBDOGNN)
        {
            if (InCustomMode())
            {
                if (TournamentCard.segment[CPAEBBDOGNN].time == 0 && CPAEBBDOGNN <= 3)
                {
                    TournamentCard.segment[CPAEBBDOGNN].time = -1;
                    InDescribeSegmentMethod = true;
                }
            }
        }
        [HarmonyPatch(typeof(Scene_Card), nameof(Scene_Card.DescribeSegment))]
        [HarmonyPostfix]
        public static void Scene_Card_DescribeSegmentPOST(Scene_Card __instance, int CPAEBBDOGNN)
        {
            if (InCustomMode())
            {
                if (InDescribeSegmentMethod == true)
                {
                    TournamentCard.segment[CPAEBBDOGNN].time = 0;
                    InDescribeSegmentMethod = false;
                }
            }
        }
        //Renaming the card name
        [HarmonyPatch(typeof(Scene_Card), nameof(Scene_Card.DescribeContext))]
        [HarmonyPostfix]
        public static void Scene_Card_DescribeContext(Scene_Card __instance, ref string __result)
        {
            if (InCustomMode())
            {
                __result = "Tournament Card:";
            }
        }
        //Renaming the card segments
        [HarmonyPatch(typeof(Card), nameof(Card.LLGKMFDLEJC))]
        [HarmonyPostfix]
        public static void Card_LLGKMFDLEJC(Card __instance, ref string __result, int KJELLNJFNGO)
        {
            if (InCustomMode())
            {
                if (TournamentCard.segment[KJELLNJFNGO].leftName == "" || TournamentCard.segment[KJELLNJFNGO].rightName == "")
                {
                    switch (KJELLNJFNGO)
                    {
                        case 1:
                            __result = "Finals";
                            break;
                        case 2:
                        case 3:
                            __result = "Semi Finals";
                            break;
                        case 4:
                        case 5:
                        case 6:
                            __result = "?????";
                            break;
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                            __result = "Quarter Finals";
                            break;
                    }
                }
                else
                {
                    __result = TournamentCard.segment[KJELLNJFNGO].leftName + " v " + TournamentCard.segment[KJELLNJFNGO].rightName;
                }
            }
        }
        //setting up the match
        [HarmonyPatch(typeof(Scene_Match_Setup), nameof(Scene_Match_Setup.Start))]
        [HarmonyPostfix]
        public static void Scene_Match_Setup_Start_Postfix(Scene_Match_Setup __instance)
        {
            if (InCustomMode())
            {
                if(OldMenu == 12)
                {
                    RemoveOpponents();
                    foreach(int wrestler in ((CustomSegment)TournamentCard.segment[TournamentCard.CurrentSegment]).leftWresters)
                    {
                        AddCharacter(wrestler, 1);
                    }
                    foreach (int wrestler in ((CustomSegment)TournamentCard.segment[TournamentCard.CurrentSegment]).rightWresters)
                    {
                        AddCharacter(wrestler, 2);
                    }
                }
            }
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
        public static void RemoveOpponents()
        {
            for (int i = NJBJIIIACEP.NBBBLJDBLNM; i > 0; i--)
            {
                RemoveCharacter(i);
            }
        }


    }
}