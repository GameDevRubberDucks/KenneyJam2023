/*
 * EditorActionButton.cs
 * 
 * Description:
 * - Creates a button in the Editor to press
 * 
 * Author(s): 
 * - Kody Wood
*/
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace RubberDucks.Utilities
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Object), true)] // Target all ScriptableObjects, MonoBehaviours and descendants
    public class EditorActionButton : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var type = target.GetType();
            foreach (var method in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                var attribute = method.GetCustomAttribute<EditorFunctionCall>(true);
                if (attribute == null) continue;
                if (GUILayout.Button(attribute.label != string.Empty ? attribute.label : method.Name))
                {
                    method.Invoke(target, new object[] { });
                    EditorUtility.SetDirty(target);
                    AssetDatabase.SaveAssets();
                }
            }
        }
    }
}