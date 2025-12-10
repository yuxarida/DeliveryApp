using DeliverySystem.Core.Base;
using DeliverySystem.Core.Services;
using System;

namespace DeliverySystem.UI.ViewModels // <--- ПЕРЕВІР ЦЕЙ РЯДОК
{
    public class ReportViewModel : ObservableObject
    {
        private readonly DbService _dbService;
        private DateTime _startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        private DateTime _endDate = DateTime.Now;
        private string _resultText = "Оберіть період";

        public DateTime StartDate { get => _startDate; set { _startDate = value; OnPropertyChanged(); } }
        public DateTime EndDate { get => _endDate; set { _endDate = value; OnPropertyChanged(); } }
        public string ResultText { get => _resultText; set { _resultText = value; OnPropertyChanged(); } }

        public RelayCommand CalculateCommand { get; }

        public ReportViewModel()
        {
            _dbService = new DbService();
            CalculateCommand = new RelayCommand(DoCalculate);
        }

        private void DoCalculate(object obj)
        {
            try
            {
                decimal sum = _dbService.GetRevenueByPeriod(StartDate, EndDate);
                ResultText = $"Дохід: {sum} грн";
            }
            catch (Exception ex) { ResultText = "Помилка: " + ex.Message; }
        }
    }
}