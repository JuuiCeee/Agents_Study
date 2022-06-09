using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace UP_11
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Agents _currentAgent;
        List<ListView> consul = new List<ListView>();
        List<Agents> agents = new List<Agents>();
        int clickedBtnIndex = 1;
        public MainWindow()
        {
            InitializeComponent();
            var context = NiceRustleEntities.GetContext();
            var allTypes = context.TypeAgent.ToList();
            allTypes.Insert(0, new TypeAgent { NameTypeAgent = "Все типы" });
            cbFilter.ItemsSource = allTypes;
            cbFilter.SelectedIndex = 0;
            cbSort.Items.Add("Название по возрастанию");
            cbSort.Items.Add("Размер скидки по возрастанию");
            cbSort.Items.Add("Приоритет по возрастанию");
            cbSort.Items.Add("Название по убыванию");
            cbSort.Items.Add("Размер скидки по убыванию");
            cbSort.Items.Add("Приотритет по убыванию");
            cbSort.SelectedIndex = 0;
            tbPoisk.Text = "";
            UpdateAgents();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateAgents();
        }

        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void tbPoisk_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void btAddAgent_Click(object sender, RoutedEventArgs e)
        {
            Window1 agentics = new Window1(lvAgents.SelectedItem, null);
            agentics.Owner = this;
            agentics.Title = "Добавление агента";
            agentics.ShowDialog();
            UpdateAgents();
        }
        private void UpdateAgents()
        {
            var currentAgents = NiceRustleEntities.GetContext().Agents.ToList();

            if (cbFilter.SelectedIndex > 0)
            {
                TypeAgent agentTypes = cbFilter.SelectedItem as TypeAgent;
                string filter = agentTypes.NameTypeAgent.ToString();
                currentAgents = currentAgents.Where(p => p.AType.Contains(filter)).ToList();
            }
            else currentAgents = NiceRustleEntities.GetContext().Agents.ToList();
            switch (cbSort.SelectedIndex)
            {
                case 0:
                    currentAgents = currentAgents.OrderBy(p => p.NameAgent).ToList();
                    break;
                case 1:
                    currentAgents = currentAgents.OrderBy(p => p.Skidka).ToList();
                    break;
                case 2:
                    currentAgents = currentAgents.OrderBy(p => p.Priority).ToList();
                    break;
                case 3:
                    currentAgents = currentAgents.OrderByDescending(p => p.NameAgent).ToList();
                    break;
                case 4:
                    currentAgents = currentAgents.OrderByDescending(p => p.Skidka).ToList();
                    break;
                case 5:
                    currentAgents = currentAgents.OrderByDescending(p => p.Priority).ToList();
                    break;
            }
            if (tbPoisk.Text != "")
                currentAgents = currentAgents.Where(p => p.NameAgent.ToLower().Contains(tbPoisk.Text.ToLower()) ||
                p.PhoneAgent.ToLower().Contains(tbPoisk.Text.ToLower()) || p.EmailAgent.ToLower().Contains(tbPoisk.Text.ToLower())).ToList();
            var count = stackPan.Children.Count;
            var agentsLength = currentAgents.Count();
            int agentsPerPage = 10;
            var pages = agentsLength / agentsPerPage;
            var filteredAgents = updateButtons(pages, currentAgents);
            lvAgents.ItemsSource = currentAgents;
        }
        public List<Agents> updateButtons(int pages, List<Agents> agents)
        {
            stackPan.Children.Clear();
            for (int i = 1; i <= pages; i++)
            {
                Button btnToAdd = new Button();
                btnToAdd.Content = i;
                btnToAdd.Style = (Style)Resources["pageBtn"];
                btnToAdd.Click += (object sender, RoutedEventArgs e) =>
                {

                };
                //if (i == clickedBtnIndex)
                //{
                //    btnToAdd.FontWeight = FontWeights.Bold;
                //}
                stackPan.Children.Add(btnToAdd);
            }
            return agents;
        }

        private void Item_DoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Window1 agentics = new Window1(lvAgents.SelectedItem, lvAgents.SelectedItem as Agents);
            agentics.Owner = this;
            agentics.Title = "Редактирование агента";
            agentics.ShowDialog();
            UpdateAgents();
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            Agents agent = (Agents)lvAgents.SelectedItem;
            if (agent == null)
            {
                MessageBox.Show("Выберите агента");
                return;
            }
            Document doc = new Document();
            BaseFont baseFont = BaseFont.CreateFont("C:/Windows/Fonts/arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL);
            Font font1 = new Font(baseFont, 16, Font.BOLD);
            Font font2 = new Font(baseFont, Font.DEFAULTSIZE, Font.BOLDITALIC);
            using (var writer = PdfWriter.GetInstance(doc, new FileStream("pdfReport.pdf", FileMode.Create)))
            {
                doc.Open();
                doc.AddTitle("Отчет");
                doc.NewPage();
                doc.Add(new Paragraph("АГЕНТ", font1));
                doc.Add(new Paragraph($"{agent.TypeAgent.NameTypeAgent} | {agent.NameAgent}", font));
                doc.Add(new Paragraph($"Электронная почта: {agent.EmailAgent}", font));
                doc.Add(new Paragraph($"Телефон: {agent.PhoneAgent}", font));
                doc.Add(new Paragraph($"Адрес: {agent.Adress}", font));
                doc.Add(new Paragraph($"Приоритет: {agent.Priority}", font));
                doc.Add(new Paragraph($"Директор: {agent.Director}", font));
                doc.Add(new Paragraph($"ИНН: {agent.INN}", font));
                doc.Add(new Paragraph($"КПП: {agent.KPP}", font));
                doc.Close();
            }
            MessageBox.Show("Документ записан");
        }

        private void btDiagram_Click(object sender, RoutedEventArgs e)
        {
            Diagram diag = new Diagram(agents);
            diag.Show();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var paymentsForRemoving = lvAgents.SelectedItems.Cast<Agents>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить {paymentsForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    NiceRustleEntities.GetContext().Agents.RemoveRange(paymentsForRemoving);
                    NiceRustleEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены");
                    UpdateAgents();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }

            }
        }
    }
}
