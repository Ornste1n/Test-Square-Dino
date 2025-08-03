using Mirror;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Player
{
    public class Player : NetworkBehaviour
    {
        [SerializeField] private Renderer _shirtRenderer;
        [SerializeField] private TextMeshPro _playerNameText;

        [SyncVar(hook = nameof(OnNameChanged))]
        private string _playerName;

        [SyncVar(hook = nameof(OnChangeColor))]
        private Color _shirtColor = Color.white;
        
        public void SetPlayerData(string name, Color color)
        {
            _playerName = name;
            _shirtColor = color;
        }
        
        private void OnNameChanged(string oldName, string newName)
        {
            if (_playerName != null)
                _playerNameText.text = newName;
        }
        
        private void OnChangeColor(Color oldColor, Color color)
        {
            _shirtRenderer.material.color = color;
        }
    }
}