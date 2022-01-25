namespace Main.Core
{
    using UnityEngine;
    using Main.Core.Updates;
    using Main.Core.Input;
    
    public class CharacterAnimationsMB : Refresh
    {
        [SerializeField]
        protected Animator Anim;

        protected CharacterMB character;

        protected static readonly int walkingAnimProperty = Animator.StringToHash("isWalking");
        protected static readonly int jumpinggAnimProperty = Animator.StringToHash("isJumping");
        protected static readonly int fallingAnimProperty = Animator.StringToHash("isFalling");
        protected static readonly int walkRunBlendAnimProperty = Animator.StringToHash("walkRunBlend");

        protected bool isBlendingToRun;
        protected const float blendWalkToRunTime = 5f;
        protected float blendWalkToRunTimer = 0;
        
        protected virtual void Awake()
        {
            character = GetComponent<CharacterMB>();
        }

        protected virtual void Update()
        {
            HandleBlendWalkToRun();
        }
        
        // == WALKING == //
        
        public virtual void StartWalking()
        {
            SetWalking(true);
        }

        public virtual void StopWalking()
        {
            SetWalking(false);
        }

        protected virtual void SetWalking(bool state)
        {
            Anim.SetBool(walkingAnimProperty, state);
        }

        // == RUNNING == //

        public virtual void StartRunning()
        {
            // TODO: Check if previous state was walk state to decide whether to blend walk anim to run anim
            
            if (Anim.GetBool(walkingAnimProperty))
            {
                isBlendingToRun = true;
            }
            else
            {
                StartWalking();
                SetRunBlend(1f);
            }
        }

        protected virtual void HandleBlendWalkToRun()
        {
            if (!isBlendingToRun) return;

            blendWalkToRunTimer += Time.deltaTime;

            if (blendWalkToRunTimer >= blendWalkToRunTime)
            {
                // Finished blending
                isBlendingToRun = false;
                blendWalkToRunTimer = 0f;
                
                SetRunBlend(1f);
            }
            else
            {
                float blendPercent = blendWalkToRunTimer / blendWalkToRunTime;
                
                SetRunBlend(blendPercent);
            }
        }

        public virtual void StopRunning()
        {
            StopWalking();
            SetRunBlend(0f);
        }
        
        protected virtual void SetRunBlend(float blendPercent)
        {
            Mathf.Clamp(blendPercent, 0, 1);
            
            Anim.SetFloat(walkRunBlendAnimProperty, blendPercent);
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
            Anim.SetBool(jumpinggAnimProperty, state);
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
            Anim.SetBool(fallingAnimProperty, state);
        }

        
        
    }
}
