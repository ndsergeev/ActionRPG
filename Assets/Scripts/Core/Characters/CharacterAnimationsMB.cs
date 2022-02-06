namespace Main.Core
{
    using UnityEngine;
    using Main.Core.Updates;
    using Main.Core.Input;
    
    public class CharacterAnimationsMB : Refresh
    {
        [SerializeField]
        protected Animator m_Anim;

        protected CharacterMB m_Character;

        protected static readonly int m_WalkingAnimProperty = Animator.StringToHash("isWalking");
        protected static readonly int m_JumpinggAnimProperty = Animator.StringToHash("isJumping");
        protected static readonly int m_FallingAnimProperty = Animator.StringToHash("isFalling");
        protected static readonly int m_WalkXBlenddAnimProperty = Animator.StringToHash("WalkXBlend");
        protected static readonly int m_WalkYBlenddAnimProperty = Animator.StringToHash("WalkYBlend");
        protected static readonly int m_CrouchBlendAnimProperty = Animator.StringToHash("CrouchBlend");

        protected readonly Vector2 WALK_WALKBLEND_POS = new Vector2(0, 0.433f);
        protected readonly Vector2 RUN_WALKBLEND_POS = new Vector2(0.5f, -0.433f);
        protected readonly Vector2 CROUCH_WALKBLEND_POS = new Vector2(-0.5f, -0.433f);

        protected enum m_WalkTypes
        {
            Walk,
            Run,
            Crouch
        }

        protected m_WalkTypes m_CurrentWalkType; // Walk type that we are blending to
        protected m_WalkTypes m_TargetWalkType; // Walk type that we are blending from

        protected bool m_IsWalkBlending;
        
        protected Vector2 m_CurrentWalkBlendPos;
        protected Vector2 m_WalkBlendOrigin = new Vector2();
        protected Vector2 m_WalkBlendTarget = new Vector2();

        protected float m_WalkBlendTimer = 0;
        protected float m_CrouchBlendTimer = 0;
        
        protected const float m_BlendToWalkTime = 0.15f;
        protected const float m_BlendToRunTime = 0.15f;
        protected const float m_BlendToCrouchTime = 0.15f;

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
            
            BlendToWalkType(m_WalkTypes.Walk);
        }

        public virtual void StopWalking()
        {
            SetWalking(false);
        }

        protected virtual void SetWalking(bool state)
        {
            m_Anim.SetBool(m_WalkingAnimProperty, state);
        }

        /// <summary>
        /// Blends between Walking | Running | CrouchWalking
        /// </summary>
        protected virtual void SetWalkBlend(Vector2 walkBlend)
        {
            m_Anim.SetFloat(m_WalkXBlenddAnimProperty, walkBlend.x);
            m_Anim.SetFloat(m_WalkYBlenddAnimProperty, walkBlend.y);
        }

        protected virtual void BlendToWalkType(m_WalkTypes walkType)
        {
            m_TargetWalkType = walkType;

            m_WalkBlendTimer = 0;
            
            m_WalkBlendOrigin.x = m_Anim.GetFloat(m_WalkXBlenddAnimProperty);
            m_WalkBlendOrigin.y = m_Anim.GetFloat(m_WalkYBlenddAnimProperty);
            
            switch (m_TargetWalkType)
            {
                case m_WalkTypes.Walk:
                    m_WalkBlendTarget = WALK_WALKBLEND_POS;
                    break;
                case m_WalkTypes.Run:
                    m_WalkBlendTarget = RUN_WALKBLEND_POS;
                    break;
                case m_WalkTypes.Crouch:
                    m_WalkBlendTarget = CROUCH_WALKBLEND_POS;
                    break;
            }

            m_IsWalkBlending = true;
        }
        
        /// <summary>
        /// Blends between Walking | Running | CrouchWalking
        /// </summary>
        protected virtual void HandleWalkBlending()
        {
            if (!m_IsWalkBlending) return;

            float walkBlendTime = 0;
            
            switch (m_TargetWalkType)
            {
                case m_WalkTypes.Walk:
                    walkBlendTime = m_BlendToWalkTime;
                    break;
                case m_WalkTypes.Run:
                    walkBlendTime = m_BlendToRunTime;
                    break;
                case m_WalkTypes.Crouch:
                    walkBlendTime = m_BlendToCrouchTime;
                    break;
            }

            m_WalkBlendTimer += Time.deltaTime;

            if (m_WalkBlendTimer >= walkBlendTime)
            {
                m_IsWalkBlending = false;
                
                SetWalkBlend(m_WalkBlendTarget);

                return;
            }

            float blendPercent = m_WalkBlendTimer / walkBlendTime;
            
            m_CurrentWalkBlendPos = m_WalkBlendOrigin + (m_WalkBlendTarget - m_WalkBlendOrigin) * blendPercent;
            
            SetWalkBlend(m_CurrentWalkBlendPos);
        }
        
        // == RUNNING == //

        public virtual void StartRunning()
        {
            StartWalking();
            BlendToWalkType(m_WalkTypes.Run);
        }

        public virtual void StopRunning()
        {
            StopWalking();
        }
        
        // == CROUCHING == //

        public virtual void StartCrouching()
        {
            print("Start crouching");
            m_IsBlendingToCrouch = true;
            m_IsBlendingFromCrouch = false;

            m_CrouchBlendTimer = 0;
            
            BlendToWalkType(m_WalkTypes.Crouch);
        }

        public virtual void StopCrouching()
        {
            print("Stop crouching");
            m_IsBlendingFromCrouch = true;
            m_IsBlendingToCrouch = false;
            
            m_CrouchBlendTimer = 0;
        }

        protected virtual void HandleCrouchBlending()
        {
            float crouchBlend = 0;
            
            if (m_IsBlendingToCrouch)
            {
                crouchBlend = m_CrouchBlendTimer / m_BlendToCrouchTime;
            }
            else if (m_IsBlendingFromCrouch)
            {
                crouchBlend = 1 - (m_CrouchBlendTimer / m_BlendToCrouchTime);
            }
            
            SetCrouchBlend(crouchBlend);
        }

        protected virtual void SetCrouchBlend(float blendPercent)
        {
            m_Anim.SetFloat(m_CrouchBlendAnimProperty, blendPercent);
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
            m_Anim.SetBool(m_JumpinggAnimProperty, state);
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
            m_Anim.SetBool(m_FallingAnimProperty, state);
        }
    }
}
