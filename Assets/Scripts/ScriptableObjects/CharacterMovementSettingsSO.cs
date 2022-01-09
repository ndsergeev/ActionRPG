
namespace Main.Characters
{
    using UnityEngine;
    
    [CreateAssetMenu(fileName = "CharacterMovementSettingsSO", menuName = "Scriptable Objects/New CharacterMovementSettingsSO", order = 10)]
    public class CharacterMovementSettingsSO : ScriptableObject
    {
        [Header("GROUNDING")]
        [SerializeField]
        protected float m_snapToGroundDistance;
        
        public float SnapToGroundDistance => m_snapToGroundDistance;
        
        [Header(("MOVEMENT"))]
        [SerializeField]
        protected float m_speed;
        
        public float Speed => m_speed;

        [Header("ROTATION")]
        [SerializeField]
        protected float m_turnSmoothTime;
        
        public float TurnSmoothTime => m_turnSmoothTime;

        [Header("JUMPING")]
        [SerializeField]
        protected float m_jumpPower;
        
        [SerializeField]
        protected float lowJumpMultiplier;
        
        public float JumpPower => m_jumpPower * 100;
        public float LowJumpMultiplier => lowJumpMultiplier;
        
        [Header("FALLING")]
        [SerializeField]
        protected float fallMultiplier;
        public float FallMultiplier => fallMultiplier;
    }
}
