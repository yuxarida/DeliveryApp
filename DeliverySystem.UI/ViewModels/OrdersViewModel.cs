using DeliverySystem.Core.Base;
using DeliverySystem.Core.Models;
using DeliverySystem.Core.Services;
using System.Data;
using System.Windows;

namespace DeliverySystem.UI.ViewModels // <--- ПЕРЕВІР ЦЕЙ РЯДОК
{
    public class OrdersViewModel : ObservableObject
    {
        private readonly DbService _dbService;
        private readonly User _currentUser;

        private DataTable _orders;
        public DataTable Orders { get => _orders; set { _orders = value; OnPropertyChanged(); } }

        private DataRowView _selectedOrder;
        public DataRowView SelectedOrder { get => _selectedOrder; set { _selectedOrder = value; OnPropertyChanged(); } }

        public Visibility AdminVisibility => (_currentUser.Role == "Admin" || _currentUser.Role == "Manager")
                                             ? Visibility.Visible : Visibility.Collapsed;

        public RelayCommand AddCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand RefreshCommand { get; }

        public OrdersViewModel(User user)
        {
            _currentUser = user;
            _dbService = new DbService();

            AddCommand = new RelayCommand(DoAdd);
            DeleteCommand = new RelayCommand(DoDelete);
            RefreshCommand = new RelayCommand(o => LoadData());

            LoadData();
        }

        private void LoadData()
        {
            try { Orders = _dbService.GetOrders(_currentUser); }
            catch { }
        }

        private void DoAdd(object obj)
        {
            var editor = new OrderEditorWindow(); // Цей клас створимо пізніше
            if (editor.ShowDialog() == true)
            {
                try
                {
                    int send = int.Parse(editor.SenderIdTxt.Text);
                    int recv = int.Parse(editor.ReceiverIdTxt.Text);
                    decimal w = decimal.Parse(editor.WeightTxt.Text);
                    decimal p = decimal.Parse(editor.PriceTxt.Text);

                    _dbService.AddOrder(send, recv, null, w, p, "Нове");
                    LoadData();
                }
                catch (System.Exception ex) { MessageBox.Show("Помилка: " + ex.Message); }
            }
        }

        private void DoDelete(object obj)
        {
            if (SelectedOrder == null) return;
            if (MessageBox.Show("Видалити?", "Підтвердження", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    _dbService.DeleteOrder((int)SelectedOrder["order_id"]);
                    LoadData();
                }
                catch (System.Exception ex) { MessageBox.Show(ex.Message); }
            }
        }
    }
}