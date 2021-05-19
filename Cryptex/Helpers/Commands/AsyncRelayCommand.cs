using System;
using System.Threading.Tasks;

namespace Cryptex.Helpers.Commands
{
    public class AsyncRelayCommand: AsyncCommandBase
    {
        private readonly Func<object,Task> _callback;
        private readonly Action<Exception> _onException;
        public AsyncRelayCommand(Func<object,Task> callback, Action<Exception> onException) : base(onException)
        {
            _callback = callback;
            _onException = onException;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _callback(parameter);
            }
            catch (Exception ex)
            {
                _onException?.Invoke(ex);
            }
        }
    }
}
