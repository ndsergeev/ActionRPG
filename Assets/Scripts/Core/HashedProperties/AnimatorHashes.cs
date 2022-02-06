
namespace Main.Core.HashedProperties
{
    using Animator = UnityEngine.Animator;
    
    /// <summary>
    /// Class that contains Animator Properties as an int hashed version
    /// </summary>
    public static class AnimatorHashes
    {
        // Animation Names
        public static readonly int DefaultAnimation = Animator.StringToHash("Default");
        
        // Property Names:
        /// <summary> <see cref="CharacterAnimationsMB"/>> </summary>
        public static readonly int WalkingAnimProperty = Animator.StringToHash("isWalking");
        public static readonly int JumpingAnimProperty = Animator.StringToHash("isJumping");
        public static readonly int FallingAnimProperty = Animator.StringToHash("isFalling");
        public static readonly int WalkRunBlendAnimProperty = Animator.StringToHash("walkRunBlend");
        
        // else
        // TODO: make a function to automatically check that there are no unused parameters
    }
}