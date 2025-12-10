using DeliverySystem.Core.Base;
using DeliverySystem.Core.Models;
using DeliverySystem.Core.Services;
using System.Windows;
using System.Windows.Controls;

namespace DeliverySystem.UI.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly DbService _dbService;
        private User _currentUser;

        // Навігація
        private object _currentView;
        public object CurrentView { get => _currentView; set { _currentView = value; OnPropertyChanged(); } }

        // Логін
        private string _loginUsername;
        public string LoginUsername { get => _loginUsername; set { _loginUsername = value; OnPropertyChanged(); } }

        private Visibility _loginVis = Visibility.Visible;
        public Visibility LoginVis { get => _loginVis; set { _loginVis = value; OnPropertyChanged(); } }

        private Visibility _navVis = Visibility.Collapsed;
        public Visibility NavVis { get => _navVis; set { _navVis = value; OnPropertyChanged(); } }

        public string WelcomeMsg => _currentUser != null ? $"{_currentUser.Username} ({_currentUser.Role})" : "";

        // Команди
        public RelayCommand LoginCommand { get; }
        public RelayCommand LogoutCommand { get; }
        public RelayCommand GoToOrdersCommand { get; }
        public RelayCommand GoToReportCommand { get; }

        public MainViewModel()
        {
            _dbService = new DbService();
            LoginCommand = new RelayCommand(DoLogin);
            LogoutCommand = new RelayCommand(DoLogout);

            // Навігація
            GoToOrdersCommand = new RelayCommand(o => CurrentView = new OrdersViewModel(_currentUser));
            GoToReportCommand = new RelayCommand(o => CurrentView = new ReportViewModel());
        }

        private void DoLogin(object parameter)
        {
            var pb = parameter as PasswordBox;
            var user = _dbService.Login(LoginUsername, pb?.Password);
            if (user != null)
            {
                _currentUser = user;
                OnPropertyChanged(nameof(WelcomeMsg));

                LoginVis = Visibility.Collapsed;
                NavVis = Visibility.Visible;

                // За замовчуванням відкриваємо замовлення
                CurrentView = new OrdersViewModel(_currentUser);
            }
            else MessageBox.Show("Помилка входу");
        }

        private void DoLogout(object obj)
        {
            _currentUser = null;
            LoginUsername = "";
            NavVis = Visibility.Collapsed;
            LoginVis = Visibility.Visible;
        }
    }
}