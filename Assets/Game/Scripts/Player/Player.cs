using Mirror;
using UnityEngine;

namespace Game.Scripts.Player
{
    public class Player : NetworkBehaviour
    {
        [SerializeField] private Renderer _shirtRenderer;

        [ClientRpc]
        public void RpcSetColor(Color color)
        {
            _shirtRenderer.material.color = color;
        }
    }
}