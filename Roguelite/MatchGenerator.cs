using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Roguelite
{
    public static class MatchGenerator
    {
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

        public static void SetupMatchRules()
        {
            FFCEGMEAIBP.JELMGJMKKEK(3);
        }
    }
}
