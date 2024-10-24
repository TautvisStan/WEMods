﻿using System;
using System.Collections.Generic;
using System.Text;
using Steamworks;
using UnityEngine;

namespace CardGame
{
    public class SteamLobby : MonoBehaviour
    {
        protected Callback<LobbyCreated_t> m_LobbyCreated;
        protected Callback<LobbyEnter_t> m_LobbyEntered;
        protected Callback<LobbyChatUpdate_t> m_LobbyChatUpdated;
        protected Callback<GameLobbyJoinRequested_t> m_GameLobbyJoinRequested;
        protected CallResult<LobbyMatchList_t> m_LobbyMatchList;

        internal CSteamID currentLobbyID;
        public int ConnectedPlayers = 0;
        public int SteamLobbyMemberIndex { get; set; } = -1;

        private const string HostAddressKey = "HostAddress";
        private string HostAddress = "";
        private bool Matchmaking { get; set; } = false;

        void Start()
        {
            if (!SteamManager.LHAIOCMDOLP) return;
            if (Plugin.CallbacksAlreadyDone) return;
            m_LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            m_LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
            m_LobbyChatUpdated = Callback<LobbyChatUpdate_t>.Create(OnLobbyChatUpdate);
            m_GameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
            m_LobbyMatchList = CallResult<LobbyMatchList_t>.Create(OnLobbyMatchListReceived);

        }
        // Function to start matchmaking
        public void StartMatchmaking()
        {
            Debug.LogWarning("Searching for public lobbies...");
            Matchmaking = true;
            // Search for public lobbies (with filters if needed)
            SteamAPICall_t handle = SteamMatchmaking.RequestLobbyList();
            m_LobbyMatchList.Set(handle); // Set the callback for the result of the search
            
        }

