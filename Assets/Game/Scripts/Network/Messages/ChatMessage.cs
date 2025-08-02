using Mirror;

namespace Game.Scripts.Network.Messages
{
    public struct ChatMessage : NetworkMessage
    {
        public string Sender;
        public string Message;
    }
}