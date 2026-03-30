using System.Collections.Generic;
using Abstractions.FigureSystem;
using Game.FigureSystem.Runtime;
using UnityEditor;
using UnityEngine;

namespace Game.FigureSystem.Editor
{
    public class FigureWindow : EditorWindow
    {
        private string _propertyPath;
        private SerializedObject _so;
        private int _width;
        private int _height;
        private Vector2 _scrollPos;

        private const int CellSize = 40;
        private const int CellPad = 3;

        public static void Open(SerializedProperty property)
        {
            var window = GetWindow<FigureWindow>("Figure Editor");
            window._propertyPath = property.propertyPath;
            window._so = property.serializedObject;

            var sizeProp = property.FindPropertyRelative("<Size>k__BackingField");
            window._width = Mathf.Max(1, Mathf.RoundToInt(sizeProp.vector2Value.x));
            window._height = Mathf.Max(1, Mathf.RoundToInt(sizeProp.vector2Value.y));

            window.minSize = new Vector2(300, 200);
            window.Show();
        }

        private void OnGUI()
        {
            if (_so == null || string.IsNullOrEmpty(_propertyPath))
            {
                EditorGUILayout.LabelField("No figure selected.");
                return;
            }

            _so.Update();
            var figureProp = _so.FindProperty(_propertyPath);

            if (figureProp == null)
            {
                EditorGUILayout.LabelField("Property is no longer valid. Please reopen the editor.");
                return;
            }

            DrawSizeControls(figureProp);
            EditorGUILayout.Space(8);
            DrawGrid(figureProp);

            _so.ApplyModifiedProperties();
        }

        private void DrawSizeControls(SerializedProperty figureProp)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Width", GUILayout.Width(45));
            var newWidth = EditorGUILayout.IntField(_width, GUILayout.Width(45));
            GUILayout.Space(16);
            EditorGUILayout.LabelField("Height", GUILayout.Width(45));
            var newHeight = EditorGUILayout.IntField(_height, GUILayout.Width(45));
            EditorGUILayout.EndHorizontal();

            newWidth = Mathf.Clamp(newWidth, 1, 20);
            newHeight = Mathf.Clamp(newHeight, 1, 20);

            if (newWidth == _width && newHeight == _height) return;
            
            _width = newWidth;
            _height = newHeight;
            RegeneratePoints(figureProp, _width, _height);
        }

        private void DrawGrid(SerializedProperty figureProp)
        {
            var pointsProp = figureProp.FindPropertyRelative("<Points>k__BackingField");
            var e = Event.current;

            float totalW = _width * (CellSize + CellPad);
            float totalH = _height * (CellSize + CellPad);
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Width(totalW + 20), GUILayout.Height(totalH + 20));

            for (int row = 0; row < _height; row++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int col = 0; col < _width; col++)
                {
                    var pointIdx = FindPointIndex(pointsProp, col, row);
                    var colorInt = 0;

                    if (pointIdx >= 0)
                    {
                        var point = pointsProp.GetArrayElementAtIndex(pointIdx);
                        colorInt = point.FindPropertyRelative("<ColorType>k__BackingField").intValue;
                    }

                    var cellColor = FigureColorUtil.GetColor((ColorType)colorInt);
                    var cellRect = GUILayoutUtility.GetRect(CellSize, CellSize, GUILayout.Width(CellSize), GUILayout.Height(CellSize));

                    EditorGUI.DrawRect(cellRect, cellColor);
                    DrawBorder(cellRect);

                    if (e.type == EventType.MouseDown && cellRect.Contains(e.mousePosition))
                    {
                        if (e.button == 0)
                        {
                            ToggleActive(pointsProp, col, row);
                            e.Use();
                        }
                        else if (e.button == 1)
                        {
                            ShowColorMenu(pointsProp, col, row);
                            e.Use();
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }

        private void ShowColorMenu(SerializedProperty pointsProp, int col, int row)
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("None"),   false, () => SetColor(pointsProp, col, row, 0));
            menu.AddItem(new GUIContent("Red"),    false, () => SetColor(pointsProp, col, row, 1));
            menu.AddItem(new GUIContent("Green"),  false, () => SetColor(pointsProp, col, row, 2));
            menu.AddItem(new GUIContent("Blue"),   false, () => SetColor(pointsProp, col, row, 3));
            menu.AddItem(new GUIContent("Yellow"), false, () => SetColor(pointsProp, col, row, 4));
            menu.ShowAsContext();
        }

