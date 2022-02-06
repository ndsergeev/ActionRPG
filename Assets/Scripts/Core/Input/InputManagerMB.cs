
using System.Collections.Generic;

namespace Main.Core.Input
{
    using Main.Core;
    
    public class InputManagerMB : SingletonMB<InputManagerMB>
    {
        private readonly List<InputMB> m_inputReaders = new List<InputMB>();

        public void SubscribeInputReader(InputMB inputReader)
            => m_inputReaders.Add(inputReader);
        
        public void UnsubscribeInputReader(InputMB inputReader)
            => m_inputReaders.Remove(inputReader);
        
        // TODO: Timer for button combos
    }
}