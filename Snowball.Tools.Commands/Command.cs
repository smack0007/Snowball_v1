using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Snowball.Tools.Commands
{
    /// <summary>
    /// Base class for Command(s).
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public abstract class Command<TOptions> : ICommand
    {
        public Type OptionsType
        {
            get { return typeof(TOptions); }
        }

        public abstract string Description
        {
            get;
        }

        private void EnsureOptionsObject(object options)
        {
            if (options == null)
                throw new ArgumentNullException("options");

            if (!(options is TOptions))
                throw new InvalidOperationException(string.Format("Options must be type {0}.", typeof(TOptions)));
        }

        protected void EnsureParamsAreNotNull(object options, ICommandLogger logger)
        {
            if (options == null)
                throw new ArgumentNullException("options");

            if (logger == null)
                throw new ArgumentNullException("logger");
        }

        public bool EnsureOptions(object options, ICommandLogger logger)
        {
            this.EnsureOptionsObject(options);
            return this.EnsureOptions((TOptions)options, logger);
        }

        public virtual bool EnsureOptions(TOptions options, ICommandLogger logger)
        {
            return true;
        }

        public void Execute(object options, ICommandLogger logger)
        {
            this.EnsureOptionsObject(options);
            this.Execute((TOptions)options, logger);
        }

        public abstract void Execute(TOptions options, ICommandLogger logger);
    }
}
