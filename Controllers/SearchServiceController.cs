using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Azure.Search.Documents.Models;
using CloudApplication.Models;

public class SearchServiceController : Controller
{
	private readonly AzureSearchService _searchService;

	public SearchServiceController(AzureSearchService searchService)
	{
		_searchService = searchService;
	}

	public async Task<IActionResult> Index(string query)
	{
		IEnumerable<SearchDocument> results = await _searchService.SearchAsync(query);
		return View(results);
	}
}
