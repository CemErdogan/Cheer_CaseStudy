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

        private static readonly SlotPosition[] Slots =
        {
            SlotPosition.TopLeft, SlotPosition.TopRight,
            SlotPosition.BottomLeft, SlotPosition.BottomRight
        };

        private static readonly string[] SlotLabels = { "TL", "TR", "BL", "BR" };

        private const int CellSize = 48;
        private const int CellPad  = 6;

        public static void Open(SerializedProperty property)
        {
            var win = GetWindow<FigureWindow>("Figure Editor");
            win._propertyPath = property.propertyPath;
            win._so = property.serializedObject;
            win.minSize = new Vector2(340, 420);
            win.Show();
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
                EditorGUILayout.LabelField("Property is no longer valid. Please reopen.");
                return;
            }

            var gridCoordProp = figureProp.FindPropertyRelative("<GridCoord>k__BackingField");
            var isSquareProp  = figureProp.FindPropertyRelative("<IsSquare>k__BackingField");
            var pointsProp    = figureProp.FindPropertyRelative("<Points>k__BackingField");

            EditorGUILayout.Space(6);
            EditorGUILayout.PropertyField(gridCoordProp, new GUIContent("Grid Coord"));
            isSquareProp.boolValue = EditorGUILayout.Toggle("Is Square", isSquareProp.boolValue);
            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("Slots  (left-click: toggle | right-click: color)", EditorStyles.miniLabel);
            EditorGUILayout.Space(4);

            var e = Event.current;

            for (int row = 0; row < 2; row++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(8);

                for (int col = 0; col < 2; col++)
                {
                    var slot = Slots[row * 2 + col];
                    var idx  = FigureDrawer.FindSlotIndex(pointsProp, slot);
                    bool active = idx >= 0;

                    var bgColor = active
                        ? FigureColorUtil.GetColor((ColorType)pointsProp
                            .GetArrayElementAtIndex(idx)
                            .FindPropertyRelative("<Color>k__BackingField").intValue)
                        : new Color(0.18f, 0.18f, 0.18f);

                    var savedBg = GUI.backgroundColor;
                    GUI.backgroundColor = bgColor;
                    var cellRect = GUILayoutUtility.GetRect(CellSize, CellSize,
                        GUILayout.Width(CellSize), GUILayout.Height(CellSize));
                    GUI.backgroundColor = savedBg;

                    EditorGUI.DrawRect(cellRect, bgColor);

                    GUI.Label(cellRect, SlotLabels[row * 2 + col],
                        new GUIStyle(EditorStyles.boldLabel)
                        { alignment = TextAnchor.UpperLeft, fontSize = 9,
                          normal = { textColor = Color.white } });

                    if (active && pointsProp.GetArrayElementAtIndex(idx)
                            .FindPropertyRelative("<IsConnected>k__BackingField").boolValue)
                    {
                        GUI.Label(cellRect, "⚡",
                            new GUIStyle(EditorStyles.label)
                            { alignment = TextAnchor.LowerRight, fontSize = 11,
                              normal = { textColor = Color.yellow } });
                    }

                    DrawBorder(cellRect);
                    GUILayout.Space(CellPad);

                    if (e.type == EventType.MouseDown && cellRect.Contains(e.mousePosition))
                    {
                        if (e.button == 0) { ToggleSlot(pointsProp, slot); e.Use(); }
                        else if (e.button == 1 && active) { ShowColorMenu(pointsProp, slot); e.Use(); }
                    }
                }

                EditorGUILayout.EndHorizontal();
                GUILayout.Space(CellPad);
            }

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Connections", EditorStyles.boldLabel);
            EditorGUILayout.Space(2);

            foreach (var slot in Slots)
            {
                var idx = FigureDrawer.FindSlotIndex(pointsProp, slot);
                if (idx < 0) continue;

                var pointProp       = pointsProp.GetArrayElementAtIndex(idx);
                var isConnectedProp = pointProp.FindPropertyRelative("<IsConnected>k__BackingField");
                var connWithProp    = pointProp.FindPropertyRelative("<ConnectedWith>k__BackingField");
                var isBigSqProp     = pointProp.FindPropertyRelative("<IsBigSquare>k__BackingField");

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(slot.ToString(), GUILayout.Width(90));
                isConnectedProp.boolValue = EditorGUILayout.Toggle(isConnectedProp.boolValue, GUILayout.Width(20));

                if (isConnectedProp.boolValue)
                {
                    connWithProp.intValue = (int)(SlotPosition)EditorGUILayout.EnumPopup(
                        (SlotPosition)connWithProp.intValue, GUILayout.Width(100));
                }
                else
                {
                    EditorGUILayout.LabelField("", GUILayout.Width(100));
                }

                isBigSqProp.boolValue = EditorGUILayout.ToggleLeft("BigSq", isBigSqProp.boolValue, GUILayout.Width(70));
                EditorGUILayout.EndHorizontal();
            }

            _so.ApplyModifiedProperties();
        }

        private void ToggleSlot(SerializedProperty pointsProp, SlotPosition slot)
        {
            _so.Update();
            var idx = FigureDrawer.FindSlotIndex(pointsProp, slot);

            if (idx >= 0)
            {
                for (int i = idx; i < pointsProp.arraySize - 1; i++)
                {
                    pointsProp.MoveArrayElement(i + 1, i);
                }
                pointsProp.arraySize--;
            }
            else
            {
                pointsProp.arraySize++;
                var newElem = pointsProp.GetArrayElementAtIndex(pointsProp.arraySize - 1);
                newElem.FindPropertyRelative("<Position>k__BackingField").intValue      = (int)slot;
                newElem.FindPropertyRelative("<Color>k__BackingField").intValue         = 0;
                newElem.FindPropertyRelative("<IsConnected>k__BackingField").boolValue  = false;
                newElem.FindPropertyRelative("<ConnectedWith>k__BackingField").intValue = 0;
                newElem.FindPropertyRelative("<IsBigSquare>k__BackingField").boolValue  = false;
            }

            _so.ApplyModifiedProperties();
            Repaint();
        }

        private void ShowColorMenu(SerializedProperty pointsProp, SlotPosition slot)
        {
            var menu = new GenericMenu();
            foreach (ColorType ct in System.Enum.GetValues(typeof(ColorType)))
            {
                var captured = ct;
                menu.AddItem(new GUIContent(ct.ToString()), false,
                    () => SetColor(pointsProp, slot, (int)captured));
            }
            menu.ShowAsContext();
        }

        private void SetColor(SerializedProperty pointsProp, SlotPosition slot, int colorInt)
        {
            _so.Update();
            var idx = FigureDrawer.FindSlotIndex(pointsProp, slot);
            if (idx < 0) return;
            pointsProp.GetArrayElementAtIndex(idx)
                .FindPropertyRelative("<Color>k__BackingField").intValue = colorInt;
            _so.ApplyModifiedProperties();
            Repaint();
        }

        private static void DrawBorder(Rect r)
        {
            var c = new Color(0.05f, 0.05f, 0.05f, 0.9f);
            EditorGUI.DrawRect(new Rect(r.x,        r.y,        r.width, 1),        c);
            EditorGUI.DrawRect(new Rect(r.x,        r.yMax - 1, r.width, 1),        c);
            EditorGUI.DrawRect(new Rect(r.x,        r.y,        1,       r.height), c);
            EditorGUI.DrawRect(new Rect(r.xMax - 1, r.y,        1,       r.height), c);
        }
    }
}
