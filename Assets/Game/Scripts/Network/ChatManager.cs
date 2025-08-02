using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Scripts.Network.Messages;

namespace Game.Scripts.Network
{
    public class ChatManager : MonoBehaviour
    {
        [SerializeField] private InputAction _sendAction;

        private void Awake()
        {
            NetworkClient.RegisterHandler<ChatMessage>(OnMessageReceived);
            
            _sendAction.performed += SendMessageToServer;
            _sendAction.Enable();
        }
        
        private void OnMessageReceived(ChatMessage msg)
        {
            Debug.Log($"{msg.Message} от <{msg.Sender}>");
        }

        private void SendMessageToServer(InputAction.CallbackContext _)
        {
            ChatMessage msg = new()
            {
                Sender = System.Environment.UserName,
                Message = "Привет"
            };

            NetworkClient.Send(msg);
        }
        
        private void OnDestroy()
        {
            NetworkClient.UnregisterHandler<ChatMessage>();
            
            _sendAction.performed -= SendMessageToServer;
            _sendAction.Disable();
            _sendAction.Dispose();
        }

    }
}