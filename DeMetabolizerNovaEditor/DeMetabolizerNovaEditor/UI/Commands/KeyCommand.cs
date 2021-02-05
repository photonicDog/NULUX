using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DeMetabolizerNovaEditor.UI.Commands
{
    public class KeyCommand : ICommand
    {
        public void Execute(object parameter)
        {
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void PressNoteButton(RadioButton button)
        {
            button.IsChecked = !(button.IsChecked);
        }

        public event EventHandler CanExecuteChanged;
    }
}
