using Newtonsoft.Json;
using System.Text;

class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Lütfen Çevirmek İstediğiniz Cümleyei Giriniz");
        string inputText = Console.ReadLine();

        string apiKey = "";

        string translatedText = await TranslateTextToEnglish(inputText,apiKey);

        if (!string.IsNullOrEmpty(translatedText))
        {
            Console.WriteLine($"Ceviri : {translatedText}");
        }
        else
        {
            Console.WriteLine("Beklenmeyen bi hata oluştur");
        }
    }

    private static async Task<string> TranslateTextToEnglish(string text,string apiKey)
    {
        using(HttpClient client=new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            var reguestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new{role="system",content="You Are a helpful translator"},
                    new{role="user",content=$"please translate this text to english: {text}"}
                }
            };

            string jsonBody = JsonConvert.SerializeObject(reguestBody);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
                string responseString = await response.Content.ReadAsStringAsync();

                dynamic responseObject = JsonConvert.DeserializeObject(responseString);
                string translation = responseObject.choices[0].message.content;

                return translation;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Bir hata var {ex.Message}");
                return null;
            }
        }
    }
}
