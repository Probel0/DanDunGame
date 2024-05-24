using System;
using UnityEngine.Events;

public class HealthView : BaseViewSingle<IVMHealth>
{
    public UnityEvent<float> onHealthCurrentChanged;
    public UnityEvent<float> onHealthMaxChanged;

    public HealthView(IVMHealth viewModel) : base(viewModel) { }

    protected override void Initialize()
    {
        viewModel.health.Value.CurrentValue.Subscribe(onHealthCurrentChanged.Invoke);
        viewModel.health.Value.MaxValue.Subscribe(onHealthMaxChanged.Invoke);
    }
}