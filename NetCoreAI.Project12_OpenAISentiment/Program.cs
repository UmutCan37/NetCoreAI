using Newtonsoft.Json;
using System.Text;

class Prgram
{
    private static readonly string apiKey = "";

    static async Task Main(string[] args)
    {
        Console.WriteLine("Lütfen Metni Giriniz");
        string input = Console.ReadLine();

        if (!string.IsNullOrEmpty(input))
        {
            Console.WriteLine();
            Console.WriteLine("Duygu Analizi Yapılıyor...");
            Console.WriteLine();
            string sentiment = await AnalyzeSentiment(input);

            Console.WriteLine($"Sonuç: {sentiment}");
        }
    }


    static async Task<string> AnalyzeSentiment(string text)
    {
        using (HttpClient client=new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            var reguestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new{role="system",content="You are an AI that analyzes sentiment.You categorize text a Posivite,Negative,or Neutral."},
                    new{role="user",content=$"Analyze the sentiment of this text:\"{text}\"and return only Positive,Negative or Neutral"}
                }
            };

            string json= JsonConvert.SerializeObject(reguestBody);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");


            HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);

            string responseJson = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<dynamic>(responseJson);
                return result.choices[0].message.content.ToString();
            }
            else
            {
                Console.WriteLine("Bir hata oluştu..." + responseJson);
                return "Hata";
            }
        }
    }

}