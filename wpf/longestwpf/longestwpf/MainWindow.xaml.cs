using Assets;
using LongestPathConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace longestwpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string N { get; set; } = "0";
        public MainWindow()
        {
            InitializeComponent();
            myN.DataContext = this;
        }

        private void OnCalcClick(object sender, RoutedEventArgs e)
        {
            ResultField.Text = "";

            IPathSolver? solver =
                Algorithm.Text == "Julians Algorithm" ? new Solver_1() :
                Algorithm.Text == "Random Order" ? new Solver_2() :
                Algorithm.Text == "Next 2" ? new Solver_3() :
                Algorithm.Text == "Nex in Row" ? new Solver_4() :
                null;

            if (solver == null)
            {
                InfoLabel.Content = "No Valid Solver";
                return;
            }

            int n = 0;
            bool valid = int.TryParse(N, out n);
            if (!valid)
            {
                InfoLabel.Content = "Invalid N";
                return;
            }

            if (n <= 1)
            {
                InfoLabel.Content = "n must be higher than 1";
                return;
            }

            var res = solver.CalculatePath(n);

            InfoLabel.Content =
                $"N: {n}\n" +
                $"Solver: {Algorithm.Text}\n" +
                $"Optimal Connection Count: {PathHelper.OptimalConnectionCount(n)}\n" +
                $"Our Connection Count: {res.ConnectionCount}\n";

            if (res.Path.Count > 0)
            {
                ResultField.Text = res.Path.Select(x => x.ToString())
                    .Aggregate(
                        (current, next) => current + ", " + next);
            }
        }
    }
}
