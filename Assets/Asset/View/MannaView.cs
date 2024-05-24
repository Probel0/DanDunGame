using System;
using UnityEngine.Events;

public class MannaView : BaseViewSingle<IVMManna>
{
    public UnityEvent<float> onMannaCurrentChanged;
    public UnityEvent<float> onMannaMaxChanged;

    public MannaView(IVMManna viewModel) : base(viewModel) { }

    protected override void Initialize()
    {
        viewModel.manna.Value.CurrentValue.Subscribe(onMannaCurrentChanged.Invoke);
        viewModel.manna.Value.MaxValue.Subscribe(onMannaMaxChanged.Invoke);
    }
}