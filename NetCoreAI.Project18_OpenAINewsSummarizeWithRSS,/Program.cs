using System.Security.AccessControl;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

class Program
{
    private static readonly string apiKey = "";
    private static readonly string rssFeedUrl = "https://www.sozcu.com.tr/rss/tum-haberler.xml";

    static async Task Main()
    {
        Console.WriteLine("Hberler Sistemden Alınıyor...");
        List<string> articles = await FetchLatestNews(10);

        foreach(var article in articles)
        {
            Console.WriteLine("Haber özeti oluşturuluyor..");
            string summary = await SummarizeArticle(article);
            Console.WriteLine(summary);
            Console.WriteLine("-------------------------------");
            
        }
    }

    static async Task<List<string>> FetchLatestNews(int count)
    {
        var client = new HttpClient();
        string rssContent = await client.GetStringAsync(rssFeedUrl);

        XDocument doc = XDocument.Parse(rssContent);
        var items = doc.Descendants("item").Take(count);

        List<string> articles = items.Select(item =>
        {
            string title = item.Element("title")?.Value ?? "";
            string description = item.Element("description")?.Value ?? "";
            return $"{title} {description}";
        }).ToList();
        return articles;
    }

    static async Task<string> SummarizeArticle(string articleText)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            var reguestBody = new
            {
                model = "gpt-4-turbo",
                messages = new[]
                {
                    new{role="system",content="You are an expert news summarize"},
                    new{role="user",content="Bu haberi 3 cümlede özetle"+articleText}

                },
                max_tokens = 500
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(reguestBody), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions", jsonContent);
            string responseContent = await response.Content.ReadAsStringAsync();

            JsonDocument doc = JsonDocument.Parse(responseContent);
            return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
        }
    }
}
