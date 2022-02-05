namespace Main.Core
{
    using UnityEngine;
    using Main.Core.HashedProperties;
    using Main.Core.Updates;
    
    public class CharacterAnimationsMB : Refresh
    {
        [SerializeField]
        protected Animator Anim;

        protected CharacterMB Character;

        protected bool IsBlendingToRun;
        protected const float BlendWalkToRunTime = 0.15f; // TODO: use SO
        protected float BlendWalkToRunTimer = 0;

        protected bool IsBlendingToWalk;
        protected const float BlendRunToWalkTime = 0.15f; // TODO: use SO
        protected float BlendRunToWalkTimer = 0;
        
        
        protected virtual void Awake()
        {
            Character = GetComponent<CharacterMB>();
        }

        protected virtual void Refresh()
        {
            HandleBlendWalkToRun();
            HandleBlendRunToWalk();
        }
        
        // == WALKING == //
        
        public virtual void StartWalking()
            => SetWalking(true);

        public virtual void StopWalking()
            => SetWalking(false);

        protected virtual void SetWalking(bool state)
            => Anim.SetBool(AnimatorHashes.WalkingAnimProperty, state);

        protected virtual void HandleBlendRunToWalk()
        {
            if (!IsBlendingToWalk)
                return;

            BlendRunToWalkTimer += Time.deltaTime;

            if (BlendRunToWalkTimer >= BlendRunToWalkTime)
            {
                // Finished blending
                IsBlendingToWalk = false;
                SetRunBlend(0f);
            }
            else
            {
                var blendPercent = 1 - (BlendRunToWalkTimer / BlendRunToWalkTime);
                SetRunBlend(blendPercent);
            }
        }
        
        // == RUNNING == //

        public virtual void StartRunning()
        {
            StartWalking();
            IsBlendingToRun = true;
            BlendWalkToRunTimer = 0f;

            /*
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
            */
        }

        public virtual void StopRunning()
        {
            StopWalking();
            
            IsBlendingToRun = false;
            IsBlendingToWalk = true;
            BlendRunToWalkTimer = 0f;
        }
        
        protected virtual void SetRunBlend(float blendPercent)
        {
            Mathf.Clamp(blendPercent, 0, 1);
            Anim.SetFloat(AnimatorHashes.WalkRunBlendAnimProperty, blendPercent);
        }
        
        protected virtual void HandleBlendWalkToRun()
        {
            if (!IsBlendingToRun)
                return;

            BlendWalkToRunTimer += Time.deltaTime;

            if (BlendWalkToRunTimer >= BlendWalkToRunTime)
            {
                // Finished blending
                IsBlendingToRun = false;
                SetRunBlend(1f);
            }
            else
            {
                var blendPercent = BlendWalkToRunTimer / BlendWalkToRunTime;
                SetRunBlend(blendPercent);
            }
        }
        
        // == JUMPING == //
        
        public virtual void StartJumping()
            => SetJumping(true);

        public virtual void StopJumping()
            => SetJumping(false);

        protected virtual void SetJumping(bool state)
            => Anim.SetBool(AnimatorHashes.JumpingAnimProperty, state);
        
        // == FALLING == //
        
        public virtual void StartFalling()
            => SetFalling(true);

        public virtual void StopFalling()
            => SetFalling(false);

        protected virtual void SetFalling(bool state)
            => Anim.SetBool(AnimatorHashes.FallingAnimProperty, state);
    }
}
