using System;
using System.Collections.Generic;
using System.Text;
using CollectibleCards2;
using HarmonyLib;
using Steamworks;
using UnityEngine;
using UnityEngine.TextCore;

namespace CardGame
{
    [HarmonyPatch]
    public  class LobbyMenu
    {
        public static int LobbyButton { get; set; }

        public static int HostTypeButton { get; set; }
        public static int HostLobbyButton { get; set; }
        public static int LobbyIDButton { get; set; }
        public static int InviteFriendsButton { get; set; }
        public static int IDToJoinButton { get; set; }
        public static int JoinButton { get; set; }
        public static int SendCardButton { get; set; }


        public static int HostType { get; set; } = 0;
        public static string LobbyID { get; set; } = "";
        public static string IDToJoin { get; set; }



        //disabling annoying audio
        [HarmonyPatch(typeof(CHLPMKEGJBJ), nameof(CHLPMKEGJBJ.DNNPEAOCDOG))]
        [HarmonyPrefix]
        public static bool CHLPMKEGJBJ_DNNPEAOCDOG_Prefix(AudioClip GGMBIAAEMKO, float ELJKCOHGBBD = 0f, float CDNNGHGFALM = 1f)
        {
            if (GGMBIAAEMKO == CHLPMKEGJBJ.PAJJMPLBDPL && LIPNHOMGGHF.ODOAPLMOJPD == Plugin.MPLobbyPage)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        //adding buttons
        [HarmonyPatch(typeof(LIPNHOMGGHF), nameof(LIPNHOMGGHF.ICGNAJFLAHL))]
        [HarmonyPostfix]
        public static void ICGNAJFLAHL_Patch()
        {
            if (LIPNHOMGGHF.FAKHAFKOBPB == 1)
            {
                if (LIPNHOMGGHF.ODOAPLMOJPD == CollectibleCards2.Plugin.CardsMenuPage)
                {
                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "MP Lobby", 350f, -150f, 1.5f, 1.5f);
                    LobbyButton = LIPNHOMGGHF.HOAOLPGEBKJ;
                }
                if (LIPNHOMGGHF.ODOAPLMOJPD == Plugin.MPLobbyPage)
                {

                    LobbyID = "";
                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(2, "Lobby Type", -450f, 150f, 1.25f, 1.25f);
                    HostTypeButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "Host Lobby", -150f, 150f, 1.25f, 1.25f);
                    HostLobbyButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(2, "Lobby ID", -450f, 50f, 1.25f, 1.25f);
                    LobbyIDButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "Invite Friends", -150f, 50f, 1.25f, 1.25f);
                    InviteFriendsButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(2, "ID To Join", 150f, 150f, 1.25f, 1.25f);
                    IDToJoinButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "Join Lobby", 450f, 150f, 1.25f, 1.25f);
                    JoinButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "Send a Random Card", 0f, -100f, 1.5f, 1.5f);
                    SendCardButton = LIPNHOMGGHF.HOAOLPGEBKJ;
                    Plugin.InitCardGameNetworking();
                }
                else
                {
                    Plugin.EndCardGameNetworking();
                }
            }
        }
        //handling buttons
        [HarmonyPatch(typeof(Scene_Titles), nameof(Scene_Titles.Update))]
        [HarmonyPostfix]
        public static void Scene_Titles_Update_Patch()
        {
            if (LIPNHOMGGHF.ODOAPLMOJPD == CollectibleCards2.Plugin.CardsMenuPage)
            {
                if (LIPNHOMGGHF.PIEMLEPEDFN == 5 && LIPNHOMGGHF.FKANHDIMMBJ[LobbyButton].CLMDCNDEBGD != 0)
                {
                    LIPNHOMGGHF.ODOAPLMOJPD = Plugin.MPLobbyPage;
                    LIPNHOMGGHF.ICGNAJFLAHL(0);
                }
            }
            if (LIPNHOMGGHF.ODOAPLMOJPD == Plugin.MPLobbyPage)
            {
                HostType = Mathf.RoundToInt(LIPNHOMGGHF.FKANHDIMMBJ[HostTypeButton].ODONMLDCHHF(HostType, 1f, 10f, 0f, 2f, 0));
                switch (HostType)
                {
                    case 0:
                        LIPNHOMGGHF.FKANHDIMMBJ[HostTypeButton].FFCNPGPALPD = "Public";
                        break;
                    case 1:
                        LIPNHOMGGHF.FKANHDIMMBJ[HostTypeButton].FFCNPGPALPD = "Friends Only";
                        break;
                    case 2:
                        LIPNHOMGGHF.FKANHDIMMBJ[HostTypeButton].FFCNPGPALPD = "Invite Only";
                        break;
                }
                if (LIPNHOMGGHF.PIEMLEPEDFN == 5 && LIPNHOMGGHF.FKANHDIMMBJ[HostLobbyButton].CLMDCNDEBGD != 0)
                {
                    Plugin.steamLobby.StartLobby(HostType);
                }
                LIPNHOMGGHF.FKANHDIMMBJ[LobbyIDButton].FFCNPGPALPD = LobbyID;
                if (LIPNHOMGGHF.PIEMLEPEDFN == 5 && LIPNHOMGGHF.FKANHDIMMBJ[LobbyIDButton].CLMDCNDEBGD != 0)
                {
                    // Clipboard.SetText("Hello, clipboard"); <- windows only
                }
                if (LIPNHOMGGHF.PIEMLEPEDFN == 5 && LIPNHOMGGHF.FKANHDIMMBJ[InviteFriendsButton].CLMDCNDEBGD != 0)
                {
                    Plugin.steamLobby.InviteFriends();
                }


                if (LIPNHOMGGHF.NNMDEFLLNBF == IDToJoinButton)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)) IDToJoin += "0";
                    if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) IDToJoin += "1";
                    if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) IDToJoin += "2";
                    if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) IDToJoin += "3";
                    if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) IDToJoin += "4";
                    if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)) IDToJoin += "5";
                    if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6)) IDToJoin += "6";
                    if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7)) IDToJoin += "7";
                    if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8)) IDToJoin += "8";
                    if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9)) IDToJoin += "9";
                    if(Input.GetKeyDown(KeyCode.Backspace) && IDToJoin.Length != 0) IDToJoin = IDToJoin.Remove(IDToJoin.Length-1);
                }
                LIPNHOMGGHF.FKANHDIMMBJ[IDToJoinButton].FFCNPGPALPD = IDToJoin;


                if (LIPNHOMGGHF.PIEMLEPEDFN == 5 && LIPNHOMGGHF.FKANHDIMMBJ[JoinButton].CLMDCNDEBGD != 0)
                    {
                        Plugin.steamLobby.JoinLobby(IDToJoin);
                    }
                    if (LIPNHOMGGHF.PIEMLEPEDFN == 5 && LIPNHOMGGHF.FKANHDIMMBJ[SendCardButton].CLMDCNDEBGD != 0)
                    {
                        int cardIndex = UnityEngine.Random.Range(0, CardMenu.Cards.Count + 1);
                        Plugin.steamNetworking.SEND_CARD(CardMenu.Cards[cardIndex]);
                    }





                    
                    if (LIPNHOMGGHF.PIEMLEPEDFN > 5)
                    {
                        LIPNHOMGGHF.PIEMLEPEDFN = 0;
                    }


                    if (LIPNHOMGGHF.PIEMLEPEDFN <= -5)
                    {
                        LIPNHOMGGHF.ODOAPLMOJPD = CollectibleCards2.Plugin.CardsMenuPage;
                        LIPNHOMGGHF.ICGNAJFLAHL(0);
                    }
                }
        }
    }
}
/*
 * public static class Clipboard
{
    public static void SetText(string text)
    {
        var powershell = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "powershell",
                Arguments = $"-command \"Set-Clipboard -Value \\\"{text}\\\"\""
            }
        };
        powershell.Start();
        powershell.WaitForExit();
    }

    public static string GetText()
    {
        var powershell = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                RedirectStandardOutput = true,
                FileName = "powershell",
                Arguments = "-command \"Get-Clipboard\""
            }
        };

        powershell.Start();
        string text = powershell.StandardOutput.ReadToEnd();
        powershell.StandardOutput.Close();
        powershell.WaitForExit();
        return text.TrimEnd();
    }
}*/