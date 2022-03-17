using UnityEditorInternal;

namespace Main.Core
{
    using UnityEngine;
   
    using Main.Core.Console;
    using Main.Core.HashedProperties;
    using Main.Core.Updates;
    
    public class CharacterAnimationsMB : Refresh, IRefresh
    {
        [SerializeField]
        protected Animator m_Animator;

        protected CharacterMB m_Character;

        // TODO: make SO settings

        protected bool m_IsMoving;
        protected bool m_IsWalking;
        protected bool m_IsRunning;

        protected Vector2 m_PreIdleWalkRunBlend;
        protected Vector2 m_IdleWalkRunBlend;

        protected bool m_IsBlendingIdleWalkRun;
        
        protected const float BLEND_TO_IDLE_TIME = 0.15f;
        protected const float BLEND_TO_WALK_TIME = 0.15f;
        protected const float BLEND_TO_RUN_TIME = 0.15f;
        protected const float BLEND_TO_CROUCH_TIME = 0.15f;
        
        protected float m_IdleWalkRunBlendTimer;
        protected float m_CrouchBlendTimer;

        [SerializeField]
        protected float m_WalkWhileTargetingBlendSpeed = 1f;

        protected enum m_IdleWalkRunBlendStates
        {
            Idle,
            Walk,
            Run,
        }

        protected m_IdleWalkRunBlendStates m_IdleWalkRunBlendState;
        
        protected bool m_IsBlendingToCrouch;
        protected bool m_IsBlendingFromCrouch;
        
        protected virtual void Awake()
        {
            m_Character = GetComponent<CharacterMB>();

            Initialize();
        }

        public virtual void Run()
        {
            if (m_Character.isTargeting)
            {
                HandleIdleWalkRunBlendingWhileTargeting();
            }
            else
            {
                HandleIdleWalkRunBlending();
            }
            
            HandleCrouchBlending();
        }
        
        protected virtual void Initialize()
        {
            m_IdleWalkRunBlend = new Vector2(0, 0);
        }
        
        // == IDLE / WALKING / RUNNING == //
        public virtual void StartWalking()
        {
            m_IsWalking = true;
            
            BlendToIdleWalkRun(m_IdleWalkRunBlendStates.Walk);
            
            SetMoving(true);
        }

        public virtual void StopWalking()
        {
            m_IsWalking = false;
            
            BlendToIdleWalkRun(m_IdleWalkRunBlendStates.Idle);
            
            SetMoving(false);
        }
        
        // == RUNNING == //
        public virtual void StartRunning()
        {
            m_IsRunning = true;

            BlendToIdleWalkRun(m_IdleWalkRunBlendStates.Run);
            
            SetMoving(true);
        }

        public virtual void StopRunning()
        {
            m_IsRunning = false;
            
            BlendToIdleWalkRun(m_IdleWalkRunBlendStates.Idle);
            
            SetMoving(false);
        }
        
        protected virtual void BlendToIdleWalkRun(m_IdleWalkRunBlendStates targetState)
        {
            m_PreIdleWalkRunBlend = m_IdleWalkRunBlend;

            m_IdleWalkRunBlendState = targetState;

            m_IdleWalkRunBlendTimer = 0;
            
            m_IsBlendingIdleWalkRun = true;
        }

