
using System.Globalization;
using RequisiçõesMarvel.src.Config;
using RestSharp;

const string baseUrl = "https://gateway.marvel.com:443/v1/public/characters";
Dictionary<string, string> dictonary = new Dictionary<string, string>();

MyConfigurator.TS = DateTime.Now;
dictonary.Add("limit", "1");
dictonary.Add("offset", "1");
dictonary.Add("ts", MyConfigurator.TS.ToString("yyyy-dd-MMHH:mm:ss:ffff", CultureInfo.InvariantCulture));
dictonary.Add("apikey", MyConfigurator.getPublicKey());
dictonary.Add("hash", MyConfigurator.getHash());

var requestSearchParams = "";
foreach (var item in dictonary) {
  requestSearchParams += $"{item.Key}={item.Value}&";
}
requestSearchParams = requestSearchParams.Substring(0, requestSearchParams.Length - 1);


RestClientOptions options = new RestClientOptions(baseUrl);
RestClient client = new RestClient(options);
RestRequest request = new RestRequest($"?{requestSearchParams}");
RestResponse response = client.ExecuteGet(request);
Console.WriteLine(response.Content);