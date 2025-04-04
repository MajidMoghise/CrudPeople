namespace CrudPeople.API.ViewModels.Base
{
    public class DataResponseViewModel<TViewModels> : ResponseViewModel
    {

        public TViewModels Data { get; set; }
    }
}
