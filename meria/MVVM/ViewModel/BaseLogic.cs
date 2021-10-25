
using meria.MVVM.Model;
using meria.MVVM.View;
using meria.MVVM.ViewModel.Base;
using meria.Services;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace meria.MVVM.ViewModel
{
    internal class BaseLogic : BaseViewModel
    {
        #region Variables
        DataBaseOnly db;
        private string _New;
        public string New
        {
            get => _New;
            set => Set(ref _New, value);
        }
        private UsersForSecondPage _SelectedUser;
        public UsersForSecondPage SelectedUser
        {
            get => _SelectedUser;
            set => Set(ref _SelectedUser, value);
        }
        private ObservableCollection<UsersForSecondPage> _ListForSecondWindow = new ObservableCollection<UsersForSecondPage>();
        public ObservableCollection<UsersForSecondPage> ListForSecondWindow
        {
            get => _ListForSecondWindow;
            set => Set(ref _ListForSecondWindow, value);
        }
        #endregion

        #region Command
        private LambdaCommand _SaveNewUser;
        public LambdaCommand SaveNewUser =>
            _SaveNewUser ?? (_SaveNewUser = new LambdaCommand(ExecuteSendCheckCommand, CanExecuteSendCheckCommand));

        void ExecuteSendCheckCommand(object p)
        {
            if (!string.IsNullOrWhiteSpace(New))
            {
                db = new DataBaseOnly();
                db.InsertData("INSERT INTO `gorkenesh`.`users` (`name`) VALUES ('" + New + "')");
                New = "";
                GetSours();
            }
        }
        bool CanExecuteSendCheckCommand(object p)
        {
            return true;
        }
        private LambdaCommand _SaveSelectedUser;
        public LambdaCommand SaveSelectedUser =>
            _SaveSelectedUser ?? (_SaveSelectedUser = new LambdaCommand(ExecuteSendCheckCommand1, CanExecuteSendCheckCommand1));

        void ExecuteSendCheckCommand1(object p)
        {
            if (SelectedUser != null)
            {
                db = new DataBaseOnly();
                db.InsertData("UPDATE `gorkenesh`.`users` SET `name`='" + SelectedUser.Name + "' WHERE  `id`=" + SelectedUser.Id);
                SelectedUser = null;
                GetSours();
            }
        }
        bool CanExecuteSendCheckCommand1(object p)
        {
            return true;
        }
        private LambdaCommand _DeleteSelectedUser;
        public LambdaCommand DeleteSelectedUser =>
            _DeleteSelectedUser ?? (_DeleteSelectedUser = new LambdaCommand(ExecuteSendCheckCommand2, CanExecuteSendCheckCommand2));

        void ExecuteSendCheckCommand2(object p)
        {
            if (SelectedUser != null)
            {
                db = new DataBaseOnly();
                db.InsertData("DELETE FROM `gorkenesh`.`users` WHERE  `id`=" + SelectedUser.Id);
                SelectedUser = null;
                GetSours();
            }
        }
        bool CanExecuteSendCheckCommand2(object p)
        {
            return true;
        }
        private LambdaCommand _GiveTimeSelectedUser;
        public LambdaCommand GiveTimeSelectedUser =>
            _GiveTimeSelectedUser ?? (_GiveTimeSelectedUser = new LambdaCommand(ExecuteSendCheckCommand3, CanExecuteSendCheckCommand3));

        void ExecuteSendCheckCommand3(object p)
        {
            string xd = "";
            if (SelectedUser != null)
            {
                db = new DataBaseOnly();
                db.del += (x) => 
                {
                    if (x.Rows.Count > 0) 
                    {
                        if (x.Rows[0][0].ToString() == "0")
                        {
                            xd= x.Rows[0][0].ToString();
                            DataBaseOnly d = new DataBaseOnly();
                            d.InsertData("INSERT INTO `gorkenesh`.`queue` (`speak_time`, `user_id`) VALUES ((SELECT `value` FROM `gorkenesh`.`options` WHERE `KEY`='defaultTime' LIMIT 1), " + SelectedUser.Id + ")");
                        }
                    }
                };
                
                db.SoursData("SELECT t.bool_var FROM `gorkenesh`.`user_in_queue` AS t WHERE t.user_id="+ SelectedUser.Id + " LIMIT 1");
                SelectedUser = null;
            }
        }
        bool CanExecuteSendCheckCommand3(object p)
        {
            return true;
        }
        #endregion

        #region QueueVariables
        private bool IsStopped = false;
        private ObservableCollection<UsersForQueue> _Queue = new ObservableCollection<UsersForQueue>();
        public ObservableCollection<UsersForQueue> Queue
        {
            get => _Queue;
            set => Set(ref _Queue, value);
        }
        private UsersForQueue _SelectedQueue;
        public UsersForQueue SelectedQueue
        {
            get => _SelectedQueue;
            set
            {
                if (value != null)
                {
                    Set(ref _SelectedQueue, value);
                    string min = (value.Time / 60).ToString();
                    string sec = (value.Time % 60).ToString();
                    SelectedSec = sec.Length == 1 ? "0" + sec : sec;
                    SelectedMin = min.Length == 1 ? "0" + min : min;
                }
            }
        }
        private string _CountQueue = "0";
        public string CountQueue
        {
            get
            {
                return "количество: "+_CountQueue;
            }
            set
            {
                Set(ref _CountQueue, value);
            }
        }
        private string _QueueTimes = "0";
        public string QueueTimes
        {
            get
            {
                return "суммарное время: " + _QueueTimes;
            }
            set
            {
                Set(ref _QueueTimes, value);
            }
        }
        private string _SelectedSec = "00";
        public string SelectedSec
        {
            get
            {
                if(_SelectedSec.Length==1)
                    return "0"+_SelectedSec;
                return _SelectedSec;
            }
            set
            {
                Set(ref _SelectedSec, value);
            }
        }
        private string _SelectedMin="00";
        public string SelectedMin
        {
            get
            {
                if(_SelectedMin.Length==1)
                    return "0"+_SelectedMin;
                return _SelectedMin;
            }
            set
            {
                Set(ref _SelectedMin, value);
            }
        }
        private UsersForQueue _CurrentQueue;
        public UsersForQueue CurrentQueue
        {
            get => _CurrentQueue;
            set
            {
                if (value == null)
                    return;
                Set(ref _CurrentQueue, value);
                string min = (value.Time / 60).ToString();
                string sec = (value.Time % 60).ToString();
                CurrentSecond = sec.Length == 1 ? "0" + sec : sec;
                CurrentMinute = min.Length == 1 ? "0" + min : min;
                TimeSec = value.Time;
            }
        }
        private string _CurrentMinute;
        public string CurrentMinute
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_CurrentMinute))
                    return "00";
                if (_CurrentMinute.Length == 1)
                    return "0" + _CurrentMinute;
                return _CurrentMinute;
            }
            set => Set(ref _CurrentMinute, value);
        }
        private string _CurrentSecond;
        public string CurrentSecond
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_CurrentSecond))
                    return "00";
                if (_CurrentSecond.Length == 1)
                    return "0" + _CurrentSecond;
                return _CurrentSecond;
            }
            set => Set(ref _CurrentSecond, value);
        }
        private int TimeSec = 0;
        #endregion

        #region QueueCommand
        public delegate void ChangedButtonImage (bool xxx);
        public static event ChangedButtonImage SetImage;
            
        private LambdaCommand _Next;
        public LambdaCommand Next =>
            _Next ?? (_Next = new LambdaCommand(NextLogic, CanUseNextLogic));

        void NextLogic(object p)
        {
            if (Queue.Count == 0)
                return;
            if (TimeSec == 0 && Queue.Count > 0 && (CurrentQueue==null|| (CurrentQueue.Name=="" && CurrentQueue.Id=="" ))) 
            {
                CurrentQueue = Queue[0];
                IsStopped = true;
            }
            else if(TimeSec == 0 && Queue.Count > 1 && CurrentQueue != new UsersForQueue()) 
            {
                if(CurrentQueue.Name == Queue[0].Name) 
                {
                    Queue.RemoveAt(0);
                    CurrentQueue = Queue[1];
                }
                else
                    CurrentQueue = Queue[0];
                IsStopped = true;
            }
            else if(TimeSec != 0 && Queue.Count > 1) 
            {
                
                IsDeleteCurrQueue b = new IsDeleteCurrQueue(CurrentQueue.Name);
                b.returnOk += () => 
                {
                    DataBaseOnly d = new DataBaseOnly();
                    d.InsertData("DELETE FROM `gorkenesh`.`queue` WHERE  `id`="+ CurrentQueue.Id);
                    //MessageBox.Show(CurrentQueue.Id + "");
                    if (CurrentQueue.Name == Queue[0].Name)
                    {
                        CurrentQueue = Queue[1];
                        Queue.RemoveAt(0);
                    }
                    else
                        CurrentQueue = Queue[0];
                    IsStopped = true;
                };
                b.ShowDialog();
            }
        }
        bool CanUseNextLogic(object p)
        {
            return true;
        }
        private LambdaCommand _Pause;
        public LambdaCommand Pause =>
            _Pause ?? (_Pause = new LambdaCommand(PauseLogic, CanUsePauseLogic));

        void PauseLogic(object p)
        {
            if (Queue.Count == 0)
                return;
            if (TimeSec != 0 && !IsStopped)
            {
                IsStopped = true;
                SetImage(false);
            }
            else 
            {
                IsStopped = false;
                SetImage(true);
            }
        }
        bool CanUsePauseLogic(object p)
        {
            return true;
        }

        private LambdaCommand _ChangeTimeCommand;
        public LambdaCommand ChangeTimeCommand =>
            _ChangeTimeCommand ?? (_ChangeTimeCommand = new LambdaCommand(ChangeTimeCommandExecuted, CanUsePauseLogic));

        void ChangeTimeCommandExecuted(object p)
        {
            if(Convert.ToInt32(p)== 1 || Convert.ToInt32(p) == -1) 
            {
                if(Convert.ToInt32(SelectedMin) + Convert.ToInt32(p) < 1) 
                {
                    SelectedMin = "00";
                    return;
                }
                SelectedMin = (Convert.ToInt32(SelectedMin) + Convert.ToInt32(p)).ToString();
            }
            else 
            {
                if (Convert.ToInt32(SelectedSec) + Convert.ToInt32(p) < 1)
                {
                    SelectedSec = "00";
                    return;
                }
                if (Convert.ToInt32(SelectedSec) + Convert.ToInt32(p) > 59) 
                {
                    SelectedMin = (Convert.ToInt32(SelectedMin) + 1).ToString();
                    SelectedSec = (Convert.ToInt32(SelectedSec) + Convert.ToInt32(p)-60).ToString();
                    return;
                }
                SelectedSec = (Convert.ToInt32(SelectedSec)+ Convert.ToInt32(p)).ToString();
            }   
        }

        private LambdaCommand _GiveWordCommand;
        public LambdaCommand GiveWordCommand =>
            _GiveWordCommand ?? (_GiveWordCommand = new LambdaCommand(GiveWordCommandExecuted, CanUsePauseLogic));

        void GiveWordCommandExecuted(object p)
        {
            if(SelectedQueue ==null) 
                return;
            if (CurrentQueue != null) 
            {
                if (SelectedQueue.Id == CurrentQueue.Id)
                    return;
            }
            if (TimeSec != 0) 
            {
                IsDeleteCurrQueue b = new IsDeleteCurrQueue(CurrentQueue.Name);
                b.returnOk += () =>
                {
                    DataBaseOnly d = new DataBaseOnly();
                    d.InsertData("DELETE FROM `gorkenesh`.`queue` WHERE  `id`=" + CurrentQueue.Id);

                    CurrentQueue = SelectedQueue;
                    SetImage(false);
                    IsStopped = true;
                };
                b.ShowDialog();
            }
            else 
            {
                CurrentQueue = SelectedQueue;
                SetImage(false);
                IsStopped = false;
                IsStopped = true;
            }
        }

        private LambdaCommand _GiveAllsTimeCommand;
        public LambdaCommand GiveAllsTimeCommand =>
            _GiveAllsTimeCommand ?? (_GiveAllsTimeCommand = new LambdaCommand(GiveAllsTimeCommandExecuted, CanUsePauseLogic));

        void GiveAllsTimeCommandExecuted(object p)
        {
            int timeForAll = 0;
            timeForAll += Convert.ToInt32(_SelectedSec);
            timeForAll += Convert.ToInt32(_SelectedMin)*60;
            db = new DataBaseOnly();
            db.InsertData("UPDATE `gorkenesh`.`queue` SET `speak_time`='"+ timeForAll + "' WHERE  `status`=0 and `speak_time`<>0");
            //
        }


        private LambdaCommand _DeleteUserCommand;
        public LambdaCommand DeleteUserCommand =>
            _DeleteUserCommand ?? (_DeleteUserCommand = new LambdaCommand(DeleteUserCommandExecuted, CanUsePauseLogic));

        void DeleteUserCommandExecuted(object p)
        {
            if(SelectedQueue!=null) 
            {
                int id = Convert.ToInt32(SelectedQueue.Id);
                if (CurrentQueue != null)
                    if (SelectedQueue.Id == CurrentQueue.Id)
                    {
                        TimeSec = 0;
                        CurrentQueue= new UsersForQueue { Name = "", Id = "" };
                        CurrentQueue = null;
                        CurrentSecond = "00";
                        CurrentMinute = "00";
                        SelectedQueue = new UsersForQueue {Name="", Id=""};
                        _SelectedQueue = null;
                    }
                
                db = new DataBaseOnly();

                db.InsertData("DELETE FROM `gorkenesh`.`queue` WHERE `id`="+ id);
                
            } 
            //
        }

        private LambdaCommand _AddTimeToCurrentCommand;
        public LambdaCommand AddTimeToCurrentCommand =>
            _AddTimeToCurrentCommand ?? (_AddTimeToCurrentCommand = new LambdaCommand(AddTimeToCurrentCommandExecuted, CanUsePauseLogic));

        void AddTimeToCurrentCommandExecuted(object p)
        {
            if (SelectedQueue == null)
                return;
            int timeForAll = 0;
            timeForAll += Convert.ToInt32(_SelectedSec);
            timeForAll += Convert.ToInt32(_SelectedMin) * 60;
            if(CurrentQueue!=null && SelectedQueue != null)
                if (SelectedQueue.Id == CurrentQueue.Id) 
                {
                    TimeSec = timeForAll;
                    CurrentMinute = SelectedMin;
                    CurrentSecond = SelectedSec;
                }
            db = new DataBaseOnly();
            db.InsertData("UPDATE `gorkenesh`.`queue` SET `speak_time`='" + timeForAll + "' WHERE  `id`="+SelectedQueue.Id);
        }
        #endregion



        /// <summary>
        /// Снизу логика для таймера
        /// </summary>
        private DispatcherTimer ticTak;
        public BaseLogic()
        {
            
            GetSours();
            ticTak = new DispatcherTimer(DispatcherPriority.Render);
            ticTak.Interval = TimeSpan.FromSeconds(1);
            ticTak.Tick += TimeSpanForSpeak;
            ticTak.Start();
            GetQueue();
            MainPage.destructor += () => 
            {
                ticTak.Tick -= TimeSpanForSpeak;
            };
            MainPage.deleteAllEvent += ()=>
            {
                TimeSec = 0;
                CurrentQueue = new UsersForQueue { Name = "", Id = "" };
                CurrentQueue = null;
                CurrentSecond = "00";
                CurrentMinute = "00";
                SelectedQueue = new UsersForQueue { Name = "", Id = "" };
                _SelectedQueue = null;
            };
        }
        ~BaseLogic() 
        {
            ticTak.Tick -= TimeSpanForSpeak;
        }
        
        private async void TimeSpanForSpeak(object sender, EventArgs args)
        {
            if (TimeSec != 0 && !IsStopped)
            {
                SetImage(true);
                TimeSec--;
                try
                {
                    DataBaseOnly d = new DataBaseOnly();
                    d.InsertData("UPDATE `gorkenesh`.`queue` SET `speak_time`='" + TimeSec + "' WHERE  `id`=" + CurrentQueue.Id);
                    d.InsertData("UPDATE `gorkenesh`.`queue` SET `status`='1' WHERE `id`=" + CurrentQueue.Id);
                    d = null;
                    
                }
                finally { }
                CurrentSecond = (TimeSec % 60).ToString();
                CurrentMinute = (TimeSec / 60).ToString();
                if (TimeSec == 0) 
                {
                    TimeSec = 0;
                    CurrentQueue = new UsersForQueue { Name = "", Id = "" };
                    CurrentQueue = null;
                    CurrentSecond = "00";
                    CurrentMinute = "00";
                    SelectedQueue = new UsersForQueue { Name = "", Id = "" };
                    _SelectedQueue = null;
                }
                
            }
            else 
            {
                SetImage(false);
            }
            GetQueue();
        }
        private async void GetSours()
        {
            Parallel.Invoke(() => {
                db = new DataBaseOnly();
                db.del += (x) =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ListForSecondWindow = new ObservableCollection<UsersForSecondPage>();
                        foreach (DataRow data in x.Rows)
                        {
                            ListForSecondWindow.Add(new UsersForSecondPage { Name = data[1].ToString(), Id = data[0].ToString() });
                        }
                    });
                };
                db.SoursData("SELECT * FROM `gorkenesh`.`users`");
            });
        }
        private async void GetQueue()
        {
            Parallel.Invoke(() => {
                db = new DataBaseOnly();
                db.del += (x) =>
                {
                    int times = 0;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        
                        Queue = new ObservableCollection<UsersForQueue>();
                        foreach (DataRow data in x.Rows)
                        {
                            Queue.Add(new UsersForQueue {Time=Convert.ToInt32(data[1].ToString()), Name= data[5].ToString() , Id= data[0].ToString()});
                            times += Convert.ToInt32(data[1].ToString());
                        }
                        Queue = Queue;
                        CountQueue = Queue.Count.ToString();
                        QueueTimes = (times/60).ToString()+ ":"+ (times % 60).ToString()+" мин";
                    });
                };
                db.SoursData("SELECT *,(SELECT f.name FROM users AS f WHERE f.id=t.user_id) FROM `gorkenesh`.`queue` AS t WHERE `created_time`>CURRENT_DATE() and `speak_time`<>0");
                return;
            });
            
        }
    }
}
