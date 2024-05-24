using System;
using System.Reflection;
using Lukomor.MVVM.Binders;
using Lukomor.Reactive;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseViewSingleWithStats<TViewModel, TVariable> : BaseViewSingle<TViewModel>
where TViewModel : I2ViewModel
{
    public UnityEvent<TVariable> onValueCurrentChanged;
    public UnityEvent<TVariable> onValueMaxChanged;

    [SerializeField, HideInInspector] private string selectedVariable;

    protected BaseViewSingleWithStats(TViewModel viewModel) : base(viewModel)
    {
    }

    protected override void Initialize()
    {
        InitializeSubscribe();
        SubscribeToVariable(selectedVariable, onValueCurrentChanged.Invoke);
        SubscribeToVariable(selectedVariable, onValueMaxChanged.Invoke);
    }

    protected abstract void InitializeSubscribe();

    protected void SubscribeToVariable(string variableName, UnityAction<TVariable> action)
    {
        PropertyInfo propertyInfo = typeof(TViewModel).GetProperty(variableName);
        if (propertyInfo != null)
        {
            IReactiveProperty<TVariable> propertyValue = propertyInfo.GetValue(viewModel) as IReactiveProperty<TVariable>;
            propertyValue.Subscribe(action.Invoke);
        }
        else
        {
            Debug.LogError($"Variable {variableName} not found in ViewModel.");
        }
    }
}