        // Callback when lobby list is received
        private void OnLobbyMatchListReceived(LobbyMatchList_t result, bool bIOFailure)
        {
            Debug.LogWarning("LOBBY SEARCH RESULTS");
            Debug.LogWarning(bIOFailure);
            Debug.LogWarning(result.m_nLobbiesMatching);
            if (bIOFailure || result.m_nLobbiesMatching == 0)
            {
                // No suitable lobbies found, create a new one
                Debug.LogWarning("No public lobbies found. Creating a new lobby...");
                LobbyMenu.HostType = 0;
                StartLobby(0);
                return;
            }

            // Join the first available public lobby (or apply custom logic to choose)
            for (int i = 0; i < result.m_nLobbiesMatching; i++)
            {
                CSteamID lobbyID = SteamMatchmaking.GetLobbyByIndex(i);
                if (SteamMatchmaking.GetLobbyData(lobbyID, "Started") != "true" && SteamMatchmaking.GetLobbyData(lobbyID, "Matchmaking") == "true")
                {
                    Debug.LogWarning("Joining existing public lobby: " + lobbyID);
                    SteamMatchmaking.JoinLobby(lobbyID);
                    return;
                }
                else
                {
                    Debug.LogWarning($"Lobby was ineligible: {lobbyID}|{SteamMatchmaking.GetLobbyData(lobbyID, "Started")}|{SteamMatchmaking.GetLobbyData(lobbyID, "Matchmaking")}|");
                }
            }
            Debug.LogWarning("Found lobbies but not eligible. Creating a new lobby...");
            LobbyMenu.HostType = 0;
            StartLobby(0);
            return;
        }
        void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
        {

            Debug.LogWarning("Received a join request. Joining lobby: " + callback.m_steamIDLobby);
            if (LIPNHOMGGHF.ODOAPLMOJPD == Plugin.MPLobbyPage)
            {
                if (SteamMatchmaking.GetLobbyData(callback.m_steamIDLobby, "Started") != "true")
                {
                    // Attempt to join the lobby via the Steam ID received in the callback
                    SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
                }
                else
                {
                    Debug.LogWarning("TRIED JOINING MATCH IN PROGRESSS!!!");
                }
            }
            else
            {
                Debug.LogWarning("WARNING! NOT IN THE LOBBY MENU!!!!");
            }
        }
        public void JoinLobby(string lobbyID)
        {
            CSteamID steamID = new CSteamID(ulong.Parse(lobbyID));
            if (SteamMatchmaking.GetLobbyData(steamID, "Started") != "true")
            {
                // Attempt to join the lobby via the Steam ID received in the callback
                SteamMatchmaking.JoinLobby(steamID);
            }
            else
            {
                Debug.LogWarning("TRIED JOINING MATCH IN PROGRESSS!!!");
            }
            SteamMatchmaking.JoinLobby(steamID);
        }
        public void StartLobby(int type)
        {
            
            Debug.LogWarning("STARTING STEAM LOBBY");
            if (!SteamManager.LHAIOCMDOLP) return;
            switch (type)
            {
                case 0:
                    SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 2);
                    break;
                case 1:
                    SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 2);
                    break;
                case 2: SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePrivate, 2);
                    break;
            }
                
        }
        public string GetLobbyStatusText()
        {
            string text = "";
            if (currentLobbyID == CSteamID.Nil) text = "Not in a lobby";
            if (currentLobbyID != CSteamID.Nil) text = "In a lobby. Connected players: " + ConnectedPlayers;
            if (Matchmaking) text = "Matchmaking. " + text;
            return text;
        }
        public void InviteFriends()
        {
            SteamFriends.ActivateGameOverlayInviteDialog(currentLobbyID);
        }
        public void LeaveLobby()
        {
            if (currentLobbyID != CSteamID.Nil)
            {
                SteamMatchmaking.LeaveLobby(currentLobbyID);
                Debug.LogWarning("Left the lobby: " + currentLobbyID);
                currentLobbyID = CSteamID.Nil;  // Clear the lobby ID after leaving
                SteamLobbyMemberIndex = -1;
                ConnectedPlayers = 0;
                Gameplay.CleanupLobby();
                Matchmaking = false;
                HostAddress = "";
            }
        }
        void OnLobbyCreated(LobbyCreated_t result)
        {
            if (result.m_eResult != EResult.k_EResultOK)
            {
                Debug.LogWarning("Failed to create lobby.");
                return;
            }

            // Set the host's Steam ID for other players to find them
            HostAddress = SteamUser.GetSteamID().ToString();
            SteamMatchmaking.SetLobbyData(new CSteamID(result.m_ulSteamIDLobby), HostAddressKey, HostAddress);
            
            Debug.LogWarning("Lobby created! Lobby ID: " + result.m_ulSteamIDLobby);
            if (LobbyMenu.HostType == 0) LobbyMenu.LobbyID = result.m_ulSteamIDLobby.ToString();
            if (Matchmaking) MarkAsMatchmaking();
        }
        public int GetCurrentIndexInLobby()
        {
            for (int i = 0; i < SteamMatchmaking.GetNumLobbyMembers(currentLobbyID); i++)
            {
                if (SteamMatchmaking.GetLobbyMemberByIndex(currentLobbyID, i) == SteamUser.GetSteamID()) return i;
            }
            return -1;
        }
        public void MarkAsStarted()
        {
            if (HostAddress == SteamUser.GetSteamID().ToString())
            {
                SteamMatchmaking.SetLobbyData(currentLobbyID, "Started", "true");
                Debug.LogWarning("Marking lobby as started");
            }
        }
        public void MarkAsMatchmaking()
        {
            if (HostAddress == SteamUser.GetSteamID().ToString())
            {
                SteamMatchmaking.SetLobbyData(currentLobbyID, "Matchmaking", "true");
                Debug.LogWarning("Marking lobby as matchmaking");
                
            }
        }
        void OnLobbyEntered(LobbyEnter_t result)
        {
            currentLobbyID = new CSteamID(result.m_ulSteamIDLobby);
            Debug.LogWarning("Entered Lobby: " + currentLobbyID);
            SteamLobbyMemberIndex = GetCurrentIndexInLobby();
            // Log all current members in the lobby
            int memberCount = SteamMatchmaking.GetNumLobbyMembers(currentLobbyID);
            for (int i = 0; i < memberCount; i++)
            {
                CSteamID memberID = SteamMatchmaking.GetLobbyMemberByIndex(currentLobbyID, i);
                Debug.LogWarning("Lobby Member: " + SteamFriends.GetFriendPersonaName(memberID));
            }
            ConnectedPlayers = memberCount;

            // If you are not the host, get the host's Steam ID
            if (!SteamUser.GetSteamID().Equals(SteamMatchmaking.GetLobbyOwner(new CSteamID(result.m_ulSteamIDLobby))))
            {
                HostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(result.m_ulSteamIDLobby), HostAddressKey);
                Debug.LogWarning("Host Steam ID: " + HostAddress);

                // You can now connect to the host via P2P
            }


        }
        void OnLobbyChatUpdate(LobbyChatUpdate_t callback)
        {
            CSteamID userChanged = new CSteamID(callback.m_ulSteamIDUserChanged);
            CSteamID makingChange = new CSteamID(callback.m_ulSteamIDMakingChange);
            EChatMemberStateChange stateChange = (EChatMemberStateChange)callback.m_rgfChatMemberStateChange;

            // Check if someone joined
            if (stateChange == EChatMemberStateChange.k_EChatMemberStateChangeEntered)
            {
                Debug.LogWarning("User joined: " + SteamFriends.GetFriendPersonaName(userChanged));
                ConnectedPlayers++;
            }
            // Check if the user left or was disconnected
            if (stateChange == EChatMemberStateChange.k_EChatMemberStateChangeLeft ||
                stateChange == EChatMemberStateChange.k_EChatMemberStateChangeDisconnected)
            {
                Debug.LogWarning("Player left or disconnected: " + SteamFriends.GetFriendPersonaName(userChanged));
                ConnectedPlayers--;
                if (Gameplay.Connected) Gameplay.AnotherPlayerDisconnected();
                // Check if this is the host (lobby will be destroyed if the host leaves)
                if (userChanged.ToString() == HostAddress)
                {
                    Debug.LogWarning("Host left. Lobby will be closed.");
                    // Clear lobby data and update UI
                    currentLobbyID = CSteamID.Nil;
                    SteamLobbyMemberIndex = -1;
                }
            }
        }
    }


}
