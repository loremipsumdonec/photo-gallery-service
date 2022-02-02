
namespace Boilerplate.Features.Testing.Services
{
    public class CompositeReadinessProbe
        : IReadinessProbe
    {
        private readonly IList<IReadinessProbe> _probes = new List<IReadinessProbe>();

        public void Add(IReadinessProbe probe) 
        {
            _probes.Add(probe);
        }

        public Task WaitAsync()
        {
            Task.WaitAll(_probes.Select(x => x.WaitAsync())
                .ToArray()
            );

            return Task.CompletedTask;
        }
    }
}
