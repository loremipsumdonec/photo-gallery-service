using Boilerplate.Features.Reactive.Events;
using Boilerplate.Features.Reactive.Services;
using Elastic.Apm.Api;

namespace Boilerplate.Features.ElasticApm.Events
{
    public class ElastApmTransactionEventDispatcher
        : IEventDispatcher
    {
        private readonly IEventDispatcher _decorated;
        private readonly ITracer _tracer;

        public ElastApmTransactionEventDispatcher(IEventDispatcher decorated, ITracer tracer)
        {
            _decorated = decorated;
            _tracer = tracer;
        }

        public void Dispatch(IEvent @event)
        {
            ITransaction transaction = null;

            if (_tracer.CurrentTransaction == null)
            {
                transaction = _tracer.StartTransaction(@event.GetType().Name, "event");
            }

            try 
            { 
                _decorated.Dispatch(@event);
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    transaction.CaptureException(ex);
                }

                throw;
            }
            finally
            {
                if (transaction != null)
                {
                    transaction.End();
                }
            }
        }
    }
}
