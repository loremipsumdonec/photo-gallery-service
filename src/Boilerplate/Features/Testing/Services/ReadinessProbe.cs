
namespace Boilerplate.Features.Testing.Services
{
    public abstract class ReadinessProbe
        : IReadinessProbe
    {
        private readonly int _retries;

        public ReadinessProbe()
            : this(10)
        {
        }

        public ReadinessProbe(int retries) 
        {
            _retries = retries;
        }

        public async Task WaitAsync()
        {
            int tick = 0;

            while (!await IsReadyAsync())
            {
                await Task.Delay(1000);

                if (tick++ > _retries)
                {
                    throw new TimeoutException("Readiness check timeout");
                }
            }
        }

        protected abstract Task<bool> IsReadyAsync();
    }
}
