
namespace Main.Input
{
    using System.Collections.Generic;

    using Core;
    
    public class InputManagerMB : SingletonMB<InputManagerMB>
    {
        private readonly List<InputMB> m_InputReaders = new List<InputMB>();

        public void SubscribeInputReader(InputMB inputReader)
            => m_InputReaders.Add(inputReader);
        
        public void UnsubscribeInputReader(InputMB inputReader)
            => m_InputReaders.Remove(inputReader);
    }
}