
using UnityEngine.Serialization;

namespace Main.Characters
{
    using UnityEngine;
    
    [CreateAssetMenu(fileName = "CharacterMovementSettingsSO", menuName = "Scriptable Objects/New CharacterMovementSettingsSO", order = 10)]
    public class CharacterMovementSettingsSO : ScriptableObject
    {
        [Header("GROUNDING")]
        [SerializeField]
        protected float m_SnapToGroundDistance;
        [SerializeField]
        private float m_SnapToGroundRadius;
        
        public float SnapToGroundDistance => m_SnapToGroundDistance;
        public float SnapToGroundRadius => m_SnapToGroundRadius;
        
        [Header(("MOVEMENT"))]
        [SerializeField]
        protected float m_WalkSpeed;
        [SerializeField]
        protected float m_RunSpeed;
        [SerializeField]
        protected float m_AirSpeed;
        [SerializeField]
        protected float m_CrouchWalkSpeed;
        
        public float WalkSpeed => m_WalkSpeed;
        public float RunSpeed => m_RunSpeed;
        public float AirSpeed => m_AirSpeed;
        public float CrouchWalkSpeed => m_CrouchWalkSpeed;

        [SerializeField]
        protected AnimationCurve m_MoveAgainstFacingDirectionCurve;

        public AnimationCurve MoveAgainstFacingDirectionCurve => m_MoveAgainstFacingDirectionCurve;
        
        [SerializeField]
        protected AnimationCurve m_MoveAgainstFacingDirectionWhileTargetingCurve;

        public AnimationCurve MoveAgainstFacingDirectionWhileTargetingCurve => m_MoveAgainstFacingDirectionWhileTargetingCurve;
        
        [Header("ROTATION")]
        [SerializeField]
        protected float m_TurnSmoothTime;

        [SerializeField]
        protected float m_TurnSmoothTimeWhileRunning;

        [SerializeField]
        protected float m_TurnSmoothTimeWhileInAir;
        
        public float TurnSmoothTime => m_TurnSmoothTime;
        public float TurnSmoothTimeWhileRunning => m_TurnSmoothTimeWhileRunning;
        public float TurnSmoothTimeWhileInAir => m_TurnSmoothTimeWhileInAir;

        [Header("OBSTACLES")]
        [SerializeField]
        protected float m_StepHeight;

        [SerializeField]
        protected float m_ObstacleCheckSize;
        
        public float StepHeight => m_StepHeight;
        public float ObstacleCheckSize => m_ObstacleCheckSize;
        
        [Header("JUMPING")]
        [SerializeField]
        protected float m_VerticalJumpPower;

        [SerializeField]
        protected float m_HorizontalJumpPower;
        
        [SerializeField]
        protected float m_LowJumpMultiplier;
        
        public float VerticalJumpPower => m_VerticalJumpPower;
        public float HorizontalJumpPower => m_HorizontalJumpPower;
        public float LowJumpMultiplier => m_LowJumpMultiplier;

        public float jumpDelay;
        
        [Header("FALLING")]
        [SerializeField]
        protected float m_FallMultiplier;
        [SerializeField]
        protected float m_MaxFallSpeed;

        public float FallMultiplier => m_FallMultiplier;
        public float MaxFallSpeed => m_MaxFallSpeed;
        
        [Header("AIR VELOCITY")]
        [SerializeField]
        protected float m_SpeedAirVelocityIsReducedAt;

        public float SpeedAirVelocityIsReducedAt => m_SpeedAirVelocityIsReducedAt;

        [Header("SLOPES")]
        [SerializeField]
        protected float m_MaxSlopeAngle;

        public float MaxSlopeAngle
        {
            get => m_MaxSlopeAngle;
            set => m_MaxSlopeAngle = value;
        }
    }
}
