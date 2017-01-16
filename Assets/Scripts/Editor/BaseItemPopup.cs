using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(BaseItemPopupAttribute))]
public class BaseItemPopup : PropertyDrawer 
{
    private bool start = true;
    private string[] choices;

    /// <summary>
    /// Overrides the OnGUI method and renders a dropdown to pick an available baseItem.
    /// </summary>
    /// <param name="position">Position of dropdown.</param>
    /// <param name="property">Property to set.</param>
    /// <param name="label">Property label.</param>
    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        if (start)
        {
            ItemFactory itemFactory = new ItemFactory();

            // TODO: limit items by category

            choices = (new List<string>(itemFactory.ItemDatabase.Keys)).ToArray();
            start = false;
        }

        int selected = Array.IndexOf(choices, property.stringValue);

        if (selected < 0)
        {
            selected = 0;
        }

        selected = EditorGUI.Popup(EditorGUI.PrefixLabel(position, label), selected, choices);

        property.stringValue = choices[selected];
    }
}
