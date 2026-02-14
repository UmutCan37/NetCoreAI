using System;
using Tesseract;
class Program { 
static void Main(string[] args)
{
    Console.Write("Karakter Okuması Yapılcak Resim Yolu=");
    string imagePath = Console.ReadLine();

    string tessDataPath = @"C:\tessdata";
    try
    {
        using(var engine=new TesseractEngine(tessDataPath, "eng", EngineMode.Default))
        {
            using(var img = Pix.LoadFromFile(imagePath))
            {
                using(var page = engine.Process(img))
                {
                    string text = page.GetText();
                    Console.WriteLine("Resimden Okunun Metin=");
                    Console.WriteLine(text);
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Bir Hata oluştu:{ex.Message}");
    }
        Console.ReadLine();
    }
}