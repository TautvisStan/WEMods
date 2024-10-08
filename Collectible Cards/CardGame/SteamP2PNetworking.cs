using Steamworks;
using UnityEngine;
using System.Text;
using Newtonsoft.Json;
using System;
using CollectibleCards2;

namespace CardGame
{
    public class NetworkMessage
    {
        public static string TYPE_TEXT_MESSAGE = "TYPE_TEXT_MESSAGE";
        public static string TYPE_READY = "TYPE_READY";
        public static string TYPE_CARD = "TYPE_CARD";

        public string Type { get; set; }
        public string Content { get; set; }
        public CSteamID SenderID { get; set; }
        public int LobbyIndex { get; set; }
        public NetworkMessage(string type, CSteamID sender, int lobbyIndex, string content)
        {
            Type = type;
            SenderID = sender;
            LobbyIndex = lobbyIndex;
            Content = content;
        }
    }
    public class SteamP2PNetworking : MonoBehaviour
    {
        protected Callback<P2PSessionRequest_t> m_P2PSessionRequest;
        protected Callback<P2PSessionConnectFail_t> m_P2PConnectFail;
        public SteamLobby lobby = Plugin.steamLobby;
        void Start()
        {
            if (!SteamManager.LHAIOCMDOLP) return;
            if (Plugin.CallbacksAlreadyDone) return;
            // Set up a callback to handle incoming P2P packets
            m_P2PSessionRequest = Callback<P2PSessionRequest_t>.Create(OnP2PSessionRequest);
            m_P2PConnectFail = Callback<P2PSessionConnectFail_t>.Create(OnP2PConnectFail);
        }

        void Update()
        {
            // Check for any incoming P2P data
            uint messageSize;
            while (SteamNetworking.IsP2PPacketAvailable(out messageSize))
            {
                byte[] data = new byte[messageSize];
                CSteamID remoteID;
                if (SteamNetworking.ReadP2PPacket(data, messageSize, out messageSize, out remoteID))
                {
                    string message = Encoding.UTF8.GetString(data);
                    //   Debug.LogWarning("Received message from " + remoteID + ": " + message);
                    Debug.LogWarning("Received message from " + remoteID);
                    NetworkMessage networkMessage = null;
                    try
                    {
                        networkMessage = JsonConvert.DeserializeObject<NetworkMessage>(message);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning("Failed to convert, message was most likely not the custom NetworkMessage type");
                        Debug.LogWarning("Contents: " + message);
                    }
                    if(networkMessage != null)
                    {
                        if(networkMessage.Type == NetworkMessage.TYPE_CARD)
                        {
                            string content = networkMessage.Content;
                            PlayableCard receivedCard = null;
                            receivedCard = JsonConvert.DeserializeObject<PlayableCard>(content);
                            Debug.LogWarning($"CARD RECEIVED: {receivedCard.WrestlerName} from {remoteID} (lobby index {networkMessage.LobbyIndex})");
                            Gameplay.ReceiveCard(receivedCard, networkMessage.LobbyIndex);
                        }
                        if (networkMessage.Type == NetworkMessage.TYPE_TEXT_MESSAGE)
                        { 
                            Debug.LogWarning($"MESSAGE RECEIVED: '{networkMessage.Content}' from {remoteID} (lobby index {networkMessage.LobbyIndex})");
                        }
                    }
                }
            }
        }
        public void SEND_CARD(CollectibleCard card)
        {
            PlayableCard playablecard = new(card);
            playablecard.WrestlerName = playablecard.WrestlerName.Trim();
            string encodedCard = JsonConvert.SerializeObject(playablecard);
            NetworkMessage message = new(NetworkMessage.TYPE_CARD, SteamUser.GetSteamID(), lobby.SteamLobbyMemberIndex, encodedCard);
            Debug.LogWarning($"Sending card: {playablecard.WrestlerName}");
            SendMessageToAll(JsonConvert.SerializeObject(message));
        }
        public void SEND_TEXT(string text)
        {
            NetworkMessage message = new(NetworkMessage.TYPE_TEXT_MESSAGE, SteamUser.GetSteamID(), lobby.SteamLobbyMemberIndex, text);
            Debug.LogWarning($"Sending text message: {message}");
            SendMessageToAll(JsonConvert.SerializeObject(message));
        }
        public void SendMessageToAll(string message)
        {
            if (lobby.currentLobbyID == null) return;
            Debug.LogWarning($"Sending message to all: {message}");
            // Get the number of players in the lobby
            int numPlayers = SteamMatchmaking.GetNumLobbyMembers(lobby.currentLobbyID);
            byte[] data = Encoding.UTF8.GetBytes(message);

            // Send message to each player in the lobby
            for (int i = 0; i < numPlayers; i++)
            {
                CSteamID playerID = SteamMatchmaking.GetLobbyMemberByIndex(lobby.currentLobbyID, i);
                SteamNetworking.SendP2PPacket(playerID, data, (uint)data.Length, EP2PSend.k_EP2PSendReliable);
            }
        }


        public void SendMessage(string message, CSteamID recipient)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            SteamNetworking.SendP2PPacket(recipient, data, (uint)data.Length, EP2PSend.k_EP2PSendReliable);
        }

        private void OnP2PSessionRequest(P2PSessionRequest_t p2pSessionRequest)
        {
            CSteamID remoteID = p2pSessionRequest.m_steamIDRemote;

            // Automatically accept the session request
            SteamNetworking.AcceptP2PSessionWithUser(remoteID);
            Debug.LogWarning("Accepted P2P session request from: " + SteamFriends.GetFriendPersonaName(remoteID));
        }

        // Handle P2P connection failures
        private void OnP2PConnectFail(P2PSessionConnectFail_t p2pConnectFail)
        {
            CSteamID remoteID = p2pConnectFail.m_steamIDRemote;
            EP2PSessionError error = (EP2PSessionError)p2pConnectFail.m_eP2PSessionError;

            Debug.LogError("P2P connection failed with user: " + SteamFriends.GetFriendPersonaName(remoteID) + ". Error: " + error);
        }
    }

}
