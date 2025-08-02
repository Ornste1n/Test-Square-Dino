using Mirror;
using UnityEngine;
using Game.Scripts.Network.Messages;

namespace Game.Scripts.Network
{
    public class MyNetworkManager : NetworkManager
    {
        public override void OnStartServer()
        {
            base.OnStartServer();
            NetworkServer.RegisterHandler<PlayerMessage>(OnCreatePlayer);
            NetworkServer.RegisterHandler<ChatMessage>(OnServerReceiveChatMessage);
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();

            PlayerMessage message = new()
            {
                ShirtColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f),
            };
            
            NetworkClient.Send(message);
        }
        
        private void OnServerReceiveChatMessage(NetworkConnectionToClient conn, ChatMessage msg)
        {
            NetworkServer.SendToAll(msg);
        }
        
        private void OnCreatePlayer(NetworkConnectionToClient conn, PlayerMessage msg)
        {
            GameObject go = Instantiate(playerPrefab);

            NetworkServer.AddPlayerForConnection(conn, go);
            
            if (go.TryGetComponent(out Player.Player player))
                player.RpcSetColor(msg.ShirtColor);
        }
    }
}