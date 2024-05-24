using System;
using Lukomor.MVVM;
using UnityEngine;

public interface I2ViewModel : IViewModel
{
    public event Action OnViewInit;
    public event Action OnDestroyed;
}