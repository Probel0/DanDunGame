using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lukomor.MVVM.Binders;
using Lukomor.Reactive;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[CustomEditor(typeof(BaseViewSingleWithStats<,>), true)]
[CanEditMultipleObjects]
public class StatsViewEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        var baseViewType = target.GetType();
        var genericArguments = baseViewType.BaseType.GetGenericArguments();

        var firstArgument = genericArguments[0];
        var secondArgument = genericArguments[1];

        var viewModelObject = serializedObject.FindProperty("GMViewModel").objectReferenceValue as GameObject;

        var viewModelComponent = viewModelObject.GetComponent(firstArgument);
        var viewModelProperties = viewModelComponent.GetType().GetProperties().Where(p => typeof(IReactiveProperty<>).MakeGenericType(secondArgument).IsAssignableFrom(p.PropertyType)).Select(p => p.Name).ToArray();
        var viewModelPropertyIndex = Array.IndexOf(viewModelProperties, serializedObject.FindProperty("selectedVariable").stringValue);
        viewModelPropertyIndex = EditorGUILayout.Popup("ViewModel Property", viewModelPropertyIndex, viewModelProperties);
        if (viewModelPropertyIndex >= 0)
        {
            serializedObject.FindProperty("selectedVariable").stringValue = viewModelProperties[viewModelPropertyIndex];
        }

        serializedObject.ApplyModifiedProperties();
    }
}
