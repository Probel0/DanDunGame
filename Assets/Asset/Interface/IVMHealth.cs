using Lukomor.Reactive;
using UnityEngine;

public interface IVMHealth : I2ViewModel
{
    public IReactiveProperty<IStats> health { get; }
}