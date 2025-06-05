using QuizClient.Models;
using QuizClient.Services;

// Create the API service
var quizApiService = new QuizApiService();

// Create the terminal service
var terminalService = new QuizTerminalService(quizApiService);

// Main menu loop
await RunMainMenu(terminalService);

// Method to run the main menu
static async Task RunMainMenu(QuizTerminalService terminalService)
{
    bool exit = false;
    
    while (!exit)
    {        Console.Clear();
        Console.WriteLine("=== Quiz Client Menu ===");
        Console.WriteLine("1. View All Questions");
        Console.WriteLine("2. Get Question by ID");
        Console.WriteLine("3. Create New Question");
        Console.WriteLine("4. Exit");
        Console.Write("\nSelect an option (1-4): ");
        
        string choice = Console.ReadLine()?.Trim() ?? "";
        
        switch (choice)
        {
            case "1":
                await terminalService.GetAndDisplayAllQuestions();
                Console.WriteLine("\nPress any key to return to the menu...");
                Console.ReadKey();
                break;
                
            case "2":
                await terminalService.GetAndDisplayQuestionById();
                Console.WriteLine("\nPress any key to return to the menu...");
                Console.ReadKey();
                break;
                
            case "3":
                await terminalService.CreateNewQuestion();
                Console.WriteLine("\nPress any key to return to the menu...");
                Console.ReadKey();
                break;
                
            case "4":
                exit = true;
                break;
                
            default:
                Console.WriteLine("Invalid option. Press any key to try again...");
                Console.ReadKey();
                break;
        }
    }
    
    Console.WriteLine("Thank you for using the Quiz Client. Goodbye!");
}
