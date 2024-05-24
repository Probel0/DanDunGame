using System;
using UnityEngine.Events;

public class HealthViewTest : BaseViewSingleWithStats<IVMHealth, IStats>
{
    public HealthViewTest(IVMHealth viewModel) : base(viewModel) { }
    
    protected override void InitializeSubscribe()
    {
        throw new NotImplementedException();
    }
}