using System;
using System.Threading;
using System.Threading.Tasks;

namespace apos_gestor_caja.Helpers
{
    public class DebounceHelper : IDisposable
    {
        private CancellationTokenSource _cancellationTokenSource;
        private readonly object _lock = new object();

        public async Task Debounce(Func<Task> action, int milliseconds)
        {
            lock (_lock)
            {
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource = new CancellationTokenSource();
            }

            var token = _cancellationTokenSource.Token;
            try
            {
                await Task.Delay(milliseconds, token);
                if (!token.IsCancellationRequested)
                {
                    await action();
                }
            }
            catch (TaskCanceledException)
            {
                // Ignored as this is expected behavior
            }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource?.Dispose();
            }
        }
    }
}
