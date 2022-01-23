
using System;
using System.Collections;

namespace Main.Core.Editor
{
    using UnityEditor;
    using UnityEditorInternal;
    
    internal class CustomReorderableList : ReorderableList
    {
        public CustomReorderableList(IList elements, Type elementType)
            : base(elements, elementType)
        {
        }

        public CustomReorderableList(IList elements, Type elementType, bool draggable,
            bool displayHeader, bool displayAddButton, bool displayRemoveButton)
            : base(elements, elementType, draggable, displayHeader, displayAddButton, displayRemoveButton)
        {
        }

        public CustomReorderableList(SerializedObject serializedObject, SerializedProperty elements)
            : base(serializedObject, elements)
        {
        }

        public CustomReorderableList(SerializedObject serializedObject, SerializedProperty elements, bool draggable,
            bool displayHeader, bool displayAddButton, bool displayRemoveButton, float elementHeight)
            : base(serializedObject, elements, draggable, displayHeader, displayAddButton, displayRemoveButton)
        {
            this.elementHeight = elementHeight;

            // TODO: draw every mod 2 element the same colour
        }
    }
}