using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CollectibleCards2;
using HarmonyLib;
using Steamworks;
using TextCopy;
using UnityEngine;
using UnityEngine.UI;

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


        public static int HostType { get; set; } = 0;
        public static string LobbyID { get; set; } = "";
        public static string IDToJoin { get; set; }

        public static GameObject MPLobbyText = null;
        public static GameObject HostText  = null;
        public static GameObject JoinText  = null;
        public static GameObject LobbyStatusText = null;
        private static bool CardsChecked = false;

        public static void DisplayLobbyTextTop()
        {
            Text text = Utils.SetupUIText(ref MPLobbyText, "Multiplayer Lobby");
            text.text = "Multiplayer Card Game Lobby Menu";
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            RectTransform rectTransform = text.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(300, 0);
            rectTransform.anchoredPosition = new Vector2(0, 300);
        }

        public static void DisplayHostText()
        {
            Text text = Utils.SetupUIText(ref HostText, "Host Lobby");
            text.text = "Host Lobby";
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            RectTransform rectTransform = text.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(300, 0);
            rectTransform.anchoredPosition = new Vector2(-300, 150);
        }
        public static void DisplayJoinText()
        {
            Text text = Utils.SetupUIText(ref JoinText, "Join Lobby");
            text.text = "Join Lobby";
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            RectTransform rectTransform = text.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(300, 0);
            rectTransform.anchoredPosition = new Vector2(300, 150);
        }
        public static void DisplayStatusText()
        {
            Text text = Utils.SetupUIText(ref LobbyStatusText, "Lobby Status");
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            RectTransform rectTransform = text.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(300, 0);
            rectTransform.anchoredPosition = new Vector2(0, -300);
        }
        public static void UpdateStatusText()
        {
            if(LobbyStatusText != null)
                LobbyStatusText.GetComponent<Text>().text = "Lobby Status: " + Plugin.steamLobby.GetLobbyStatusText();
        }
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
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "Multiplayer Card Game", 350f, -150f, 1.5f, 1.5f);
                    LobbyButton = LIPNHOMGGHF.HOAOLPGEBKJ;
                    CardsChecked = false;

                }
                if (LIPNHOMGGHF.ODOAPLMOJPD == Plugin.MPLobbyPage)
                {

                    LobbyID = "";
                    DisplayLobbyTextTop();
                    DisplayHostText();
                    DisplayJoinText();
                    DisplayStatusText();
                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(2, "Lobby Type", -425f, 100f, 1f, 1f);
                    HostTypeButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "Host Lobby", -175f, 100f, 1f, 1f);
                    HostLobbyButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(2, "Lobby ID (Click to Copy)", -425f, 50f, 1f, 1f);
                    LobbyIDButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "Invite Friends", -175f, 50f, 1f, 1f);
                    InviteFriendsButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(2, "ID To Join (Click to Paste)", 175f, 100f, 1f, 1f);
                    IDToJoinButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "Join Lobby", 436f, 100f, 1f, 1f);
                    JoinButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    Plugin.InitCardGameNetworking();
                }
                else
                {
                    if(LIPNHOMGGHF.ODOAPLMOJPD != Plugin.GameplayPage)
                    {
                        Plugin.EndCardGameNetworking();
                    }
                    if (MPLobbyText != null)
                    {
                        UnityEngine.Object.Destroy(MPLobbyText);
                        MPLobbyText = null;
                    }
                    if (LobbyStatusText != null)
                    {
                        UnityEngine.Object.Destroy(LobbyStatusText);
                        LobbyStatusText = null;
                    }
                    if (HostText != null)
                    {
                        UnityEngine.Object.Destroy(HostText);
                        HostText = null;
                    }
                    if (JoinText != null)
                    {
                        UnityEngine.Object.Destroy(JoinText);
                        JoinText = null;
                    }
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
                if(!CardsChecked)
                {
                    Debug.LogWarning("CHECKING MP CARDS");
                    Gameplay.FillupDeck();
                    Debug.LogWarning("FOUND CARDS " + Gameplay.Deck.Count);
                    if (Gameplay.Deck.Count < 10)
                    {
                        LIPNHOMGGHF.FKANHDIMMBJ[LobbyButton].AHBNKMMMGFI = 0;
                    }
                    CardsChecked = true;
                }
            }
            if (LIPNHOMGGHF.ODOAPLMOJPD == Plugin.MPLobbyPage)
            {
                UpdateStatusText();
                if (Plugin.steamLobby.currentLobbyID == CSteamID.Nil)
                {
                    LIPNHOMGGHF.FKANHDIMMBJ[HostTypeButton].AHBNKMMMGFI = 1;
                    LIPNHOMGGHF.FKANHDIMMBJ[HostLobbyButton].AHBNKMMMGFI = 1;
                    LIPNHOMGGHF.FKANHDIMMBJ[LobbyIDButton].AHBNKMMMGFI = 0;
                    LIPNHOMGGHF.FKANHDIMMBJ[InviteFriendsButton].AHBNKMMMGFI = 0;
                    LIPNHOMGGHF.FKANHDIMMBJ[IDToJoinButton].AHBNKMMMGFI = 1;
                    LIPNHOMGGHF.FKANHDIMMBJ[JoinButton].AHBNKMMMGFI = 1;

                }
                else
                {
                    LIPNHOMGGHF.FKANHDIMMBJ[HostTypeButton].AHBNKMMMGFI = 0;
                    LIPNHOMGGHF.FKANHDIMMBJ[HostLobbyButton].AHBNKMMMGFI = 0;
                    LIPNHOMGGHF.FKANHDIMMBJ[LobbyIDButton].AHBNKMMMGFI = 1;
                    LIPNHOMGGHF.FKANHDIMMBJ[InviteFriendsButton].AHBNKMMMGFI = 1;
                    LIPNHOMGGHF.FKANHDIMMBJ[IDToJoinButton].AHBNKMMMGFI = 0;
                    LIPNHOMGGHF.FKANHDIMMBJ[JoinButton].AHBNKMMMGFI = 0;
                }
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
                if (HostType == 0)
                {
                    LIPNHOMGGHF.FKANHDIMMBJ[LobbyIDButton].FFCNPGPALPD = LobbyID;
                }
                else
                {
                    LIPNHOMGGHF.FKANHDIMMBJ[LobbyIDButton].FFCNPGPALPD = "";
                    LIPNHOMGGHF.FKANHDIMMBJ[LobbyIDButton].AHBNKMMMGFI = 0;
                }
                if (LIPNHOMGGHF.PIEMLEPEDFN == 5 && LIPNHOMGGHF.FKANHDIMMBJ[LobbyIDButton].CLMDCNDEBGD != 0)
                {
                    ClipboardService.SetText(LobbyID); 
                }
                if (LIPNHOMGGHF.PIEMLEPEDFN == 5 && LIPNHOMGGHF.FKANHDIMMBJ[InviteFriendsButton].CLMDCNDEBGD != 0)
                {
                    Plugin.steamLobby.InviteFriends();
                }


                if (LIPNHOMGGHF.PIEMLEPEDFN == 5 && LIPNHOMGGHF.FKANHDIMMBJ[IDToJoinButton].CLMDCNDEBGD != 0)
                {
                    IDToJoin = ClipboardService.GetText();
                }
                LIPNHOMGGHF.FKANHDIMMBJ[IDToJoinButton].FFCNPGPALPD = IDToJoin;


                if (LIPNHOMGGHF.PIEMLEPEDFN == 5 && LIPNHOMGGHF.FKANHDIMMBJ[JoinButton].CLMDCNDEBGD != 0)
                {
                    Plugin.steamLobby.JoinLobby(IDToJoin);
                }
                if(Plugin.steamLobby.ConnectedPlayers == Gameplay.Players)
                {
                    LIPNHOMGGHF.ODOAPLMOJPD = Plugin.GameplayPage;
                    LIPNHOMGGHF.ICGNAJFLAHL(0);
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
