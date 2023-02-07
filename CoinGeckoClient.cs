// The CoinGeckoClient handles the requests tot CoinGecko
using System.Globalization;
using Newtonsoft.Json;
using tracker.tokens;

// The CoinGecko Client
public class CoinGeckoClient{

    private const string SIMPLE_PRICE_URL_START = "https://api.coingecko.com/api/v3/simple/price?ids=";
    private const string SIMPLE_PRICE_URL_END = "&vs_currencies=usd&include_market_cap=true&include_24hr_change=true";

    // Gets the crypto prices from CoinGecko from an Array of coin names. 
    public async Task GetSimpleCryptoPrices(HttpClient client, string[] cryptoIds)
    {
        var url = GetPricesUrl(cryptoIds);
        var jsonString = await client.GetStringAsync(url);

        var coinList = JsonConvert.DeserializeObject<Dictionary<string, Token>>(jsonString);

        PrintTokenPriceInformation(coinList);
    }

    // Prints the tokenprice information.
    // Includes daily price change and market cap. 
    private void PrintTokenPriceInformation(Dictionary<string, Token> coinList)
    {
        foreach (var token in coinList)
        {
            var tokenName = GetTokenName(token.Key);
            var tokenPrice = GetRoundedNumber(token.Value.usd);
            var dailyChange = GetRoundedNumber(token.Value.usd_24h_change);
            var marketCap = GetFormattedMarketCap(token.Value.usd_market_cap);

            var bullish = false; 
            if(dailyChange >= 0){
                bullish = true;
            }

            System.Console.Write($"{tokenName} is ");
            Console.ForegroundColor = bullish ? ConsoleColor.Green : ConsoleColor.DarkRed;
            System.Console.WriteLine($"${tokenPrice}");
            Console.ResetColor();
            System.Console.Write($"24H price change: ");
            Console.ForegroundColor = bullish ? ConsoleColor.Green : ConsoleColor.DarkRed;
            System.Console.WriteLine($"{dailyChange}%");
            Console.ResetColor();
            System.Console.WriteLine($"Mcap: {marketCap}\n");
        }
    }

    // Rounds a number to 3 decimals
    private double GetRoundedNumber(double number)
    {
        number = Math.Round(number, 3);
        
        return number;
    }

    // Formats the marketcap to include dots for readability. 
    private string GetFormattedMarketCap(double marketCap)
    {
        // No decimals pls 
        marketCap = Math.Round(marketCap, 0);

        // Create specific culture, so large numbers are divided by dots. 
        CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("el-GR");

        // Add M or B depending on how large the mcap is 
        if(marketCap > 1000000000)
        {
            return marketCap.ToString("0,,,.#B", cultureInfo);
        }

        if(marketCap > 1000000)  
        {
            return marketCap.ToString("#,##0,,.#M", cultureInfo);
        }
        
        // Yes to dots pls 
        return marketCap.ToString("0,0", cultureInfo);
    }

    // Removes the dash ("-") and all chars after that from a string.
    // Used for CoinGecko token Id's, where dashes are used before adding additional information.
    private string GetTokenName(string tokenName)
    {
        var pos = tokenName.IndexOf("-");
        if (pos >= 0)
        {
            tokenName = tokenName.Remove(pos);
        }

        return tokenName.ToUpper();
    }

    // Creates the Url needed for requesting prices in the GetSimpleCryptoPrices method
    private string GetPricesUrl(string[] cryptoIds) {
        var result = SIMPLE_PRICE_URL_START;

        if (cryptoIds != null) {
            
            if(cryptoIds.Length == 1) 
            {
                return cryptoIds[0];
            }

            foreach (var id in cryptoIds)
            {
                result += $"{id}%2C";
            }
        }
        
        return result + SIMPLE_PRICE_URL_END;
    }
}

