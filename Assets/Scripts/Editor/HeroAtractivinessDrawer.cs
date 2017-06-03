using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

[CustomPropertyDrawer(typeof(HeroFloatDictionary))]
public class HeroAtractivinessDrawer : DictionaryDrawer<HeroClass, float>
{
    private SerializableDictionary<HeroClass, float> _Dictionary;
    private bool _Foldout;
    private const float kButtonWidth = 18f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        //CheckInitialize(property, label);
        if (_Dictionary == null)
        {
            var target = property.serializedObject.targetObject;
            _Dictionary = fieldInfo.GetValue(target) as SerializableDictionary<HeroClass, float>;
            if (_Dictionary == null)
            {
                _Dictionary = new SerializableDictionary<HeroClass, float>();
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


        for (HeroClass hero = 0; hero != HeroClass.endEnum; hero++)
        {
            if (!_Dictionary.ContainsKey(hero))
                _Dictionary.Add(hero, 0.0f);
            HeroClass key = hero;
            float value = _Dictionary[key];

            position.y += 17f;

            var keyRect = position;
            keyRect.width /= 2;
            keyRect.width -= 4;
            EditorGUI.LabelField(keyRect, new GUIContent(HeroClass.GetName(typeof(HeroClass), key)));

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