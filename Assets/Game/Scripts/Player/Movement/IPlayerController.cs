using UnityEngine;

namespace Game.Scripts.Player.Movement
{
    public interface IPlayerController
    {
        public float MoveSpeed { get; }
        public float RotationSpeed { get; }
        
        public CharacterController Character { get; }
        public Transform Transform { get; }
        public Animator Animator { get; }
    }
}