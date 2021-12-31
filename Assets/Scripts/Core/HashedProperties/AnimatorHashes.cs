
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
        
        // Parameter Names
        public static readonly int DefaultParameter = Animator.StringToHash("Default");
        
        // else
        // TODO: make a function to automatically check that there are no unused parameters
    }
}