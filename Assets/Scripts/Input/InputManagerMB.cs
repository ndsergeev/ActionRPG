using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

using Core;

namespace Main.Input
{
    public class InputManagerMB : SingletonMB<InputManagerMB>
    {
        private List<InputMB> m_inputReaders = new List<InputMB>();

        public void SubscribeInputReader(InputMB inputReader)
        {
            m_inputReaders.Add((inputReader));
        }
        
        public void UnsubscribeInputReader(InputMB inputReader)
        {
            m_inputReaders.Remove((inputReader));
        }
    }
}