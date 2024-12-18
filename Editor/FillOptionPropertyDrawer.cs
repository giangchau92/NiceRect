using UnityEditor;
using UnityEngine;

namespace NiceRectUI
{
    [CustomPropertyDrawer(typeof(FillOption))]
    public class FillOptionPropertyDrawer : PropertyDrawer
    {
        private int currentLine = 0;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            currentLine = 0;
            EditorGUI.BeginProperty(position, label, property);
            {
                // Rect LabelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
                SerializedProperty fillType = property.FindPropertyRelative("fillType");
                SerializedProperty color = property.FindPropertyRelative("color");
                SerializedProperty gradientType = property.FindPropertyRelative("gradientType");
                SerializedProperty gradient = property.FindPropertyRelative("gradient");
                SerializedProperty gradientRotation = property.FindPropertyRelative("gradientRotation");
                SerializedProperty gradientScale = property.FindPropertyRelative("gradientScale");

                // EditorGUI.LabelField(LabelRect, label);

                EditorGUI.BeginChangeCheck();
                {
                    EditorGUI.PropertyField(GetRectLine(position), fillType);
                    if (fillType.intValue == (int)FillType.SolidColor)
                    {
                        EditorGUI.PropertyField(GetRectLine(position), color);
                    }

                    if (fillType.intValue == (int)FillType.Gradient)
                    {
                        EditorGUI.PropertyField(GetRectLine(position), gradientType);
                        EditorGUI.PropertyField(GetRectLine(position), gradient);
                        EditorGUI.PropertyField(GetRectLine(position), gradientScale);
                        if (gradientType.intValue == (int)GradientType.Linear)
                        {
                            EditorGUI.PropertyField(GetRectLine(position), gradientRotation);
                        }
                    }
                }
                if (EditorGUI.EndChangeCheck())
                {
                    property.serializedObject.ApplyModifiedProperties();
                }
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * currentLine + EditorGUIUtility.standardVerticalSpacing * currentLine;
        }

        private Rect GetRectLine(Rect position)
        {
            var rect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * currentLine + EditorGUIUtility.standardVerticalSpacing * currentLine, position.width, EditorGUIUtility.singleLineHeight);
            currentLine++;
            return rect;
        }
    }
}