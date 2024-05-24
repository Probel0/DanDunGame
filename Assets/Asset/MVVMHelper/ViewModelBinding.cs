using System;
using System.Linq;
using Lukomor.Reactive;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class ViewModelBinding : MonoBehaviour
{
    [Serializable]
    public class PropertyMethodBinding
    {
        public GameObject viewModelObject;
        public string viewModelComponent;
        public string viewModelProperty;

        public GameObject viewObject;
        public string viewComponent;
        public string viewMethod;
    }

    public PropertyMethodBinding[] bindings;

    void Start()
    {
        foreach (var binding in bindings)
        {
            if (binding.viewModelObject == null || binding.viewObject == null ||
                string.IsNullOrEmpty(binding.viewModelComponent) || string.IsNullOrEmpty(binding.viewModelProperty) ||
                string.IsNullOrEmpty(binding.viewComponent) || string.IsNullOrEmpty(binding.viewMethod))
            {
                Debug.LogWarning("Incomplete binding");
                continue;
            }

            var viewModelComponent = binding.viewModelObject.GetComponent(binding.viewModelComponent);
            var viewComponent = binding.viewObject.GetComponent(binding.viewComponent);

            if (viewModelComponent == null)
            {
                Debug.LogError($"Component '{binding.viewModelComponent}' not found in {binding.viewModelObject.name}");
                continue;
            }

            if (viewComponent == null)
            {
                Debug.LogError($"Component '{binding.viewComponent}' not found in {binding.viewObject.name}");
                continue;
            }

            // Get ViewModel property
            var viewModelProperty = viewModelComponent.GetType().GetProperty(binding.viewModelProperty);
            if (viewModelProperty == null)
            {
                Debug.LogError($"Property '{binding.viewModelProperty}' not found in {viewModelComponent.GetType()}");
                continue;
            }

            // Get View method
            var viewMethod = viewComponent.GetType().GetMethod(binding.viewMethod);
            if (viewMethod == null)
            {
                Debug.LogError($"Method '{binding.viewMethod}' not found in {viewComponent.GetType()}");
                continue;
            }

            // Create UnityEvent and add listener
            var unityEvent = new UnityEvent<object>();
            unityEvent.AddListener((param) =>
            {
                viewMethod.Invoke(viewComponent, new[] { param });
            });

            // Subscribe to ReactiveProperty changes
            var reactiveProperty = viewModelProperty.GetValue(viewModelComponent) as IReactiveProperty<float>;
            if (reactiveProperty != null)
            {
                reactiveProperty.Subscribe(value =>
                {
                    unityEvent.Invoke(value);
                });
            }
            else
            {
                Debug.LogError($"Property '{binding.viewModelProperty}' is not a ReactiveProperty");
            }
        }
    }
}

[CustomEditor(typeof(ViewModelBinding))]
[CanEditMultipleObjects]
public class ViewModelBindingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var bindings = serializedObject.FindProperty("bindings");

        for (int i = 0; i < bindings.arraySize; i++)
        {
            var binding = bindings.GetArrayElementAtIndex(i);

            EditorGUILayout.PropertyField(binding.FindPropertyRelative("viewModelObject"));
            var viewModelObject = binding.FindPropertyRelative("viewModelObject").objectReferenceValue as GameObject;

            if (viewModelObject != null)
            {
                var viewModelComponents = viewModelObject.GetComponents<Component>();
                var viewModelComponentNames = viewModelComponents.Select(c => c.GetType().Name).ToArray();
                var viewModelComponentIndex = Array.IndexOf(viewModelComponentNames, binding.FindPropertyRelative("viewModelComponent").stringValue);
                viewModelComponentIndex = EditorGUILayout.Popup("ViewModel Component", viewModelComponentIndex, viewModelComponentNames);
                if (viewModelComponentIndex >= 0)
                {
                    binding.FindPropertyRelative("viewModelComponent").stringValue = viewModelComponentNames[viewModelComponentIndex];
                    var viewModelComponent = viewModelComponents[viewModelComponentIndex];
                    var viewModelProperties = viewModelComponent.GetType().GetProperties().Where(p => typeof(IReactiveProperty<float>).IsAssignableFrom(p.PropertyType)).Select(p => p.Name).ToArray();
                    var viewModelPropertyIndex = Array.IndexOf(viewModelProperties, binding.FindPropertyRelative("viewModelProperty").stringValue);
                    viewModelPropertyIndex = EditorGUILayout.Popup("ViewModel Property", viewModelPropertyIndex, viewModelProperties);
                    if (viewModelPropertyIndex >= 0)
                    {
                        binding.FindPropertyRelative("viewModelProperty").stringValue = viewModelProperties[viewModelPropertyIndex];
                    }
                }
            }

            EditorGUILayout.PropertyField(binding.FindPropertyRelative("viewObject"));
            var viewObject = binding.FindPropertyRelative("viewObject").objectReferenceValue as GameObject;

            if (viewObject != null)
            {
                var viewComponents = viewObject.GetComponents<Component>();
                var viewComponentNames = viewComponents.Select(c => c.GetType().Name).ToArray();
                var viewComponentIndex = Array.IndexOf(viewComponentNames, binding.FindPropertyRelative("viewComponent").stringValue);
                viewComponentIndex = EditorGUILayout.Popup("View Component", viewComponentIndex, viewComponentNames);
                if (viewComponentIndex >= 0)
                {
                    binding.FindPropertyRelative("viewComponent").stringValue = viewComponentNames[viewComponentIndex];
                    var viewComponent = viewComponents[viewComponentIndex];
                    var viewMethods = viewComponent.GetType().GetMethods().Where(m => m.GetParameters().Length == 1).Select(m => m.Name).ToArray();
                    var viewMethodIndex = Array.IndexOf(viewMethods, binding.FindPropertyRelative("viewMethod").stringValue);
                    viewMethodIndex = EditorGUILayout.Popup("View Method", viewMethodIndex, viewMethods);
                    if (viewMethodIndex >= 0)
                    {
                        binding.FindPropertyRelative("viewMethod").stringValue = viewMethods[viewMethodIndex];
                    }
                }
            }
            EditorGUILayout.Space();
        }

        if (GUILayout.Button("Add Binding"))
        {
            bindings.InsertArrayElementAtIndex(bindings.arraySize);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
