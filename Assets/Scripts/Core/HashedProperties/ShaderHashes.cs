
namespace Main.Core.HashedProperties
{
    using Shader = UnityEngine.Shader;
    
    /// <summary>
    /// Class that contains Shader Properties as an int hashed version
    /// </summary>
    public static class ShaderHashes
    {
        public static readonly int ColorProperty = Shader.PropertyToID("_Color");
        
        // TODO: make a function to automatically check that there are no unused parameters
    }
}