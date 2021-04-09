using GraphQL;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HttpMultipartParser;
using Newtonsoft.Json;
using System.IO;
using GraphQL.Relay.Utilities;
using GraphQL.SystemTextJson;

namespace GraphQL.Relay.Http
{
    public static class Deserializer
    {
        public static RelayRequest Deserialize(string body, string contentType)
        {
            RelayRequest queries;

            switch (contentType)
            {
                case "application/json":
                //supports dart https://github.com/dart-lang/http/issues/184
                case "application/json; charset=utf-8":
                    queries = DeserializeJson(body);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        $"Unknown media type: {contentType}. Cannot deserialize the Http request");
            }

            return queries;
        }

        public static RelayRequest DeserializeJson(string stringContent)
        {
            if (stringContent[0] == '[')
                return new RelayRequest(
                    JsonConvert.DeserializeObject<RelayQuery[]>(stringContent, new JsonSerializerSettings
                    {
                        // disable Json date serialization as we handle that internally 
                        DateParseHandling = DateParseHandling.None,
                        // amount values are being converted into float by default so change that to a decimal values.
                        FloatParseHandling = FloatParseHandling.Decimal
                    }),
                    isBatched: true
                );

            if (stringContent[0] == '{')
                return new RelayRequest()
                {

                    JsonConvert.DeserializeObject<RelayQuery>(stringContent, new JsonSerializerSettings
                    {
                        // disable Json date serialization as we handle that internally 
                        DateParseHandling = DateParseHandling.None,
                        // amount values are being converted into float by default so change that to a decimal values.
                        FloatParseHandling = FloatParseHandling.Decimal
                    })
                };

            throw new Exception(
                "Unrecognized request json. GraphQL queries requests should be a single object, or an array of objects");
        }
    }
}
