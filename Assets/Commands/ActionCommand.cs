using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class ActionCommand : ICommand
    {
        private readonly Action _action = null;

        public ActionCommand(Action action)
        {
            _action = action;
        }

        public void Execute() => _action?.Invoke();
    }
}
