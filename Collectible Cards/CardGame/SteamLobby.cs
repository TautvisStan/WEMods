using System;
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
        private const string LobbyStatusKey = "LobbyStatus";
        private const string LobbyStatusOpen = "Open";
        private const string LobbyStatusMatchmaking = "Matchmaking";
        private const string LobbyStatusStarted = "Started";
        private const string LobbyStatusClosed = "Closed";
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
            Plugin.Log.LogInfo("Searching for public lobbies...");
            Matchmaking = true;
            // Search for public lobbies (with filters if needed)
            SteamAPICall_t handle = SteamMatchmaking.RequestLobbyList();
            m_LobbyMatchList.Set(handle); // Set the callback for the result of the search
            
        }

        // Callback when lobby list is received
        private void OnLobbyMatchListReceived(LobbyMatchList_t result, bool bIOFailure)
        {
            Plugin.Log.LogInfo("Lobby search results:");
            if (bIOFailure || result.m_nLobbiesMatching == 0)
            {
                // No suitable lobbies found, create a new one
                Plugin.Log.LogInfo("No public lobbies found. Creating a new lobby...");
                LobbyMenu.HostType = 0;
                StartLobby(0);
                return;
            }

            // Join the first available public lobby (or apply custom logic to choose)
            for (int i = 0; i < result.m_nLobbiesMatching; i++)
            {
                CSteamID lobbyID = SteamMatchmaking.GetLobbyByIndex(i);
                Plugin.Log.LogInfo($"Checking lobby {lobbyID}");
                Plugin.Log.LogInfo($"Lobby status: {SteamMatchmaking.GetLobbyData(lobbyID, LobbyStatusKey)}");
                Plugin.Log.LogInfo($"Connected players: {SteamMatchmaking.GetNumLobbyMembers(lobbyID)}");
                if (SteamMatchmaking.GetLobbyData(lobbyID, LobbyStatusKey) == LobbyStatusMatchmaking && SteamMatchmaking.GetNumLobbyMembers(lobbyID) == 1)
                {
                    Plugin.Log.LogInfo("Joining existing public lobby: " + lobbyID);
                    SteamMatchmaking.JoinLobby(lobbyID);
                    return;
                }
            }
            Plugin.Log.LogInfo("Found lobbies but not suitable. Creating a new lobby...");
            LobbyMenu.HostType = 0;
            StartLobby(0);
            return;
        }
        void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
        {

            Plugin.Log.LogInfo("Received a join request. Joining lobby: " + callback.m_steamIDLobby);
            string status = SteamMatchmaking.GetLobbyData(callback.m_steamIDLobby, LobbyStatusKey);
            if (LIPNHOMGGHF.ODOAPLMOJPD == Plugin.MPLobbyPage)
            {
                if(status != LobbyStatusClosed && status != LobbyStatusStarted)
                {
                    // Attempt to join the lobby via the Steam ID received in the callback
                    SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
                }
                else
                {
                    Plugin.Log.LogError($"Failed to join, bad lobby status: {status}");
                }
            }
            else
            {
                Plugin.Log.LogError("WARNING! NOT IN THE LOBBY MENU!!!!");
            }
        }
        public void JoinLobby(string lobbyID)
        {
            CSteamID steamID = new CSteamID(ulong.Parse(lobbyID));
            string status = SteamMatchmaking.GetLobbyData(steamID, LobbyStatusKey);
            if (status != LobbyStatusClosed && status != LobbyStatusStarted)
            {
                // Attempt to join the lobby via the Steam ID received in the callback
                SteamMatchmaking.JoinLobby(steamID);
            }
            else
            {
                Plugin.Log.LogError($"Failed to join, bad lobby status: {status}");
            }
        }
        public void StartLobby(int type)
        {

            Plugin.Log.LogInfo("Starting Steam lobby...");
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
                SteamMatchmaking.SetLobbyData(currentLobbyID, LobbyStatusKey, LobbyStatusClosed);
                SteamMatchmaking.LeaveLobby(currentLobbyID);
                Plugin.Log.LogInfo("Left the lobby: " + currentLobbyID);
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
                Plugin.Log.LogError("Failed to create lobby.");
                Plugin.Log.LogError(result.m_eResult);
                return;
            }
            currentLobbyID = new CSteamID(result.m_ulSteamIDLobby);
            // Set the host's Steam ID for other players to find them
            HostAddress = SteamUser.GetSteamID().ToString();
            SteamMatchmaking.SetLobbyData(currentLobbyID, HostAddressKey, HostAddress);
            
            Plugin.Log.LogInfo("Lobby created! Lobby ID: " + currentLobbyID);
            if (LobbyMenu.HostType == 0) LobbyMenu.LobbyID = currentLobbyID.ToString();
            if (Matchmaking) MarkAsMatchmaking();
            else SteamMatchmaking.SetLobbyData(currentLobbyID, LobbyStatusKey, LobbyStatusOpen);
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
                SteamMatchmaking.SetLobbyData(currentLobbyID, LobbyStatusKey, LobbyStatusStarted);
                Plugin.Log.LogInfo("Marking lobby as started");
            }
        }
        public void MarkAsMatchmaking()
        {
            if (HostAddress == SteamUser.GetSteamID().ToString())
            {
                SteamMatchmaking.SetLobbyData(currentLobbyID, LobbyStatusKey, LobbyStatusMatchmaking);
                Plugin.Log.LogInfo("Marking lobby as matchmaking");
                
            }
        }
        void OnLobbyEntered(LobbyEnter_t result)
        {
            currentLobbyID = new CSteamID(result.m_ulSteamIDLobby);
            Plugin.Log.LogInfo("Entered Lobby: " + currentLobbyID);
            SteamLobbyMemberIndex = GetCurrentIndexInLobby();
            // Log all current members in the lobby
            int memberCount = SteamMatchmaking.GetNumLobbyMembers(currentLobbyID);
            for (int i = 0; i < memberCount; i++)
            {
                CSteamID memberID = SteamMatchmaking.GetLobbyMemberByIndex(currentLobbyID, i);
                Plugin.Log.LogInfo("Lobby Member: " + SteamFriends.GetFriendPersonaName(memberID));
            }
            ConnectedPlayers = memberCount;

            // If you are not the host, get the host's Steam ID
            if (!SteamUser.GetSteamID().Equals(SteamMatchmaking.GetLobbyOwner(new CSteamID(result.m_ulSteamIDLobby))))
            {
                HostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(result.m_ulSteamIDLobby), HostAddressKey);
                Plugin.Log.LogInfo("Host Steam ID: " + HostAddress);

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
                Plugin.Log.LogInfo("User joined: " + SteamFriends.GetFriendPersonaName(userChanged));
                ConnectedPlayers++;
            }
            // Check if the user left or was disconnected
            if (stateChange == EChatMemberStateChange.k_EChatMemberStateChangeLeft ||
                stateChange == EChatMemberStateChange.k_EChatMemberStateChangeDisconnected)
            {
                Plugin.Log.LogInfo("Player left or disconnected: " + SteamFriends.GetFriendPersonaName(userChanged));
                ConnectedPlayers--;
                if (Gameplay.Connected) Gameplay.AnotherPlayerDisconnected();
                // Check if this is the host (lobby will be destroyed if the host leaves)
                if (userChanged.ToString() == HostAddress)
                {
                    Plugin.Log.LogInfo("Host left. Lobby will be closed.");
                    // Clear lobby data and update UI
                  //  currentLobbyID = CSteamID.Nil;
                   // SteamLobbyMemberIndex = -1;
                }
            }
        }
    }


}
