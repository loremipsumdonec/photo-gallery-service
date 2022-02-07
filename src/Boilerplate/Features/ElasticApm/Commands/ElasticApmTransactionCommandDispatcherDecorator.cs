using Boilerplate.Features.Core.Commands;
using Elastic.Apm.Api;

namespace Boilerplate.Features.ElasticApm.Commands
{
    public class ElasticApmTransactionCommandDispatcherDecorator
        : ICommandDispatcher
    {
        private readonly ICommandDispatcher _decorated;
        private readonly ITracer _tracer;

        public ElasticApmTransactionCommandDispatcherDecorator(ICommandDispatcher decorated, ITracer tracer)
        {
            _decorated = decorated;
            _tracer = tracer;
        }

        public async Task<bool> DispatchAsync(ICommand command)
        {
            ITransaction transaction = null;

            if (_tracer.CurrentTransaction == null)
            {
                transaction = _tracer.StartTransaction(command.GetType().Name, "command");
            }

            try
            {
                return await _decorated.DispatchAsync(command);
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
