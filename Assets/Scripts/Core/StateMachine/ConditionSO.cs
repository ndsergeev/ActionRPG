
namespace Main.Core.StateMachine
{
    using UnityEngine;
    
    /// <summary>
    /// Class to decompose Character condition logic from <see cref="CharacterMB"/>.
    /// Put condition-related fields into your 
    /// <see cref="ConditionSO"/>-inherited class to use memory efficiently. 
    /// </summary>
    public abstract class ConditionSO : ScriptableObject
    {
        public abstract bool CanTransit(CharacterMB character);
    }
}
