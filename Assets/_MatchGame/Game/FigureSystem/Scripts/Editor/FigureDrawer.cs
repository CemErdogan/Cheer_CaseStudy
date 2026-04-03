using Abstractions.FigureSystem;
using Game.FigureSystem.Runtime;
using UnityEditor;
using UnityEngine;

namespace Game.FigureSystem.Editor
{
    [CustomPropertyDrawer(typeof(FigureData))]
    public class FigureDrawer : PropertyDrawer
    {
        private const int CellSize   = 14;
        private const int CellPad    = 2;
        private const float BtnHeight = 22f;
        private const float Pad       = 4f;

        // 2 rows of slots
        private const int Rows = 2;
        private const int Cols = 2;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float grid = Rows * (CellSize + CellPad);
            return EditorGUIUtility.singleLineHeight   // label
                 + Pad + EditorGUIUtility.singleLineHeight  // GridCoord + IsSquare
                 + Pad + grid
                 + Pad + BtnHeight
                 + Pad;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            float x = position.x;
            float y = position.y;
            float w = position.width;
            float lh = EditorGUIUtility.singleLineHeight;

            // Header label
            EditorGUI.LabelField(new Rect(x, y, w, lh), label, EditorStyles.boldLabel);
            y += lh + Pad;

            // GridCoord + IsSquare on one line
            var gridCoordProp = property.FindPropertyRelative("<GridCoord>k__BackingField");
            var isSquareProp  = property.FindPropertyRelative("<IsSquare>k__BackingField");

            float half = w * 0.6f;
            EditorGUI.PropertyField(new Rect(x, y, half, lh), gridCoordProp, new GUIContent("GridCoord"));
            isSquareProp.boolValue = EditorGUI.ToggleLeft(
                new Rect(x + half + 4, y, w - half - 4, lh),
                "IsSquare", isSquareProp.boolValue);
            y += lh + Pad;

            // 2×2 slot preview
            var pointsProp = property.FindPropertyRelative("<Points>k__BackingField");
            DrawSlotPreview(x + 4, y, pointsProp);
            y += Rows * (CellSize + CellPad) + Pad;

            // Edit button
            if (GUI.Button(new Rect(x, y, w, BtnHeight), "Edit Figure"))
                FigureWindow.Open(property.Copy());

            EditorGUI.EndProperty();
        }

        private static void DrawSlotPreview(float x, float y, SerializedProperty pointsProp)
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    var rect = new Rect(
                        x + col * (CellSize + CellPad),
                        y + row * (CellSize + CellPad),
                        CellSize, CellSize);

                    var slot  = (SlotPosition)(row * Cols + col);
                    var color = new Color(0.15f, 0.15f, 0.15f);

                    var idx = FindSlotIndex(pointsProp, slot);
                    if (idx >= 0)
                    {
                        var colorVal = pointsProp.GetArrayElementAtIndex(idx)
                            .FindPropertyRelative("<Color>k__BackingField").intValue;
                        color = FigureColorUtil.GetColor((ColorType)colorVal);
                    }

                    EditorGUI.DrawRect(rect, color);
                    DrawBorder(rect);
                }
            }
        }

        internal static int FindSlotIndex(SerializedProperty pointsProp, SlotPosition slot)
        {
            for (int i = 0; i < pointsProp.arraySize; i++)
            {
                var pos = pointsProp.GetArrayElementAtIndex(i)
                    .FindPropertyRelative("<Position>k__BackingField");
                if (pos.intValue == (int)slot) return i;
            }
            return -1;
        }

        private static void DrawBorder(Rect r)
        {
            var c = new Color(0.08f, 0.08f, 0.08f, 0.9f);
            EditorGUI.DrawRect(new Rect(r.x,        r.y,        r.width, 1),       c);
            EditorGUI.DrawRect(new Rect(r.x,        r.yMax - 1, r.width, 1),       c);
            EditorGUI.DrawRect(new Rect(r.x,        r.y,        1,       r.height), c);
            EditorGUI.DrawRect(new Rect(r.xMax - 1, r.y,        1,       r.height), c);
        }
    }
}
