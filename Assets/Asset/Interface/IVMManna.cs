using Lukomor.Reactive;
using UnityEngine;

public interface IVMManna : I2ViewModel
{
    public IReactiveProperty<IStats> manna { get; }
}