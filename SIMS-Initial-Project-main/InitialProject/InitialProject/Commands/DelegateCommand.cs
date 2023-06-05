using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InitialProject.Commands
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

}
