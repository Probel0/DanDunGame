using Lukomor.Reactive;
using UnityEngine;

public interface IVMDefense : I2ViewModel
{
    public IReactiveProperty<IStats> defense { get; }
}