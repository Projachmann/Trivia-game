using System.Text.Json;

namespace Trivia
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string url = "https://api.api-ninjas.com/v1/trivia?category=";
            string apiKey = Environment.GetEnvironmentVariable("API_KEY");
            bool gameOver = false;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
                while (!gameOver)
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode();
                        string json = await response.Content.ReadAsStringAsync();
                        Question[] question = JsonSerializer.Deserialize<Question[]>(json);

                        if (question != null)
                        {
                            Console.WriteLine($"CATEGORY: {question[0].category}");
                            Console.WriteLine($"QUESTION: {question[0].question}");
                            Console.Write($"ANSWER: ");
                            string answer = Console.ReadLine().ToLower();
                            if (answer != question[0].answer.ToLower())
                            {
                                Console.WriteLine("That was not correct!");
                                gameOver = true;
                            }
                            else
                            {
                                Console.WriteLine("That was correct!");
                                Console.ReadLine();
                                Console.Clear();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }
    }

    public class Question()
    {
        public string category { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
    }
}
