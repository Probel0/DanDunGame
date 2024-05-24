using System;
using System.Reflection;
using Lukomor.Reactive;
using UnityEngine;
using UnityEngine.Events;

public class VariableBindToUnityEvent<TViewModel, TVariable> : BaseBindToUnityEvent<TViewModel>
where TViewModel : I2ViewModel
{
    public UnityEvent<TVariable> _event;
    
    public override void InitializeSubscribe(TViewModel viewModel)
    {
        SubscribeToVariable(viewModel);
    }
    protected override void SubscribeToVariable(TViewModel viewModel)
    {
        PropertyInfo propertyInfo = typeof(TViewModel).GetProperty(selectedVariable);
        if (propertyInfo != null)
        {
            IReactiveProperty<TVariable> propertyValue = propertyInfo.GetValue(viewModel) as IReactiveProperty<TVariable>;
            propertyValue.Subscribe(_event.Invoke);
        }
        else
        {
            Debug.LogError($"Variable {selectedVariable} not found in ViewModel.");
        }
    }
}