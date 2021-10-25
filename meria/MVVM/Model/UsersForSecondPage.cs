using meria.MVVM.ViewModel.Base;
namespace meria.MVVM.Model
{
    public class UsersForSecondPage:BaseViewModel
    {
        private string _Id;
        public string Id
        {
            get => _Id;
            set => Set(ref _Id, value);
        }
        private string _Name;
        public string Name 
        {
            get => _Name;
            set => Set(ref _Name, value);
        }
    }
}
