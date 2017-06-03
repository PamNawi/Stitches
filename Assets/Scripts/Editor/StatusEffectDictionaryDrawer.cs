using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEditor;

[CustomPropertyDrawer(typeof(StatusEffectDictionary))]
public class StatusEffectDictionaryDrawer : DictionaryDrawer<StatusEnum, float>// {
{
    private SerializableDictionary<StatusEnum, float> _Dictionary;
    private bool _Foldout;
    private const float kButtonWidth = 18f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        //CheckInitialize(property, label);
        if (_Dictionary == null)
        {
            var target = property.serializedObject.targetObject;
            _Dictionary = fieldInfo.GetValue(target) as SerializableDictionary<StatusEnum, float>;
            if (_Dictionary == null)
            {
                _Dictionary = new SerializableDictionary<StatusEnum, float>();
                fieldInfo.SetValue(target, _Dictionary);
            }
            _Foldout = EditorPrefs.GetBool(label.text);
        }

        position.height = 17f;
        var foldoutRect = position;
        foldoutRect.width -= 2 * kButtonWidth;
        EditorGUI.BeginChangeCheck();
        _Foldout = EditorGUI.Foldout(foldoutRect, _Foldout, label, true);
        if (EditorGUI.EndChangeCheck())
            EditorPrefs.SetBool(label.text, _Foldout);

        if (!_Foldout)
            return;


        for (StatusEnum status = 0; status != StatusEnum.endEnum; status++)
        {
            if (!_Dictionary.ContainsKey(status))
                _Dictionary.Add(status, 0.0f);
            StatusEnum key = status;
            float value = _Dictionary[key];

            position.y += 17f;

            var keyRect = position;
            keyRect.width /= 2;
            keyRect.width -= 4;
            EditorGUI.LabelField(keyRect, new GUIContent(HeroClass.GetName(typeof(StatusEnum), key)));

            var valueRect = position;
            valueRect.x = position.width / 2 + 15;
            valueRect.width = keyRect.width - kButtonWidth;
            EditorGUI.BeginChangeCheck();
            value = DoField(valueRect, typeof(float), value);
            if (EditorGUI.EndChangeCheck())
            {
                _Dictionary[key] = value;
                break;
            }
        }

        EditorGUI.EndProperty();
    }
}