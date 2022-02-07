using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using Elastic.Apm.Api;

namespace Boilerplate.Features.ElasticApm.Queries
{
    internal class ElasticApmTransactionQueryDispatcherDecorator
        : IQueryDispatcher
    {
        private readonly IQueryDispatcher _decorated;
        private readonly ITracer _tracer;

        public ElasticApmTransactionQueryDispatcherDecorator(IQueryDispatcher decorated, ITracer tracer)
        {
            _decorated = decorated;
            _tracer = tracer;
        }

        public async Task<M> DispatchAsync<M>(IQuery query) where M : class, IModel
        {
            ITransaction transaction = null;

            if (_tracer.CurrentTransaction == null) 
            {
                transaction = _tracer.StartTransaction(query.GetType().Name, "query");
            }

            try 
            {
                return await _decorated.DispatchAsync<M>(query);
            }
            catch (Exception ex) 
            {
                if(transaction != null) 
                {
                    transaction.CaptureException(ex);
                }

                throw;
            }
            finally 
            {
                if(transaction != null) 
                {
                    transaction.End();
                }
            }
        }

        public async Task<IModel> DispatchAsync(IQuery query)
        {
            ITransaction transaction = null;

            if (_tracer.CurrentTransaction == null)
            {
                transaction = _tracer.StartTransaction(query.GetType().Name, "query");
            }

            try
            {
                return await _decorated.DispatchAsync(query);
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
