using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;
using Personal_App.Common;
using ST.Models.Paper;
using Personal_App.ViewModel.Exam;

namespace Personal_App.Domain.Exam
{
    /// <summary>
    /// ExamMainWin.xaml 的交互逻辑
    /// </summary>
    public partial class ExamMainWin : UserControl
    {

        public ExamType ExamType { get; set; }

        //public string SimulationId { get; set; } = "-1";

        //public SimulationPaper SimulationPaper { get; set; }

        public ExamMainWin()
        {
            InitializeComponent();

        }

        public ExamMainWin(SimulationPaper simulationPaper, ExamType examType) : this()
        {
            //SimulationPaper = simulationPaper;
            this.ExamType = examType;

            this.DataContext = new ExamMainWinVM(GridContent, ExamType);
        }
    }
}
