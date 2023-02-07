// Initializes the  HttpClient
using System.Globalization;
using HttpClient client = new();

// Initializes the CoinGeckoClient
CoinGeckoClient coingeckoClient = new CoinGeckoClient();

// Create a list to fill with coin names
List<string> listOfCoins = new List<string>();

// Add coins to the list of coins
listOfCoins.Add("bitcoin");
listOfCoins.Add("ethereum");
listOfCoins.Add("avalanche-2");
listOfCoins.Add("request-network");
listOfCoins.Add("phantasma");

// Convert List to Array for the Get method 
var arrayOfCoins = listOfCoins.ToArray();

// Prepare loop:
// 1. Culture information for DateTime notation
// 2. Define a string divider
var culture = new CultureInfo("en-GB"); 
var halfHour = 1800000;

// Get price information of cryptocurrencies every half hour
while (true)
{
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    //System.Console.WriteLine($"\n{divider}\n{DateTime.Now.ToString(culture)}\n{divider}\n");
    System.Console.WriteLine($"\n{DateTime.Now.ToString(culture)}\n");
    Console.ResetColor();
    await coingeckoClient.GetSimpleCryptoPrices(client, arrayOfCoins);
    Thread.Sleep(halfHour);    
}
 



