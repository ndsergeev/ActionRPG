namespace Main.Core
{
    using Main.Core.Updates;
    
    public class MovementMB : Refresh, IRefresh
    {
        protected CharacterMB m_Character;

        public virtual void Run() { }
    }
}
