using Google.Cloud.Vision.V1;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Resim Yolu Giriniz:");
        string imagePath=Console.ReadLine();

        string credentialPath = "buraya servis json dosyası gelcek";
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREEDENTIALS", credentialPath);

        try
        {
            var client = ImageAnnotatorClient.Create();
            var image = Image.FromFile(imagePath);
            var response = client.DetectText(image);
            Console.WriteLine("Resimdeki Metin=");
            Console.WriteLine();
            foreach(var annotination in response)
            {
                if (!string.IsNullOrEmpty(annotination.Description))
                {
                    Console.WriteLine(annotination.Description);
                }
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Bir hata oluştu {ex.Message} ");
        }
    }
}