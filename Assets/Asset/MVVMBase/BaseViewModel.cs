using System;
using Lukomor.MVVM;
using UnityEngine;
using UnityEngine.PlayerLoop;

public abstract class BaseViewModel<TModel> : MonoBehaviour, IViewModel
where TModel : BaseModel
{
    [SerializeField] protected TModel model;
    public event Action OnViewInit;
    public event Action OnDestroyed;

    public BaseViewModel(TModel model = null, bool instanceInit = false)
    {
        this.model = model ?? Activator.CreateInstance<TModel>();
        if (instanceInit)
            Initialize();
    }

    private void InitializeModel() => model.Initialize();

    public void Initialize()
    {
        InitializeModel();
        InitializeViewModel();
        InitializeView();
    }

    protected virtual void InitializeViewModel() { }

    private void InitializeView() => OnViewInit?.Invoke();

    public void SpawnPrefab(GameObject view, Vector3 position, Quaternion rotation, Transform parent)
    {
        Instantiate(view, position, rotation, parent);
    }

    public void OnDestroy()
    {
        // Model.UnsubscribeFromModelChanges(UpdateData);
        // ViewModelChanged.Invoke("OnDestroy", true);
    }
}