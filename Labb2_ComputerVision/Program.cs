using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Extensions.Configuration;


namespace Labb2_ComputerVision
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            bool input = false; 

            while (input = true)
            {
                try
                {
                    IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                    IConfigurationRoot configuration = builder.Build();
                    string endpoint = configuration["Endpoint"];
                    string subscriptionKey = configuration["SubscriptionKey"];

                    ComputerVisionClient client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(subscriptionKey))
                    {
                        Endpoint = endpoint
                    };

                    Console.Write("Enter the URL of the image you want to analyze: ");
                    string imageUrl = Console.ReadLine();
                    Console.Clear();

                    // Analyze the image
                    ImageAnalysis analysis = await client.AnalyzeImageAsync(imageUrl, new List<VisualFeatureTypes?>
            {
                VisualFeatureTypes.Description,
                VisualFeatureTypes.Categories,
                VisualFeatureTypes.Tags,
                VisualFeatureTypes.Faces,
                VisualFeatureTypes.Objects,
                VisualFeatureTypes.Brands,
                VisualFeatureTypes.Adult,
                VisualFeatureTypes.Color
            });

                    Console.WriteLine("Tags: \n ");
                    foreach (var tag in analysis.Tags)
                    {
                        Console.WriteLine($"{tag.Name}, {tag.Confidence * 100:F1}%");
                    }

                    Console.WriteLine("\n");
                    // Process the results
                    Console.WriteLine("Description: " + analysis.Description.Captions[0].Text);
                    Console.WriteLine("Categories: " + string.Join(", ", analysis.Categories.Select(c => c.Name)));
                    //Console.WriteLine("Tags: " + string.Join(", ", analysis.Tags.Select(t => t.Name)));
                    Console.WriteLine("Adult: " + analysis.Adult.IsAdultContent);
                    Console.WriteLine("Number of faces: " + analysis.Faces.Count);
                    Console.WriteLine("Objects: " + string.Join(", ", analysis.Objects.Select(o => o.ObjectProperty)));
                    Console.WriteLine("Brands: " + string.Join(", ", analysis.Brands.Select(b => b.Name)));
                    Console.WriteLine("Is adult content: " + analysis.Adult.IsAdultContent);
                    Console.WriteLine("Dominant colors: " + string.Join(", ", analysis.Color.DominantColors));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }

                Console.WriteLine("");
                Console.WriteLine("Do you want to check another url image address? please enter Y/N");
                string checkUrl = Console.ReadLine().ToLower();
                if (checkUrl == "y") 
                { input = true; Console.Clear(); }
                else { input = false; break; }
            }
        }
    }
}