using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class TextEditor : EditorWindow
{
    string text;
    string path;

    [MenuItem("Window/Text Editor")]
    static void Init()
    {
        GetWindow<TextEditor>(false, "Text Editor");
    }

    void OnGUI()
    {
        var toolbar_rect = DrawToolbar();
        float y_offset = toolbar_rect.height + toolbar_rect.y;
        var text_rect = new Rect(toolbar_rect.x, y_offset, position.width, position.height - y_offset - 4);

        var style = EditorStyles.textArea;
        style.richText = true;
        text = EditorGUI.TextArea(text_rect, text, style);
    }

    Rect DrawToolbar()
    {
        var rect = EditorGUILayout.BeginHorizontal();
        EditorGUI.DrawRect(rect, Color.white * 0.5f);
        Button(new GUIContent("New"), NewFile, GUILayout.Width(48));
        Button(new GUIContent("Open"), OpenFile, GUILayout.Width(48));
        Button(new GUIContent("Save"), SaveFile, GUILayout.Width(48));

        EditorGUILayout.LabelField(Path.GetFileName(path), EditorStyles.miniLabel, GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();
        return rect;
    }

    void NewFile()
    {
        text = "";
        path = "";
        DefocusAndRepaint();
    }

    void OpenFile()
    {
        path = EditorUtility.OpenFilePanel("Open text file", "", "*");
        text = File.ReadAllText(path);
        DefocusAndRepaint();
    }

    void SaveFile()
    {
        if (string.IsNullOrEmpty(path))
        {
            path = EditorUtility.SaveFilePanel("Save text file", "", "", "*");
        }
        File.WriteAllText(path, text);
        DefocusAndRepaint();
    }

    void DefocusAndRepaint()
    {
        GUI.FocusControl(null);
        Repaint();
    }
    // Function used to draw buttons in one line, Copy it and use it  elsewhere if you want ;)
    void Button(GUIContent content, Action action, params GUILayoutOption[] options)
    {
        if (GUILayout.Button(content, EditorStyles.miniButtonLeft, options))
        {
            action();
        }
    }
}