        private void ToggleActive(SerializedProperty pointsProp, int col, int row)
        {
            _so.Update();
            var idx = FindPointIndex(pointsProp, col, row);
            if (idx < 0) return;
            var activeProp = pointsProp.GetArrayElementAtIndex(idx).FindPropertyRelative("<IsActive>k__BackingField");
            activeProp.boolValue = !activeProp.boolValue;
            _so.ApplyModifiedProperties();
            Repaint();
        }

        private void SetColor(SerializedProperty pointsProp, int col, int row, int colorIntValue)
        {
            _so.Update();
            var idx = FindPointIndex(pointsProp, col, row);
            if (idx < 0) return;
            var colorProp = pointsProp.GetArrayElementAtIndex(idx).FindPropertyRelative("<ColorType>k__BackingField");
            colorProp.intValue = colorIntValue;
            _so.ApplyModifiedProperties();
            Repaint();
        }

        private void RegeneratePoints(SerializedProperty figureProp, int newWidth, int newHeight)
        {
            _so.Update();

            var sizeProp = figureProp.FindPropertyRelative("<Size>k__BackingField");
            var pointsProp = figureProp.FindPropertyRelative("<Points>k__BackingField");

            var existing = new Dictionary<(int, int), (bool isActive, int color)>();
            for (int i = 0; i < pointsProp.arraySize; i++)
            {
                var elem = pointsProp.GetArrayElementAtIndex(i);
                var coord = elem.FindPropertyRelative("<Coord>k__BackingField");
                var isActive = elem.FindPropertyRelative("<IsActive>k__BackingField").boolValue;
                var colorInt = elem.FindPropertyRelative("<ColorType>k__BackingField").intValue;
                existing[(coord.vector2IntValue.x, coord.vector2IntValue.y)] = (isActive, colorInt);
            }

            pointsProp.arraySize = newWidth * newHeight;

            int index = 0;
            for (int row = 0; row < newHeight; row++)
            {
                for (int col = 0; col < newWidth; col++)
                {
                    var elem = pointsProp.GetArrayElementAtIndex(index);
                    var coordProp = elem.FindPropertyRelative("<Coord>k__BackingField");
                    var activeProp = elem.FindPropertyRelative("<IsActive>k__BackingField");
                    var colorProp = elem.FindPropertyRelative("<ColorType>k__BackingField");

                    coordProp.vector2IntValue = new Vector2Int(col, row);

                    if (existing.TryGetValue((col, row), out var saved))
                    {
                        activeProp.boolValue = saved.isActive;
                        colorProp.intValue = saved.color;
                    }
                    else
                    {
                        activeProp.boolValue = false;
                        colorProp.intValue = 0;
                    }

                    index++;
                }
            }

            sizeProp.vector2Value = new Vector2(newWidth, newHeight);
            _so.ApplyModifiedProperties();
            Repaint();
        }

        private static int FindPointIndex(SerializedProperty pointsProp, int col, int row)
        {
            for (int i = 0; i < pointsProp.arraySize; i++)
            {
                var coord = pointsProp.GetArrayElementAtIndex(i).FindPropertyRelative("<Coord>k__BackingField");
                if (coord.vector2IntValue.x == col && coord.vector2IntValue.y == row)
                    return i;
            }
            return -1;
        }

        private static void DrawBorder(Rect rect)
        {
            var c = new Color(0.08f, 0.08f, 0.08f, 0.9f);
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, 1), c);
            EditorGUI.DrawRect(new Rect(rect.x, rect.yMax - 1, rect.width, 1), c);
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, 1, rect.height), c);
            EditorGUI.DrawRect(new Rect(rect.xMax - 1, rect.y, 1, rect.height), c);
        }
    }
}
