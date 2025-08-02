using Mirror;
using UnityEngine;

namespace Game.Scripts.Network.Messages
{
    public struct PlayerMessage : NetworkMessage
    {
        public Color ShirtColor;
    }
}