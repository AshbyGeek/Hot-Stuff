using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    /// <summary>
    /// A command that does nothing
    /// </summary>
    public sealed class NullCommand : ICommand
    {
        /// <summary>
        /// Use <see cref="NullCommand.Instance"/> instead of constructing a new instance of the null command
        /// </summary>
        private NullCommand() { }

        public string Name => "Do Nothing";

        public void Execute() { }

        public static NullCommand Instance = new NullCommand();
    }
}
