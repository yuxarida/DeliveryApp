using System.Windows;

namespace DeliverySystem.UI
{
    public partial class OrderEditorWindow : Window
    {
        public OrderEditorWindow()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}