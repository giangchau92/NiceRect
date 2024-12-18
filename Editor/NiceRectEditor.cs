
using UnityEditor;

namespace NiceRectUI
{
    [CustomEditor(typeof(NiceRect), true)]
    [CanEditMultipleObjects]
    public class NiceRectEditor : UnityEditor.Editor
    {
        private SerializedProperty _spTopLeft, _spTopRight, _spBottomLeft, _spBottomRight, _spCornerRadius;
        private SerializedProperty _spFillOption;
        private SerializedProperty _spStrokeType, _spStrokeWidth, _spStrokeOption;
        private SerializedProperty _spUseGrayScale, _spGrayScaleFactor;


        protected void OnEnable()
        {
            _spTopLeft = serializedObject.FindProperty("_topLeft");
            _spTopRight = serializedObject.FindProperty("_topRight");
            _spBottomLeft = serializedObject.FindProperty("_botLeft");
            _spBottomRight = serializedObject.FindProperty("_botRight");
            _spCornerRadius = serializedObject.FindProperty("_cornerRadius");
            _spFillOption = serializedObject.FindProperty("_fillOption");
            _spStrokeType = serializedObject.FindProperty("_strokeType");
            _spStrokeWidth = serializedObject.FindProperty("_strokeWidth");
            _spStrokeOption = serializedObject.FindProperty("_strokeOption");
            _spUseGrayScale = serializedObject.FindProperty("_useGrayScale");
            _spGrayScaleFactor = serializedObject.FindProperty("_grayScaleFactor");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            using (EditorGUI.ChangeCheckScope changedCheck = new EditorGUI.ChangeCheckScope())
            {
                EditorGUILayout.PropertyField(_spTopLeft);
                EditorGUILayout.PropertyField(_spTopRight);
                EditorGUILayout.PropertyField(_spBottomLeft);
                EditorGUILayout.PropertyField(_spBottomRight);

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(_spCornerRadius);
                EditorGUILayout.Space();

                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.PropertyField(_spFillOption);
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();

                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.PropertyField(_spStrokeType);
                if (_spStrokeType.enumValueIndex == (int)StrokeType.InnerStroke)
                {
                    EditorGUILayout.PropertyField(_spStrokeWidth);
                    EditorGUILayout.PropertyField(_spStrokeOption);
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();

                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.PropertyField(_spUseGrayScale);
                if (_spUseGrayScale.boolValue)
                {
                    EditorGUILayout.PropertyField(_spGrayScaleFactor);
                }
                EditorGUILayout.EndVertical();

                if (changedCheck.changed)
                {
                    serializedObject.ApplyModifiedProperties();
                    (serializedObject.targetObject as NiceRect)?.ValidateData();
                }
            }

        }
    }
}