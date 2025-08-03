using TMPro;
using Mirror;
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Network.Messages;

namespace Game.Scripts.Network.Chat
{
    public class ChatAuthenticator : NetworkAuthenticator
    {
        [SerializeField] private TMP_InputField _nicknameField;

        private readonly Dictionary<NetworkConnectionToClient, string> _connNames = new();
        private string _playerName;

        public string GetName(NetworkConnectionToClient key) => _connNames.GetValueOrDefault(key) ?? "Unknown";
        public void RemoveConnection(NetworkConnectionToClient conn) =>  _connNames.Remove(conn);

        #region Server

        public override void OnStartServer()
        {
            NetworkServer.RegisterHandler<AuthRequestMessage>(OnAuthRequestMessage, false);
        }

        private void OnAuthRequestMessage(NetworkConnectionToClient conn, AuthRequestMessage msg)
        {
            if (!_connNames.Values.Contains(msg.AuthUsername))
            {
                _connNames.Add(conn, msg.AuthUsername);
                conn.authenticationData = msg.AuthUsername;
                
                AuthResponseMessage authResponseMessage = new AuthResponseMessage
                {
                    Code = 100,
                    Message = "Success"
                };
                
                conn.Send(authResponseMessage);
                ServerAccept(conn);
            }
            else
            {
                AuthResponseMessage authResponseMessage = new AuthResponseMessage
                {
                    Code = 200,
                    Message = "Username already in use...try again"
                };

                conn.Send(authResponseMessage);
                conn.isAuthenticated = false;
                StartCoroutine(DelayedDisconnect(conn, 1f));
            }
        }
        
        private IEnumerator DelayedDisconnect(NetworkConnectionToClient conn, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            ServerReject(conn);
        }

        public override void OnStopServer()
        {
            NetworkServer.UnregisterHandler<AuthRequestMessage>();
        }
        #endregion

        #region Client

        public override void OnStartClient()
        {
            base.OnStartClient();
            NetworkClient.RegisterHandler<AuthResponseMessage>(OnAuthResponseMessage, false);
            
            if (_nicknameField != null && !string.IsNullOrWhiteSpace(_nicknameField.text))
                _playerName = _nicknameField.text;
            else
                _playerName = "Player" + Random.Range(1000, 9999);
            
            _nicknameField.gameObject.SetActive(false);
        }

        private void OnAuthResponseMessage(AuthResponseMessage msg)
        {
            if (msg.Code == 100)
            {
                ClientAccept();
            }
            else
            {
                Debug.LogWarning($"Authentication Response: {msg.Code} {msg.Message}");
                NetworkManager.singleton.StopHost();
            }
        }
        
        public override void OnClientAuthenticate()
        {
            NetworkClient.Send(new AuthRequestMessage { AuthUsername = _playerName });
        }
        
        public override void OnStopClient()
        {
            _nicknameField.gameObject.SetActive(true);
            NetworkClient.UnregisterHandler<AuthResponseMessage>();
        }
        
        #endregion
    }
}