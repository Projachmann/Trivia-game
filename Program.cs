using System.Text.Json;

namespace Trivia
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string url = "https://api.api-ninjas.com/v1/trivia?category=";
            string apiKey = Environment.GetEnvironmentVariable("API_KEY");
            string filePath = "high_score.txt";
            bool gameOver = false;
            int highScore = 0;
            int points = 0;

            if (File.Exists(filePath))
            {
                highScore = Convert.ToInt32(File.ReadAllText(filePath));
            }

            Console.WriteLine("WELCOME TO THE TRIVIA GAME");
            Console.ReadLine();

            if (string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("No api key found! Get your api key from here: https://api-ninjas.com/profile");
                apiKey = Console.ReadLine();
            }

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
                            Console.WriteLine("Points: " + points);
                            Console.WriteLine();
                            Console.WriteLine($"CATEGORY: {question[0].category}");
                            Console.WriteLine($"QUESTION: {question[0].question}");
                            Console.Write($"ANSWER: ");
                            string answer = Console.ReadLine().ToLower();
                            if (answer == question[0].answer.ToLower() || answer == "yes")
                            {
                                Console.WriteLine("That was correct!");
                                Console.ReadLine();
                                Console.Clear();
                                points++;
                            }
                            else
                            {
                                Console.WriteLine("That was not correct!");
                                gameOver = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }

            if (points > highScore)
            {
                highScore = points;
                Console.WriteLine("You reached a new High score of " + highScore + "!");
            }
            else
            {
                Console.WriteLine("\nHIGH SCORE: " + highScore);
                Console.WriteLine();
                Console.WriteLine("CURRENT SCORE: " + points);
            }

            File.WriteAllText(filePath, Convert.ToString(highScore));
        }
    }

    public class Question()
    {
        public string category { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
    }
}
