
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
        protected float m_WalkSpeed;
        [SerializeField]
        protected float m_RunSpeed;
        [SerializeField]
        protected float m_AirSpeed;
        
        public float WalkSpeed => m_WalkSpeed;
        public float RunSpeed => m_RunSpeed;
        public float AirSpeed => m_AirSpeed;

        [Header("ROTATION")]
        [SerializeField]
        protected float m_turnSmoothTime;
        
        public float TurnSmoothTime => m_turnSmoothTime;

        [Header("OBSTACLES")]
        [SerializeField]
        protected float stepHeight;

        [SerializeField]
        protected float obstacleCheckSize;
        
        public float StepHeight => stepHeight;
        public float ObstacleCheckSize => obstacleCheckSize;
        
        
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

        [Header("SLOPES")]
        [SerializeField]
        protected float m_MaxSlopeAngle;

        public float maxSlopeAngle
        {
            get => m_MaxSlopeAngle;
            set => m_MaxSlopeAngle = value;
        }
    }
}
