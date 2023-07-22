/*
 * EditorFunctionCall.cs
 * 
 * Description:
 * - Call in script to be able to call a function trough editor
 * 
 * Author(s): 
 * - Kody Wood
*/

using System;

namespace RubberDucks.Utilities
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EditorFunctionCall : Attribute
    {
        public string label;
        public EditorFunctionCall(string label = "")
        {
            this.label = label;
        }
    }
}
