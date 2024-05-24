using System;
using System.Reflection;
using Lukomor.Reactive;
using UnityEngine;
using UnityEngine.Events;

public class StatBindToUnityEvent<TViewModel> : BaseBindToUnityEvent<TViewModel>
where TViewModel : I2ViewModel
{
    public UnityEvent<float> onValueCurrentChanged;
    public UnityEvent<float> onValueMaxChanged;

    public override void InitializeSubscribe(TViewModel viewModel)
    {
        SubscribeToVariable(viewModel);
    }

    protected override void SubscribeToVariable(TViewModel viewModel)
    {
        PropertyInfo propertyInfo = typeof(TViewModel).GetProperty(selectedVariable);
        if (propertyInfo != null)
        {
            IReactiveProperty<IStats> propertyValue = propertyInfo.GetValue(viewModel) as IReactiveProperty<IStats>;
            propertyValue.Value.CurrentValue.Subscribe(onValueCurrentChanged.Invoke);
            propertyValue.Value.MaxValue.Subscribe(onValueMaxChanged.Invoke);
        }
        else
        {
            Debug.LogError($"Variable {selectedVariable} not found in ViewModel.");
        }
    }
}