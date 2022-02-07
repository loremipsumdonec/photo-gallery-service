using Boilerplate.Features.Core.Commands;
using Elastic.Apm.Api;

namespace Boilerplate.Features.ElasticApm.Commands
{
    public class ElasticApmCommandDispatcherDecorator
        : ICommandDispatcher
    {
        private readonly ICommandDispatcher _decorated;
        private readonly ITracer _tracer;

        public ElasticApmCommandDispatcherDecorator(ICommandDispatcher decorated, ITracer tracer)
        {
            _decorated = decorated;
            _tracer = tracer;
        }

        public async Task<bool> DispatchAsync(ICommand command)
        {
            ISpan span = null;

            if (_tracer.CurrentTransaction != null)
            {
                span = _tracer.CurrentTransaction.StartSpan(command.GetType().Name, "command");
            }

            try
            {
                return await _decorated.DispatchAsync(command);
            }
            catch (Exception ex)
            {
                if (span != null)
                {
                    span.CaptureException(ex);
                }

                throw;
            }
            finally
            {
                if (span != null)
                {
                    span.End();
                }
            }
        }
    }
}
