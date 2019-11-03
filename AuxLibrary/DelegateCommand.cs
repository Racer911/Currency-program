using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AuxLibrary
{
    public class DelegateCommand : ICommand
    {
        Action _action;
        Action<object> _actionParam;
        Func<bool> _canExecute;

        public DelegateCommand(Action action, Func<bool> canExecute = null)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public DelegateCommand(Action<object> actionParam, Func<bool> canExecute = null)
        {
            _actionParam = actionParam;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute.Invoke();
        }

        public void Execute(object parameter)
        {
            _action?.Invoke();
            _actionParam?.Invoke(parameter);
        }
    }
}
