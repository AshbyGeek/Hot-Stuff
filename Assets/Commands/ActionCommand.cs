using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    /// <summary>
    /// A command that can be given an action to perform.
    /// </summary>
    public class ActionCommand : ICommand
    {
        private readonly Action _action = null;

        public string Name { get; }

        public ActionCommand(string name, Action action)
        {
            Name = name;
            _action = action;
        }

        public void Execute() => _action?.Invoke();
    }
}
