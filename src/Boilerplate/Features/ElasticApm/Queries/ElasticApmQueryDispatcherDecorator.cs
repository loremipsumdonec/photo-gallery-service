using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using Elastic.Apm.Api;

namespace Boilerplate.Features.ElasticApm.Queries
{
    internal class ElasticApmQueryDispatcherDecorator
        : IQueryDispatcher
    {
        private readonly IQueryDispatcher _decorated;
        private readonly ITracer _tracer;

        public ElasticApmQueryDispatcherDecorator(IQueryDispatcher decorated, ITracer tracer)
        {
            _decorated = decorated;
            _tracer = tracer;
        }

        public async Task<M> DispatchAsync<M>(IQuery query) where M : class, IModel
        {
            ISpan span = null;

            if (_tracer.CurrentTransaction != null) 
            {
                span = _tracer.CurrentTransaction.StartSpan(query.GetType().Name, "query");
            }

            try 
            {
                return await _decorated.DispatchAsync<M>(query);
            }
            catch (Exception ex) 
            {
                if(span != null) 
                {
                    span.CaptureException(ex);
                }

                throw;
            }
            finally 
            {
                if(span != null) 
                {
                    span.End();
                }
            }
        }

        public async Task<IModel> DispatchAsync(IQuery query)
        {
            ISpan span = null;

            if (_tracer.CurrentTransaction != null)
            {
                span = _tracer.CurrentTransaction.StartSpan(query.GetType().Name, "query");
            }

            try
            {
                return await _decorated.DispatchAsync(query);
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
