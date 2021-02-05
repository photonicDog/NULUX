using DeMetabolizerNovaEditor.UI.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DeMetabolizerNovaEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<RadioButton> _noteButtons;
        private RadioButton _noteButtonCurrentlyChecked;

        public MainWindow()
        {
            InitializeComponent();
            _noteButtons = new List<RadioButton>() { heart, diamond, star, circle, line };
        }

        void OnClickNoteButton(object sender, RoutedEventArgs e)
        {
            RadioButton noteButtonPressed = sender as RadioButton;
            if (_noteButtonCurrentlyChecked == noteButtonPressed) {
                noteButtonPressed.IsChecked = false;
                _noteButtonCurrentlyChecked = null;
            } else
            {
                _noteButtonCurrentlyChecked = noteButtonPressed;
            }
        }


        //TODO: key bindings
        //KeyBinding PressNoteButtonViaKey = new KeyBinding(KeyCommand.PressNoteButton(), Key.D1);
            
    }
}