        protected virtual void HandleIdleWalkRunBlending()
        {
            if (!m_IsBlendingIdleWalkRun)
                return;

            m_IdleWalkRunBlendTimer += Time.deltaTime;

            float blendPercent;
            
            switch (m_IdleWalkRunBlendState)
            {
                case m_IdleWalkRunBlendStates.Idle:

                    if (m_IdleWalkRunBlendTimer > BLEND_TO_IDLE_TIME)
                    {
                        m_IdleWalkRunBlendTimer = BLEND_TO_IDLE_TIME;

                        m_IsBlendingIdleWalkRun = false;
                    }

                    blendPercent = m_IdleWalkRunBlendTimer / BLEND_TO_IDLE_TIME;

                    m_IdleWalkRunBlend.y = Mathf.Lerp(m_PreIdleWalkRunBlend.y, 0.0f, blendPercent);
                    
                    break;
                
                case m_IdleWalkRunBlendStates.Walk:
                    
                    if (m_IdleWalkRunBlendTimer > BLEND_TO_WALK_TIME)
                    {
                        m_IdleWalkRunBlendTimer = BLEND_TO_WALK_TIME;

                        m_IsBlendingIdleWalkRun = false;
                    }
                    
                    blendPercent = m_IdleWalkRunBlendTimer / BLEND_TO_WALK_TIME;
                    
                    m_IdleWalkRunBlend.y = Mathf.Lerp(m_PreIdleWalkRunBlend.y, 0.5f, blendPercent);
                    
                    break;
                
                case m_IdleWalkRunBlendStates.Run:
                    
                    if (m_IdleWalkRunBlendTimer > BLEND_TO_RUN_TIME)
                    {
                        m_IdleWalkRunBlendTimer = BLEND_TO_RUN_TIME;

                        m_IsBlendingIdleWalkRun = false;
                    }
                    
                    blendPercent = m_IdleWalkRunBlendTimer / BLEND_TO_RUN_TIME;
                    
                    m_IdleWalkRunBlend.y = Mathf.Lerp(m_PreIdleWalkRunBlend.y, 1f, blendPercent);
                    
                    break;
            }

            m_IdleWalkRunBlend.x = 0;
            
            SetIdleWalkRunBlend(m_IdleWalkRunBlend);
        }

        protected virtual void HandleIdleWalkRunBlendingWhileTargeting()
        {
            if (!m_IsWalking && !m_IsRunning)
            {
                if (m_IdleWalkRunBlend.x != 0 || m_IdleWalkRunBlend.y != 0)
                {
                    m_IdleWalkRunBlend =
                        Vector2.Lerp(m_IdleWalkRunBlend, Vector2.zero, m_WalkWhileTargetingBlendSpeed * Time.deltaTime);
                }

                if (0 + Mathf.Abs(m_IdleWalkRunBlend.x) < 0.01)
                    m_IdleWalkRunBlend.x = 0;
                
                if (0 + Mathf.Abs(m_IdleWalkRunBlend.y) < 0.01)
                    m_IdleWalkRunBlend.y = 0;
                
                SetIdleWalkRunBlend(m_IdleWalkRunBlend);
                return;
            }

            Vector2 targetBlend = m_IdleWalkRunBlend;
            
            Vector3 facingDirection = CachedTransform.forward;
            Vector3 leftDirection = -CachedTransform.right;
            Vector3 rightDirection = CachedTransform.right;
            Vector3 moveDirection = m_Character.Movement.MoveDirection;

            float angleBetweenFaceDirAndMoveDir = Vector3.Angle(facingDirection, moveDirection);
            float angleBetweenMoveDirAndLeftDir = Vector3.Angle(moveDirection, leftDirection);
            float angleBetweenMoveDirAndRightDir = Vector3.Angle(moveDirection, rightDirection);

            bool isMovingLeft = false;
            bool isMovingRight = false;
            
            if (angleBetweenMoveDirAndLeftDir < angleBetweenMoveDirAndRightDir)
            {
                isMovingLeft = true;
            }
            else if (angleBetweenMoveDirAndRightDir < angleBetweenMoveDirAndLeftDir)
            {
                isMovingRight = true;
            }
            
            if (angleBetweenFaceDirAndMoveDir <= 45)
            {
                if (isMovingLeft)
                {
                    targetBlend.x = Mathf.Lerp(0, -0.5f, angleBetweenFaceDirAndMoveDir / 45);
                    targetBlend.y = 0.5f;
                    
                }
                else if (isMovingRight)
                {
                    targetBlend.x = Mathf.Lerp(0, 0.5f, angleBetweenFaceDirAndMoveDir / 45);
                    targetBlend.y = 0.5f;
                }
                else
                {
                    targetBlend.x = 0;
                    targetBlend.y = 0.5f;
                }
            }
            else if (angleBetweenFaceDirAndMoveDir > 45 && angleBetweenFaceDirAndMoveDir <= 90)
            {
                targetBlend.y = Mathf.Lerp(0.5f, 0, (angleBetweenFaceDirAndMoveDir - 45) / 45);
                
                if (isMovingLeft)
                {
                    targetBlend.x = -0.5f;
                    
                }
                else if (isMovingRight)
                {
                    targetBlend.x = 0.5f;
                }
            }
            else if (angleBetweenFaceDirAndMoveDir > 90 && angleBetweenFaceDirAndMoveDir <= 135)
            {
                targetBlend.y = Mathf.Lerp(0, -0.5f, (angleBetweenFaceDirAndMoveDir - 90) / 45);
                
                if (isMovingLeft)
                {
                    targetBlend.x = -0.5f;
                    
                }
                else if (isMovingRight)
                {
                    targetBlend.x = 0.5f;
                }
            }
            else if (angleBetweenFaceDirAndMoveDir > 135)
            {
                if (isMovingLeft)
                {
                    targetBlend.x = Mathf.Lerp(-0.5f, 0, (angleBetweenFaceDirAndMoveDir - 135) / 45);
                    targetBlend.y = -0.5f;
                    
                }
                else if (isMovingRight)
                {
                    targetBlend.x = Mathf.Lerp(0.5f, 0, (angleBetweenFaceDirAndMoveDir - 135) / 45);
                    targetBlend.y = -0.5f;
                }
                else
                {
                    targetBlend.x = 0;
                    targetBlend.y = -0.5f;
                }
            }
            
            // Blend
            m_IdleWalkRunBlend =
                Vector2.Lerp(m_IdleWalkRunBlend, targetBlend, m_WalkWhileTargetingBlendSpeed * Time.deltaTime);

            if (m_IdleWalkRunBlend.x < -0.5f)
                m_IdleWalkRunBlend.x = -0.5f;
            if (m_IdleWalkRunBlend.x > 0.5f)
                m_IdleWalkRunBlend.x = 0.5f;
            if (m_IdleWalkRunBlend.y < -0.5f)
                m_IdleWalkRunBlend.y = -0.5f;
            if (m_IdleWalkRunBlend.y > 0.5f)
                m_IdleWalkRunBlend.y = 0.5f;
            
            SetIdleWalkRunBlend(m_IdleWalkRunBlend);
        }

