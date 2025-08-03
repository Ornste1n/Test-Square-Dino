using Mirror;
using UnityEngine;
using Game.Scripts.Network.Chat;
using Game.Scripts.Network.Messages;

namespace Game.Scripts.Network
{
    public class MyNetworkManager : NetworkManager
    {
        [SerializeField] private ChatAuthenticator _chatAuthenticator;
        
        public override void OnStartServer()
        {
            base.OnStartServer();
            NetworkServer.RegisterHandler<PlayerMessage>(OnCreatePlayer);
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
        
        private void OnCreatePlayer(NetworkConnectionToClient conn, PlayerMessage msg)
        {
            GameObject go = Instantiate(playerPrefab);

            NetworkServer.AddPlayerForConnection(conn, go);

            if (!go.TryGetComponent(out Player.Player player)) return;
            {
                string nickname = _chatAuthenticator.GetName(conn);
                player.SetPlayerData(nickname, msg.ShirtColor);
            }
        }
        
        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);
            _chatAuthenticator.RemoveConnection(conn);
        }
    }
}