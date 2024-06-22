using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

namespace MatchSetup
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.MatchSetup";
        public const string PluginName = "MatchSetup";
        public const string PluginVer = "1.0.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;
        public static ConfigEntry<bool> BookerManagersConfig;


        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);


            BookerManagersConfig = Config.Bind("General",
             "Booker Managers",
             true,
             "If disabled, the bookers will no longer be added as managers in cross promotion matches");
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
        public static void SetupMatch(int[] ids, int[] teams)
        {
            RemoveCharacters();
            for (int i = 0; i < ids.Length; i++)
            {
                AddCharacter(ids[i], teams[i]);
            }
        }
        [HarmonyPatch(typeof(NJBJIIIACEP), nameof(NJBJIIIACEP.LACGAAEHEOL))]
        [HarmonyPrefix]
        static bool NJBJIIIACEP_LACGAAEHEOL()
        {
            if(BookerManagersConfig.Value == true)
            {
                return true;
            }
            else
            {
                NoBookerManagers();
                return false;
            }
        }
        public static void NoBookerManagers()
        {
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 1; j <= NJBJIIIACEP.NBBBLJDBLNM; j++)
                {
                    Roster roster = Characters.fedData[NJBJIIIACEP.OAAMGFLINOB[j].EMDMDLNJFKP.fed];
                    int num = 0;
                    if (i >= 1 && i <= 2)
                    {
                        num = NJBJIIIACEP.OAAMGFLINOB[j].EMDMDLNJFKP.relationship[i];
                    }
                    if (Characters.c[num].fed != NJBJIIIACEP.OAAMGFLINOB[j].EMDMDLNJFKP.fed)
                    {
                        num = 0;
                    }
                    if (LIPNHOMGGHF.FAKHAFKOBPB == 70 && j <= 2)
                    {
                        if (i == 0 && NJBJIIIACEP.OAAMGFLINOB[j].GOOKPABIPBC == NEGAFEHECNL.FEAIGHFCIBK)
                        {
                            if (NEGAFEHECNL.GOEACIHJCCJ == 806 || (NEGAFEHECNL.GOEACIHJCCJ >= 1508 && NEGAFEHECNL.GOEACIHJCCJ <= 1510) || NEGAFEHECNL.GOEACIHJCCJ == 1606 || NEGAFEHECNL.GOEACIHJCCJ == 1609 || NEGAFEHECNL.GOEACIHJCCJ == 1616)
                            {
                                num = NEGAFEHECNL.MKEBAFANECN;
                            }
                            if (NEGAFEHECNL.GOEACIHJCCJ == 1446 || NEGAFEHECNL.GOEACIHJCCJ == 1447)
                            {
                                num = roster.champ[1, 1];
                            }
                        }
                        if (i == 0 && NJBJIIIACEP.OAAMGFLINOB[j].GOOKPABIPBC == Characters.star)
                        {
                            if (NEGAFEHECNL.GOEACIHJCCJ == 805 || NEGAFEHECNL.GOEACIHJCCJ == 1590)
                            {
                                num = NEGAFEHECNL.MKEBAFANECN;
                            }
                            if (NEGAFEHECNL.GOEACIHJCCJ == 1446 || NEGAFEHECNL.GOEACIHJCCJ == 1447)
                            {
                                num = roster.champ[1, 1];
                            }
                        }
                        if (i == 2 && num == 0 && NEGAFEHECNL.GOEACIHJCCJ >= 1500 && NEGAFEHECNL.GOEACIHJCCJ < 1600 && NJBJIIIACEP.OAAMGFLINOB[1].EMDMDLNJFKP.fed != NJBJIIIACEP.OAAMGFLINOB[2].EMDMDLNJFKP.fed && (NJBJIIIACEP.OAAMGFLINOB[j].EMDMDLNJFKP.contract > 0 || NJBJIIIACEP.OAAMGFLINOB[j].EMDMDLNJFKP.fed != Characters.school))
                        {
                            num = roster.booker;
                        }
                    }
                    if (NAEEIFNFBBO.CBMHGKFFHJE > 0 && Characters.c[num].absent != 0)
                    {
                        num = 0;
                    }
                    if (NAEEIFNFBBO.CCHJELGOHLG == 0 && (num != Characters.star || NAEEIFNFBBO.CBMHGKFFHJE == 0))
                    {
                        num = 0;
                    }
                    if (num > 0 && num != NJBJIIIACEP.OAAMGFLINOB[j].GOOKPABIPBC && NJBJIIIACEP.NBBBLJDBLNM < NAEEIFNFBBO.ILLMCDIFFON && (NJBJIIIACEP.OAAMGFLINOB[j].FIEMGOLBHIO == 1 || (LIPNHOMGGHF.FAKHAFKOBPB == 70 && j <= 2)) && NJBJIIIACEP.OCHONNCHPEK(num) == 0)
                    {
                        int num2 = NJBJIIIACEP.DFLLBNMHHIH(num, 2);
                        if (num2 > 0)
                        {
                            Debug.Log(string.Concat(new string[]
                            {
                            "Added ",
                            NJBJIIIACEP.OAAMGFLINOB[num2].EMDMDLNJFKP.name,
                            " as manager for ",
                            NJBJIIIACEP.OAAMGFLINOB[j].EMDMDLNJFKP.name,
                            " active=",
                            NJBJIIIACEP.OAAMGFLINOB[num2].AHBNKMMMGFI.ToString()
                            }));
                            NJBJIIIACEP.OAAMGFLINOB[num2].AHBNKMMMGFI = NJBJIIIACEP.OAAMGFLINOB[j].AHBNKMMMGFI;
                            NJBJIIIACEP.OAAMGFLINOB[num2].NNMDEFLLNBF = NJBJIIIACEP.OAAMGFLINOB[j].NNMDEFLLNBF;
                            NJBJIIIACEP.OAAMGFLINOB[num2].LBCFAJGDKJP = NJBJIIIACEP.OAAMGFLINOB[j].LBCFAJGDKJP;
                            NJBJIIIACEP.OAAMGFLINOB[num2].FHMLNCBCALP = j;
                            if (NJBJIIIACEP.OAAMGFLINOB[j].DEMGOJPEHLJ == 0)
                            {
                                NJBJIIIACEP.OAAMGFLINOB[j].DEMGOJPEHLJ = num2;
                            }
                            if (NJBJIIIACEP.OAAMGFLINOB[num2].AHBNKMMMGFI == 0f)
                            {
                                NJBJIIIACEP.OAAMGFLINOB[num2].PCNHIIPBNEK[0].SetActive(false);
                                NJBJIIIACEP.OAAMGFLINOB[num2].NLOOBNDGIKO.NNAGIMAACLN = 0f;
                                NJBJIIIACEP.OAAMGFLINOB[num2].NELODEMHJHN = 0;
                                NJBJIIIACEP.OAAMGFLINOB[num2].NLDPMDNKGIC = 0;
                            }
                            if (LIPNHOMGGHF.FAKHAFKOBPB == 70 && NJBJIIIACEP.OAAMGFLINOB[num2].AHBNKMMMGFI > 0f)
                            {
                                int num3 = 0;
                                int num6;
                                do
                                {
                                    float num4 = NJBJIIIACEP.OAAMGFLINOB[j].MPFFANIIEDG + (float)NAEEIFNFBBO.PMEEFNOLAGF(115, 245, 0);
                                    float num5 = UnityEngine.Random.Range(5f, 10f);
                                    if (NJBJIIIACEP.OAAMGFLINOB[j].OHLENIHCHAP > 0)
                                    {
                                        num5 += 3f;
                                    }
                                    if (World.location == 20)
                                    {
                                        num5 *= 0.75f;
                                    }
                                    NJBJIIIACEP.OAAMGFLINOB[num2].NJDGEELLAKG = NJBJIIIACEP.OAAMGFLINOB[j].NJDGEELLAKG + NAEEIFNFBBO.PDOBPEFCMCK(num4, num5);
                                    NJBJIIIACEP.OAAMGFLINOB[num2].BMFDFFLPBOJ = NJBJIIIACEP.OAAMGFLINOB[j].BMFDFFLPBOJ + NAEEIFNFBBO.GPMMBFPCFFL(num4, num5);
                                    NJBJIIIACEP.OAAMGFLINOB[num2].FNNBCDPJBIO = World.KJOEBADBOME(NJBJIIIACEP.OAAMGFLINOB[num2].NJDGEELLAKG, NJBJIIIACEP.OAAMGFLINOB[j].FNNBCDPJBIO, NJBJIIIACEP.OAAMGFLINOB[num2].BMFDFFLPBOJ);
                                    NJBJIIIACEP.OAAMGFLINOB[num2].MPFFANIIEDG = NJBJIIIACEP.OAAMGFLINOB[j].MPFFANIIEDG;
                                    num6 = 1;
                                    num3++;
                                    for (int k = 1; k <= NJBJIIIACEP.NBBBLJDBLNM; k++)
                                    {
                                        if (k != num2 && NAEEIFNFBBO.FHPCDHIGILG(NJBJIIIACEP.OAAMGFLINOB[num2].NJDGEELLAKG, NJBJIIIACEP.OAAMGFLINOB[num2].BMFDFFLPBOJ, NJBJIIIACEP.OAAMGFLINOB[k].NJDGEELLAKG, NJBJIIIACEP.OAAMGFLINOB[k].BMFDFFLPBOJ) < 5f)
                                        {
                                            num6 = 0;
                                        }
                                    }
                                }
                                while (num3 < 100 && num6 == 0);
                            }
                            Debug.Log("Active=" + NJBJIIIACEP.OAAMGFLINOB[num2].AHBNKMMMGFI.ToString() + " Control=" + NJBJIIIACEP.OAAMGFLINOB[num2].OJAJENJLBMF.ToString());
                        }
                    }
                }
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
        public static void RemoveCharacters()
        {
            Scene_Match_Setup code = UnityEngine.GameObject.Find("Code").GetComponent<Scene_Match_Setup>();
            for (int i = NJBJIIIACEP.NBBBLJDBLNM; i > 0; i--)
            {
                code.RemoveCast(i);
            }
        }
    }
}