        protected virtual void SetMoving(bool state)
        {
            m_Animator.SetBool(AnimatorHashes.MovingAnimProperty, state);
        }
        
        protected virtual void SetIdleWalkRunBlend(Vector2 idleWalkRunBlend)
        {
            m_Animator.SetFloat(AnimatorHashes.WalkXBlendAnimProperty, idleWalkRunBlend.x);
            m_Animator.SetFloat(AnimatorHashes.WalkYBlendAnimProperty, idleWalkRunBlend.y);
        }
        // == CROUCHING == //
        public virtual void StartCrouching()
        {
            m_IsBlendingToCrouch = true;
            m_IsBlendingFromCrouch = false;

            m_CrouchBlendTimer = 0;
        }

        public virtual void StopCrouching()
        {
            m_IsBlendingFromCrouch = true;
            m_IsBlendingToCrouch = false;
            
            m_CrouchBlendTimer = 0;
        }

        protected virtual void HandleCrouchBlending()
        {
            if (!m_IsBlendingFromCrouch && !m_IsBlendingToCrouch)
                return;
            
            float crouchBlend = 0;

            m_CrouchBlendTimer += Time.deltaTime;
            
            if (m_IsBlendingToCrouch)
            {
                if (m_Animator.GetFloat(AnimatorHashes.CrouchBlendAnimProperty) == 1f)
                    return;
                
                crouchBlend = m_CrouchBlendTimer / BLEND_TO_CROUCH_TIME;

                if (crouchBlend >= 1)
                {
                    crouchBlend = 1;
                    m_IsBlendingToCrouch = false;
                }
            }
            else if (m_IsBlendingFromCrouch)
            {
                if (m_Animator.GetFloat(AnimatorHashes.CrouchBlendAnimProperty) == 0f)
                    return;
                
                crouchBlend = 1 - (m_CrouchBlendTimer / BLEND_TO_CROUCH_TIME);
                
                if (crouchBlend <= 0)
                {
                    crouchBlend = 0;
                    m_IsBlendingFromCrouch = false;
                }
            }
            else
            {
                return;
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
