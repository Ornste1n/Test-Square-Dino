using Mirror;

namespace Game.Scripts.Network.Messages
{
    public struct AuthRequestMessage : NetworkMessage
    {
        public string AuthUsername;
    }
        
    public struct AuthResponseMessage : NetworkMessage
    {
        public byte Code;
        public string Message;
    }
}