using meria.MVVM.ViewModel.Base;

namespace meria.MVVM.Model
{
    class UsersForQueue:BaseViewModel
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
        private int _Time;
        public int Time
        {
            get => _Time;
            set => Set(ref _Time, value);
        }
        public string TimeForView
        {
            get 
            {
                string o = (_Time / 60).ToString();
                if (o.Length == 1)
                    o = "0"+o;
                o += ":";
                if((_Time % 60).ToString().Length==1)
                    o += "0" + (_Time % 60).ToString();
                else
                    o +=(_Time % 60).ToString();
                return o;
            }
        }
    }
}
