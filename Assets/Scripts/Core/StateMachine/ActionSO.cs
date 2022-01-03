
namespace Main.Core.StateMachine
{
    using UnityEngine;
    
    /// <summary>
    /// Class to decompose Character action logic from <see cref="CharacterMB"/>.
    /// Put action-related fields into your 
    /// <see cref="ActionSO"/>-inherited class to use memory efficiently. 
    /// </summary>
    public abstract class ActionSO : ScriptableObject
    {
        public abstract void OnEnter(CharacterMB character);
        public abstract void OnUpdate(CharacterMB character);
        public abstract void OnExit(CharacterMB character);
    }
}