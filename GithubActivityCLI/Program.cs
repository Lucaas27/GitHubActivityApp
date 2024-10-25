using System.Text.Json;
using GitHubApiClient;
using GitHubApp.Models;

var httpClient = new HttpClient();  // Create a new HttpClient instance
var apiClient = new GitHubApi(httpClient);

var response = await apiClient.GetRecentActivityAsync("lucaas27");  // Call the GetRecentActivityAsync method

if (string.IsNullOrEmpty(response))
{
    Console.WriteLine("No activity found for the specified user.");
    return;
}

var events = JsonSerializer.Deserialize<List<EventModel>>(response);  // Deserialize the JSON response
