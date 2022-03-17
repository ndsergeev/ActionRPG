
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
        public static readonly int MovingAnimProperty = Animator.StringToHash("IsMoving");
        public static readonly int JumpingAnimProperty = Animator.StringToHash("IsJumping");
        public static readonly int FallingAnimProperty = Animator.StringToHash("IsFalling");
        public static readonly int WalkXBlendAnimProperty = Animator.StringToHash("WalkRunX");
        public static readonly int WalkYBlendAnimProperty = Animator.StringToHash("WalkRunY");
        public static readonly int CrouchBlendAnimProperty = Animator.StringToHash("CrouchBlend");
        
        // else
        // TODO: make a function to automatically check that there are no unused parameters
    }
}