using Lukomor.Reactive;
using UnityEngine;

public interface IVMEntity : I2ViewModel, IVMDefense, IVMHealth, IVMManna
{
    public IReactiveProperty<IStats> speed { get; }
}