
namespace Main.Core.Editor
{
    using UnityEditor;
    using UnityEngine;

    using Main.Core.StateMachine;

    [CustomEditor(typeof(StateTableSO))]
    public class StateTableSOEditor : Editor
    {
        private SerializedProperty m_states = default;
        private CustomReorderableList m_statesReorderableList = default;

        private Editor m_stateEditor = null;

        private DrawingEditor m_currentDrawingEditor = DrawingEditor.StateTable;
        
        private void OnEnable()
        {
            Init();
        }

        private void OnDisable()
        {
            if (m_stateEditor != null)
            {
                ToDrawingStateTable();
            }
        }
        
        private void Init()
        {
            m_states = serializedObject.FindProperty(nameof(StateTableSO.states));

            m_statesReorderableList = new CustomReorderableList(serializedObject, m_states,
                true, true, true, true, EditorGUIUtility.singleLineHeight)
            {
                drawElementCallback = DrawStates,
                drawHeaderCallback = DrawHeader
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            switch (m_currentDrawingEditor)
            {
                case DrawingEditor.StateTable:
                    DrawScriptProperty();
                    m_statesReorderableList.DoLayoutList();
                    break;
                case DrawingEditor.State:
                    DrawSelectedStateHeader();
                    m_stateEditor.OnInspectorGUI();
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawSelectedStateHeader()
        {
            var selectedSO = Selection.activeObject;
            var returnButtonName = selectedSO != null ? "Return to " + selectedSO.name : "Return";
            if (GUILayout.Button(returnButtonName, GUILayout.Height(EditorGUIUtility.singleLineHeight * 2)))
            {
                ToDrawingStateTable();
                return;
            }
            GUILayout.Space(5);
        }

        protected internal void DrawScriptProperty()
        {
            EditorGUILayout.Space();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();
        }

        protected internal static void DrawHeader(Rect rect)
        {
            const string stateTableName = "State Table";
            EditorGUI.LabelField(rect, stateTableName);
        }

        protected internal void DrawStates(Rect rect, int index, bool isActive, bool isFocused)
        {
            var state = m_statesReorderableList.serializedProperty.GetArrayElementAtIndex(index);
            if (state == null)
                return;
            
            // TODO: replace these hardcoded values with meaningful constants
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, EditorGUIUtility.currentViewWidth - EditorGUIUtility.fieldWidth - 65,
                    EditorGUIUtility.singleLineHeight),
                state,
                new GUIContent(index.Equals(0) ? "Default" : $"State {index}")
            );
            
            var stateObjectReferenceValue = state.objectReferenceValue as StateSO;
            if (stateObjectReferenceValue == null)
            {
                return;
            }
            
            // TODO: replace these hardcoded values with meaningful constants
            var buttonRect = new Rect(EditorGUIUtility.currentViewWidth - EditorGUIUtility.fieldWidth - 20,
                rect.y, 
                60, 
                EditorGUIUtility.singleLineHeight);
            
            if (GUI.Button(buttonRect, "Edit"))
            { 
                ToDrawingState(stateObjectReferenceValue);
            }
            
            // TODO: draw transitions straight in the list with dynamic list item size
            // var stateObj = new SerializedObject(stateObjectReferenceValue);
            // var transitions = state.FindPropertyRelative("transitions");
            // if (transitions == null)
            //     return;
            //
            // var showPosition = EditorGUI.Foldout(new Rect(3, 3, 10, EditorGUIUtility.singleLineHeight), true, "");
            //
            // for (var i = 0; i < transitions.arraySize; ++i)
            // {
            //     var transition = transitions.GetArrayElementAtIndex(i);
            //     if (transition == null)
            //         continue;
            //
            //     EditorGUI.PropertyField(
            //         new Rect(rect.x + 10,
            //             rect.y + EditorGUIUtility.singleLineHeight + i * EditorGUIUtility.singleLineHeight,
            //             EditorGUIUtility.currentViewWidth - EditorGUIUtility.fieldWidth,
            //             EditorGUIUtility.singleLineHeight),
            //         transition,
            //         new GUIContent($"Transition {i}")
            //     );
            // }

            // stateObj.ApplyModifiedProperties();
        }

        private void ToDrawingStateTable()
        {
            DestroyImmediate(m_stateEditor);
            m_currentDrawingEditor = DrawingEditor.StateTable;
        }

        private void ToDrawingState(Object stateObject)
        {
            m_stateEditor = Editor.CreateEditor(stateObject);
            m_currentDrawingEditor = DrawingEditor.State;
        }

        internal enum DrawingEditor
        {
            StateTable = 0,
            State
        }
    }
}