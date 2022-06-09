using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace UP_11
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private Agents _currentAgent = new Agents();
        private string Pathfile;

        public Window1(object agentId, Agents agent)
        {
            InitializeComponent();
            cbType.ItemsSource = NiceRustleEntities.GetContext().TypeAgent.ToList();
            if (agent != null) { cbType.SelectedIndex = agent.TypeAgent.ID - 1; }
            else { cbType.SelectedIndex = 0; }
            agentId = null;
            if (agent == null)
            {
                _currentAgent.Id = Convert.ToInt32(agentId);
            }

            else _currentAgent = agent;
            {
                DataContext = _currentAgent;
            }

        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            string nameAgent = tbNameAgent.Text;
            string priority = tbPriority.Text;
            string adress = tbAdress.Text;
            string INN = tbINN.Text;
            string KPP = tbKPP.Text;
            string director = tbDirector.Text;
            string phone = tbPhone.Text;
            string email = tbEmail.Text;
            
            if (_currentAgent.Id == 0)
                NiceRustleEntities.GetContext().Agents.Add(_currentAgent);

            if (String.IsNullOrEmpty(nameAgent) || String.IsNullOrEmpty(priority) ||
                String.IsNullOrEmpty(adress) || String.IsNullOrEmpty(INN) ||
                String.IsNullOrEmpty(KPP) || String.IsNullOrEmpty(director) || String.IsNullOrEmpty(phone) ||
                String.IsNullOrEmpty(email))
            {
                MessageBox.Show("Некорректный ввод данных!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                try
                {
                    NiceRustleEntities.GetContext().SaveChanges();
                    if (MessageBox.Show("Информация сохранена", "", MessageBoxButton.OK) == MessageBoxResult.OK)
                        Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var index = NiceRustleEntities.GetContext().Agents.ToList().Count();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;)|*.png;";
            if (openFileDialog.ShowDialog() == true)
            {
                Pathfile = openFileDialog.FileName;
                if (File.Exists(_currentAgent.LogoAgent)) File.Delete(_currentAgent.LogoAgent);
                string newPath = "";
                if (_currentAgent.Id == 0)
                {
                    newPath = $"E:\\agents\\agent_{index + 10}";
                    File.Move(Pathfile, newPath);
                    _currentAgent.LogoAgent = newPath;
                }
                else
                {
                    File.Move(Pathfile, $"{_currentAgent.LogoAgent}");
                }
                    imLogo.Source = new BitmapImage(new Uri(newPath));
                
            }


        }

        private void cbType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _currentAgent.IdTypeAgent = cbType.SelectedIndex + 1;
        }
    }
}
