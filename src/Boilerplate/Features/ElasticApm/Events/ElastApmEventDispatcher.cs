using Boilerplate.Features.Reactive.Events;
using Boilerplate.Features.Reactive.Services;
using Elastic.Apm.Api;

namespace Boilerplate.Features.ElasticApm.Events
{
    public class ElastApmEventDispatcher
        : IEventDispatcher
    {
        private readonly IEventDispatcher _decorated;
        private readonly ITracer _tracer;

        public ElastApmEventDispatcher(IEventDispatcher decorated, ITracer tracer)
        {
            _decorated = decorated;
            _tracer = tracer;
        }

        public void Dispatch(IEvent @event)
        {
            ISpan span = null;

            if (_tracer.CurrentTransaction != null)
            {
                span = _tracer.CurrentTransaction.StartSpan(@event.GetType().Name, "event");
            }

            try
            {
                _decorated.Dispatch(@event);
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
