
using System.Collections.Generic;

namespace Main.Core.Input
{
    using Main.Core;
    
    public class InputManagerMB : SingletonMB<InputManagerMB>
    {
        private readonly List<InputMB> m_InputReaders = new List<InputMB>();

        public void SubscribeInputReader(InputMB inputReader)
            => m_InputReaders.Add(inputReader);
        
        public void UnsubscribeInputReader(InputMB inputReader)
            => m_InputReaders.Remove(inputReader);
        
        // TODO: Timer for button combos
    }
}