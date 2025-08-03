using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts.Network.Chat
{
    public class ChatManager : NetworkBehaviour
    {
        [SerializeField] private ChatAuthenticator _chatAuthenticator;
        [Space]
        [SerializeField] private InputAction _sendAction;
        [SerializeField] private string _message = "Привет";

        private void Awake()
        {
            _sendAction.performed += SendMessageToServer;
            _sendAction.Enable();
        }
        
        [Command(requiresAuthority = false)]
        private void CmdSend(string message, NetworkConnectionToClient sender = null)
        {
            string playerName = _chatAuthenticator.GetName(sender);

            if (!string.IsNullOrWhiteSpace(message))
                RpcReceive(playerName, message.Trim());
        }

        [ClientRpc]
        private void RpcReceive(string playerName, string message)
        {
            Debug.Log($"{message} от <{playerName}>");
        }
        
        private void SendMessageToServer(InputAction.CallbackContext _)
        {
            CmdSend(_message);
        }

        private void OnDestroy()
        {
            _sendAction.performed -= SendMessageToServer;
            _sendAction.Disable();
            _sendAction.Dispose();
        }
    }
}