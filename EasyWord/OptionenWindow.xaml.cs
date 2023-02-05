using System.Windows;
using EasyWord.Models;
using EasyWord.Service.Models;
using EasyWord.ViewModels;

namespace EasyWord
{
    /// <summary>
    /// Interaction logic for OptionenWindow.xaml
    /// </summary>
    public partial class OptionenWindow : Window
    {
        public OptionenWindow(AppConfigurations configs)
        {
            InitializeComponent();

            var context = DataContext as OptionenWindowViewModel;
            context.IsCaseSensitive = configs.IsCaseSensitive;
            context.Lernsprache = configs.LernLanguage;
        }
    }
}
