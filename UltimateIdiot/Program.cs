using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HttpClientSample
{
    public class Self
    {
        public string Href { get; set; }
    }

    public class Links
    {
        public Self Self { get; set; }
    }

    public class Author
    {
        public string Author_id { get; set; }
        public object Bio { get; set; }
        public DateTime Created_at { get; set; }
        public string Name { get; set; }
        public string slug { get; set; }
        public DateTime updated_at { get; set; }
        public Links _links { get; set; }
    }

    public class Self2
    {
        public string href { get; set; }
    }

    public class Links2
    {
        public Self2 self { get; set; }
    }

    public class Source
    {
        public DateTime created_at { get; set; }
        public object filename { get; set; }
        public string quote_source_id { get; set; }
        public object remarks { get; set; }
        public DateTime updated_at { get; set; }
        public string url { get; set; }
        public Links2 _links { get; set; }
    }

    public class Embedded2
    {
        public List<Author> author { get; set; }
        public List<Source> source { get; set; }
    }

    public class Self3
    {
        public string href { get; set; }
    }

    public class Links3
    {
        public Self3 self { get; set; }
    }

    public class Quote
    {
        public DateTime appeared_at { get; set; }
        public DateTime created_at { get; set; }
        public string quote_id { get; set; }
        public List<string> tags { get; set; }
        public DateTime updated_at { get; set; }
        public string value { get; set; }
        public Embedded2 _embedded { get; set; }
        public Links3 _links { get; set; }
    }

    public class Embedded
    {
        public List<Quote> quotes { get; set; }
    }

    public class Self4
    {
        public string href { get; set; }
    }

    public class First
    {
        public string href { get; set; }
    }

    public class Prev
    {
        public string href { get; set; }
    }

    public class Next
    {
        public string href { get; set; }
    }

    public class Last
    {
        public string href { get; set; }
    }

    public class Links4
    {
        public Self4 self { get; set; }
        public First first { get; set; }
        public Prev prev { get; set; }
        public Next next { get; set; }
        public Last last { get; set; }
    }

    public class ListOfQuotes
    {
        public int Count { get; set; }
        public int Total { get; set; }
        public Embedded _embedded { get; set; }
        public Links4 _links { get; set; }
    }

    internal class Program
    {
        private static readonly HttpClient client = new HttpClient();

        private static void ShowQuote(Quote quote)
        {
            Console.WriteLine(quote);
        }

        private static async Task<Quote> GetRandomQuote(string path)
        {
            Quote quote = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                quote = await response.Content.ReadAsAsync<Quote>();
            }
            return quote;
        }

        private static async Task<ListOfQuotes> GetSpecificQuote(string path)
        {
            ListOfQuotes quotes = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                quotes = await response.Content.ReadAsAsync<ListOfQuotes>();
            }
            return quotes;
        }

        private static async Task Main()
        {
            Console.BackgroundColor = ConsoleColor.White;
            SetupTheClient();
            Random rnd = new Random();
            ArrayList colors = new ArrayList();
            colors.AddRange(Enum.GetValues(typeof(ConsoleColor)));
            colors.Remove(ConsoleColor.Yellow);
            colors.Remove(ConsoleColor.White);

            colors.TrimToSize();
            int randomColor = 0;

            while (true)
            {
                randomColor = rnd.Next(colors.Count);
                Console.ForegroundColor = (ConsoleColor)colors[randomColor];

                await PrintNewQuoteAsync(Console.ReadLine());
            }
        }

        private static void SetupTheClient()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("https://www.tronalddump.io/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private static async Task GetTheNewThing(String tag)
        {
            if (tag == "")
            {
                try
                {
                    Quote quote = new Quote();
                    quote = await GetRandomQuote(client.BaseAddress + "/random/quote");
                    Console.WriteLine($"At {quote.appeared_at.Date.ToShortDateString()} he said: " + quote.value);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                try
                {
                    ListOfQuotes list = await GetSpecificQuote(client.BaseAddress + "search/quote?query=" + tag);
                    if (list.Count == 0)
                    {
                        Console.WriteLine($" ---------- The idiot has nothing to say about this. Thank god.");
                    }
                    else
                    {
                        Console.WriteLine($" ---------- The idiot has {list.Count} things to say about this. Yikes.");
                        foreach (Quote quote in list._embedded.quotes)
                        {
                            Console.ReadLine();

                            Console.WriteLine($"At {quote.appeared_at.Date.ToShortDateString()} he said: " + quote.value);
                        }
                        Console.WriteLine(" --------- And that is all he had to say about it!");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private static async Task PrintNewQuoteAsync(String tag)
        {
            await GetTheNewThing(tag);
        }
    }
}