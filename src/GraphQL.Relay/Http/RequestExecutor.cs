using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GraphQL.SystemTextJson;

namespace GraphQL.Relay.Http
{
    public class RequestExecutor
    {
        private readonly IDocumentExecuter _executer = new DocumentExecuter();
        private readonly IDocumentWriter _writer = new DocumentWriter();

        public RequestExecutor()
        {
        }

        public RequestExecutor(IDocumentExecuter executer, IDocumentWriter writer)
        {
            _executer = executer;
            _writer = writer;
        }

        /// <summary>
        /// This method is currently used by Soft.Framework
        /// </summary>
        /// <param name="queries"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public async Task<RelayResponse> ExecuteAsync(RelayRequest queries, Action<ExecutionOptions, IEnumerable<HttpFile>> configure)
        {
            var results = await Task.WhenAll(
                queries.Select(q => _executer.ExecuteAsync(options =>
                {
                    options.Query = q.Query;
                    options.OperationName = q.OperationName;
                    options.Inputs = q.Variables;

                    configure(options, queries.Files);
                }))
            );

            return new RelayResponse
            {
                Writer = _writer,
                IsBatched = queries.IsBatched,
                Results = results
            };
        }
    }
}
