
namespace Main.Core
{
    using UnityEngine;
   
    using Main.Core.Console;
    using Main.Core.HashedProperties;
    using Main.Core.Updates;
    
    public class CharacterAnimationsMB : Refresh
    {
        [SerializeField]
        protected Animator m_Animator;

        protected CharacterMB m_Character;

        // TODO: make SO settings
        protected readonly Vector2 m_WalkWalkBlendPos = new Vector2(0, 0.433f);
        protected readonly Vector2 m_RunWalkBlendPos = new Vector2(0.5f, -0.433f);
        protected readonly Vector2 m_CrouchWalkBlendPos = new Vector2(-0.5f, -0.433f);

        protected enum WalkTypes
        {
            Walk,
            Run,
            Crouch
        }

        protected WalkTypes m_CurrentWalkType; // Walk type that we are blending to
        protected WalkTypes m_TargetWalkType; // Walk type that we are blending from

        protected bool m_IsWalkBlending;
        
        protected Vector2 m_CurrentWalkBlendPos;
        protected Vector2 m_WalkBlendOrigin;
        protected Vector2 m_WalkBlendTarget;

        protected float m_WalkBlendTimer;
        protected float m_CrouchBlendTimer;
        
        protected const float BLEND_TO_WALK_TIME = 0.15f;
        protected const float BLEND_TO_RUN_TIME = 0.15f;
        protected const float BLEND_TO_CROUCH_TIME = 0.15f;

        protected bool m_IsBlendingToCrouch;
        protected bool m_IsBlendingFromCrouch;
        
        protected virtual void Awake()
        {
            m_Character = GetComponent<CharacterMB>();
        }

        protected virtual void Update()
        {
            HandleWalkBlending();
            HandleCrouchBlending();
        }
        
        // == WALKING == //
        public virtual void StartWalking()
        {
            SetWalking(true);
            
            BlendToWalkType(WalkTypes.Walk);
        }

        public virtual void StopWalking()
        {
            SetWalking(false);
        }

        protected virtual void SetWalking(bool state)
        {
            m_Animator.SetBool(AnimatorHashes.WalkingAnimProperty, state);
        }

        /// <summary>
        /// Blends between Walking | Running | CrouchWalking
        /// </summary>
        protected virtual void SetWalkBlend(Vector2 walkBlend)
        {
            m_Animator.SetFloat(AnimatorHashes.WalkXBlendAnimProperty, walkBlend.x);
            m_Animator.SetFloat(AnimatorHashes.WalkYBlendAnimProperty, walkBlend.y);
        }

        protected virtual void BlendToWalkType(WalkTypes walkType)
        {
            m_TargetWalkType = walkType;

            m_WalkBlendTimer = 0;
            
            m_WalkBlendOrigin.x = m_Animator.GetFloat(AnimatorHashes.WalkXBlendAnimProperty);
            m_WalkBlendOrigin.y = m_Animator.GetFloat(AnimatorHashes.WalkYBlendAnimProperty);
            
            switch (m_TargetWalkType)
            {
                case WalkTypes.Walk:
                    m_WalkBlendTarget = m_WalkWalkBlendPos;
                    break;
                case WalkTypes.Run:
                    m_WalkBlendTarget = m_RunWalkBlendPos;
                    break;
                case WalkTypes.Crouch:
                    m_WalkBlendTarget = m_CrouchWalkBlendPos;
                    break;
            }

            m_IsWalkBlending = true;
        }
        
        /// <summary>
        /// Blends between Walking | Running | CrouchWalking
        /// </summary>
        protected virtual void HandleWalkBlending()
        {
            if (!m_IsWalkBlending)
                return;

            float walkBlendTime = 0;
            
            switch (m_TargetWalkType)
            {
                case WalkTypes.Walk:
                    walkBlendTime = BLEND_TO_WALK_TIME;
                    break;
                case WalkTypes.Run:
                    walkBlendTime = BLEND_TO_RUN_TIME;
                    break;
                case WalkTypes.Crouch:
                    walkBlendTime = BLEND_TO_CROUCH_TIME;
                    break;
            }

            m_WalkBlendTimer += Time.deltaTime;

            if (m_WalkBlendTimer >= walkBlendTime)
            {
                m_IsWalkBlending = false;
                
                SetWalkBlend(m_WalkBlendTarget);

                return;
            }

            var blendPercent = m_WalkBlendTimer / walkBlendTime;
            
            m_CurrentWalkBlendPos = m_WalkBlendOrigin + (m_WalkBlendTarget - m_WalkBlendOrigin) * blendPercent;
            
            SetWalkBlend(m_CurrentWalkBlendPos);
        }
        
        // == RUNNING == //
        public virtual void StartRunning()
        {
            StartWalking();
            BlendToWalkType(WalkTypes.Run);
        }

        public virtual void StopRunning()
        {
            StopWalking();
        }
        
        // == CROUCHING == //
        public virtual void StartCrouching()
        {
            m_IsBlendingToCrouch = true;
            m_IsBlendingFromCrouch = false;

            m_CrouchBlendTimer = 0;
            
            BlendToWalkType(WalkTypes.Crouch);
        }

        public virtual void StopCrouching()
        {
            m_IsBlendingFromCrouch = true;
            m_IsBlendingToCrouch = false;
            
            m_CrouchBlendTimer = 0;
        }

        protected virtual void HandleCrouchBlending()
        {
            float crouchBlend = 0;
            
            if (m_IsBlendingToCrouch)
            {
                crouchBlend = m_CrouchBlendTimer / BLEND_TO_CROUCH_TIME;
            }
            else if (m_IsBlendingFromCrouch)
            {
                crouchBlend = 1 - (m_CrouchBlendTimer / BLEND_TO_CROUCH_TIME);
            }
            
            SetCrouchBlend(crouchBlend);
        }

        protected virtual void SetCrouchBlend(float blendPercent)
        {
            m_Animator.SetFloat(AnimatorHashes.CrouchBlendAnimProperty, blendPercent);
        }
        
        // == JUMPING == //
        public virtual void StartJumping()
        {
            SetJumping(true);
        }

        public virtual void StopJumping()
        {
            SetJumping(false);
        }

        protected virtual void SetJumping(bool state)
        {
            m_Animator.SetBool(AnimatorHashes.JumpingAnimProperty, state);
        }
        
        // == FALLING == //
        public virtual void StartFalling()
        {
            SetFalling(true);
        }

        public virtual void StopFalling()
        {
            SetFalling(false);
        }

        protected virtual void SetFalling(bool state)
        {
            m_Animator.SetBool(AnimatorHashes.FallingAnimProperty, state);
        }
    }
}
