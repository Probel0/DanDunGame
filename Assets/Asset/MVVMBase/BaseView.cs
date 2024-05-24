using UnityEngine;

public abstract class BaseViewSingle<TViewModel> : MonoBehaviour
where TViewModel : I2ViewModel
{
    [SerializeField] private GameObject GMViewModel;
    public TViewModel viewModel;

    public BaseViewSingle(TViewModel viewModel)
    {
        this.viewModel = viewModel;
    }

    private void Awake()
    {
        viewModel = GMViewModel.GetComponent<TViewModel>();
        viewModel.OnViewInit += Initialize;
    }

    protected abstract void Initialize();
}