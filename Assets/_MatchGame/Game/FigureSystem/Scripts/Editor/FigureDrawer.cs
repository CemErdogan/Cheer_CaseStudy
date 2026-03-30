using Abstractions.FigureSystem;
using UnityEditor;
using UnityEngine;

namespace Game.FigureSystem.Editor
{
    [CustomPropertyDrawer(typeof(FigureData))]
    public class FigureDrawer : PropertyDrawer
    {
        private const int CellSize = 14;
        private const int CellPad = 2;
        private const float ButtonHeight = 22f;
        private const float Pad = 4f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var sizeProp = property.FindPropertyRelative("<Size>k__BackingField");
            var rows = Mathf.Max(1, Mathf.RoundToInt(sizeProp.vector2Value.y));
            float gridHeight = rows * (CellSize + CellPad);
            return EditorGUIUtility.singleLineHeight + Pad + gridHeight + Pad + ButtonHeight + Pad;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(labelRect, label, EditorStyles.boldLabel);

            var sizeProp = property.FindPropertyRelative("<Size>k__BackingField");
            var pointsProp = property.FindPropertyRelative("<Points>k__BackingField");

            var cols = Mathf.Max(1, Mathf.RoundToInt(sizeProp.vector2Value.x));
            var rows = Mathf.Max(1, Mathf.RoundToInt(sizeProp.vector2Value.y));

            var gridTop = position.y + EditorGUIUtility.singleLineHeight + Pad;
            DrawGridPreview(position.x + 4f, gridTop, cols, rows, pointsProp);

            float gridHeight = rows * (CellSize + CellPad);
            var buttonRect = new Rect(position.x, gridTop + gridHeight + Pad, position.width, ButtonHeight);

            if (GUI.Button(buttonRect, "Edit Figure"))
            {
                FigureWindow.Open(property.Copy());
            }

            EditorGUI.EndProperty();
        }

        private static void DrawGridPreview(float x, float y, int cols, int rows, SerializedProperty pointsProp)
        {
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    var cellRect = new Rect(
                        x + col * (CellSize + CellPad),
                        y + row * (CellSize + CellPad),
                        CellSize,
                        CellSize
                    );

                    var cellColor = new Color(0.15f, 0.15f, 0.15f);
                    for (int i = 0; i < pointsProp.arraySize; i++)
                    {
                        var point = pointsProp.GetArrayElementAtIndex(i);
                        var coord = point.FindPropertyRelative("<Coord>k__BackingField");
                        
                        if (coord.vector2IntValue.x != col || coord.vector2IntValue.y != row) continue;
                        
                        var isActive = point.FindPropertyRelative("<IsActive>k__BackingField");
                        var colorType = point.FindPropertyRelative("<ColorType>k__BackingField");
                        cellColor = GetColor(colorType.intValue, isActive.boolValue);
                        break;
                    }

                    EditorGUI.DrawRect(cellRect, cellColor);
                }
            }
        }

        internal static Color GetColor(int colorIntValue, bool isActive)
        {
            if (!isActive) return new Color(0.2f, 0.2f, 0.2f);
            return colorIntValue switch
            {
                0 => new Color(0.55f, 0.55f, 0.55f),
                1 => Color.red,
                2 => Color.green,
                3 => Color.blue,
                4 => Color.yellow,
                _ => Color.white
            };
        }
    }
}
