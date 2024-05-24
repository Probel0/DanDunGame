using System;
using System.Reflection;
using Lukomor.Reactive;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseBindToUnityEvent<TViewModel>
where TViewModel : I2ViewModel
{
    [SerializeField, HideInInspector] protected string selectedVariable;

    public abstract void InitializeSubscribe(TViewModel viewModel);

    protected abstract void SubscribeToVariable(TViewModel viewModel);
}