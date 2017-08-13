using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Newtonsoft.Json;
using System.Net;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace GetDynamoDB
{
    public class Function
    {
        private static readonly AmazonDynamoDBClient Client = new AmazonDynamoDBClient(RegionEndpoint.APNortheast1);

        public TableValue[] FunctionHandler(string input, ILambdaContext context)
        {
            var dbContext = new DynamoDBContext(Client);
            var emaList = dbContext.QueryAsync<TableValue>(input).GetNextSetAsync().Result;
            var test = new List<TableValue>();

            TableValue[] emaArray =emaList.ToArray();

            return emaArray; //new LambdaResponse
            //{
            //    // StatusCode = HttpStatusCode.OK,
            //    // EmaList = JsonConvert.SerializeObject(emaArray)
            //    EmaList = emaArray
            //};
        }
    }

    [DynamoDBTable("PSO2ema")]
    public class TableValue
    {
        [DynamoDBHashKey]
        [JsonProperty(PropertyName = "key")] // yyyymmdd
        public string Key { get; set; }

        [DynamoDBRangeKey]
        [JsonProperty(PropertyName = "rkey")] //hhmm
        public string Rkey { get; set; }

        [DynamoDBProperty("EvantName")]
        [JsonProperty(PropertyName = "evant")]
        public string EventName { get; set; }

        [DynamoDBProperty("Month")]
        [JsonProperty(PropertyName = "month")]
        public int Month { get; set; }

        [DynamoDBProperty("Date")]
        [JsonProperty(PropertyName = "date")]
        public int Date { get; set; }

        [DynamoDBProperty("Hour")]
        [JsonProperty(PropertyName = "hour")]
        public int Hour { get; set; }
    }

    public class LambdaResponse
    {
        //[JsonProperty(PropertyName = "statusCode")]
        //public HttpStatusCode StatusCode { get; set; }

        [JsonProperty(PropertyName = "emajson")]
        public TableValue[] EmaList { get; set; }
    }
}