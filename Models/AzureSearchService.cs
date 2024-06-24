using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;

namespace CloudApplication.Models
{
	public class AzureSearchService
	{
		private readonly SearchClient _searchClient;

		public AzureSearchService(IConfiguration configuration)
		{
			string serviceName = configuration["AzureSearch:SearchServiceName"];
			string apiKey = configuration["AzureSearch:SearchServiceApiKey"];
			string indexName = configuration["AzureSearch:SearchIndexName"];

			Uri serviceEndpoint = new Uri($"https://{serviceName}.search.windows.net");
			AzureKeyCredential credential = new AzureKeyCredential(apiKey);
			_searchClient = new SearchClient(serviceEndpoint, indexName, credential);
		}

		public async Task<IEnumerable<SearchDocument>> SearchAsync(string searchText)
		{
			var options = new SearchOptions
			{
				IncludeTotalCount = true,
				Size = 20,
				Filter = "Availability eq true",
				OrderBy = { "Price asc" }
			};

			SearchResults<SearchDocument> results = await _searchClient.SearchAsync<SearchDocument>(searchText, options);
			return results.GetResults().Select(r => r.Document);
		}
	}
}
