using System;
using System.Reactive.Linq;
using Lukomor.MVVM;
using Lukomor.MVVM.Binders;
using Lukomor.Reactive;
using UnityEngine;

public class EntityViewModel : BaseViewModel<EntityModel>, IVMEntity
{
    public IReactiveProperty<IStats> health => model.Health;
    public IReactiveProperty<IStats> manna => model.Manna;
    public IReactiveProperty<IStats> defense => model.Defense;
    public IReactiveProperty<IStats> speed => model.Speed;

    void Start()
    {
        Initialize();
    }

    protected override void InitializeViewModel()
    {
        base.InitializeViewModel();
    }

    [ContextMenu("Initialization")]
    public void TakeDamage()
    {
        model.Health.Value.ChangeCurrent(-5);
    }

    [ContextMenu("Destroy")]
    public void Destroyy()
    {
        Destroy(gameObject);
    }
}
