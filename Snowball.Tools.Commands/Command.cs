using System;
using System.Collections.Generic;
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

        public bool EnsureOptions(object options, Action<string> onError)
        {
            this.EnsureOptionsObject(options);
            return this.EnsureOptions((TOptions)options, onError);
        }

        public virtual bool EnsureOptions(TOptions options, Action<string> onError)
        {
            return true;
        }

        public void Execute(object options)
        {
            this.EnsureOptionsObject(options);
            this.Execute((TOptions)options);
        }

        public abstract void Execute(TOptions options);
    }
}
