using System;
using System.IO.Ports;
using System.Net.Http;
using System.Threading.Tasks;
namespace ReceteSistemiAPI;
class Program
{
    static SerialPort  _serialPort;

    static async Task Main(string[] args)
    {
        _serialPort = new SerialPort("COM3", 9600); // COM port ayarını kontrol et
        _serialPort.DataReceived += async (sender, e) =>
        {
            string hastaIdRaw = _serialPort.ReadLine().Trim();
            if (int.TryParse(hastaIdRaw, out int hastaId))
            {
                Console.WriteLine($"Okunan HastaID: {hastaId}");
                await GetHastaFromApi(hastaId);
            }
            else
            {
                Console.WriteLine("Geçersiz RFID verisi.");
            }
        };

        _serialPort.Open();
        Console.WriteLine("RFID okuyucu aktif. Çıkmak için Enter'a bas.");
        Console.ReadLine();
    }

    static async Task GetHastaFromApi(int hastaId)
    {
        using (HttpClient client = new HttpClient())
        {
            string apiUrl = $"http://localhost:5279/api/Hasta/{hastaId}";
            var response = await client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Hasta Verisi:");
                Console.WriteLine(json);
            }
            else
            {
                Console.WriteLine($"API Hatası: {response.StatusCode}");
            }
        }
    }
}