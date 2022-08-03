using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace APIClientApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var restClient = new RestClient("https://api.postcodes.io/");


            var restRequest = new RestRequest();
            restRequest.Method = Method.Get;
            restRequest.AddHeader("Content-Type", "application/json");

            var postCode = "EC2Y 5AS";
            restRequest.Resource = $"postcodes/{postCode.ToLower().Replace(" ", "")}";

            var singlePostcodeResponse = await restClient.ExecuteAsync(restRequest);
            //Console.WriteLine("Response content (string)");
            //Console.WriteLine(singlePostcodeResponse.Content);
            //Console.WriteLine("Repsonse stauts code (enum)");
            //Console.WriteLine(singlePostcodeResponse.StatusCode);
            //Console.WriteLine("Repsonse stauts code (int)");
            //Console.WriteLine((int)singlePostcodeResponse.StatusCode);

            //foreach(var item in singlePostcodeResponse.Headers)
            //{
            //    Console.WriteLine(item);
            //}

            //var responseContentType = singlePostcodeResponse.Headers.Where(x => x.Name == "Date").Select(h => h.Value.ToString()).FirstOrDefault();
            //Console.WriteLine(responseContentType);


            //var client = new RestClient();
            //var request = new RestRequest("https://api.postcodes.io/postcodes/", Method.Post);
            //request.AddHeader("Content-Type", "application/json");

            ////request.AddStringBody("{\r\n\"postcodes\" : [\"OX49 5NU\", \"M32 0JG\", \"NE30 1DP\"]\r\n}\r\n", DataFormat.Json);

            //var postcodes = new
            //{
            //    Postcodes = new string[] { "PR3 0SG", "M45 6GN", "EX165BL" }
            //};
            //request.AddJsonBody(postcodes);
            //RestResponse response = client.Execute(request);
            ////Console.WriteLine(response.Content);

            //var singlePostcodeJsonResponse = JObject.Parse(singlePostcodeResponse.Content);
            //Console.WriteLine(singlePostcodeJsonResponse);
            //var adminDistrict = singlePostcodeJsonResponse["result"]["admin_district"];
            //var status = singlePostcodeJsonResponse["status"];


            var client = new RestClient();
            var request = new RestRequest("https://api.postcodes.io/postcodes/", Method.Post);
            request.AddHeader("Content-Type", "application/json");

            //request.AddStringBody("{\r\n\"postcodes\" : [\"OX49 5NU\", \"M32 0JG\", \"NE30 1DP\"]\r\n}\r\n", DataFormat.Json);

            var postcodes = new
            {
                Postcodes = new string[] { "PR3 0SG", "M45 6GN", "EX165BL" }
            };
            request.AddJsonBody(postcodes);
            RestResponse bulkPostcodeResponse = client.Execute(request);

            var bulkPostcodeJsonResponse = JObject.Parse(bulkPostcodeResponse.Content);
            //Console.WriteLine(bulkPostcodeJsonResponse);
            //Console.WriteLine(bulkPostcodeJsonResponse["result"][2]["result"]["codes"]["admin_county"]);

            var singlePostcodeObjectRepsone = JsonConvert.DeserializeObject<SinglePostcodeResponse>(singlePostcodeResponse.Content);
            var bulkPostcodeObjectRepsone = JsonConvert.DeserializeObject<BulkPostcodeResponse>(bulkPostcodeResponse.Content);

            Console.WriteLine("Bulk postcode object response:\n");
            foreach(var p in bulkPostcodeObjectRepsone.result)
            {
                Console.WriteLine(p.query);
                Console.WriteLine($"{p.result.admin_ward}\n");
            }

            var selectedAdminCounty = bulkPostcodeObjectRepsone.result.Where(q => q.query == "PR3 0SG").FirstOrDefault().result.codes.admin_county;
            Console.WriteLine(selectedAdminCounty);
            /////
            ///
            var outCode = "EC2Y";

            var outRequest = new RestRequest($"https://api.postcodes.io/outcodes/{outCode.Trim()}", Method.Get);
//            outRequest.Resource = $"outcodes/{outCode.Trim()}/";
            var outClient = new RestClient();

            RestResponse outResponse = await outClient.ExecuteAsync(outRequest);
            var outcodesJsonResponse = JObject.Parse(outResponse.Content);


        }
    }
}