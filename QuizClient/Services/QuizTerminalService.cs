using QuizClient.Models;
using QuizClient.Services;

namespace QuizClient.Services;

public class QuizTerminalService
{
    private readonly QuizApiService _apiService;

    public QuizTerminalService(QuizApiService apiService)
    {
        _apiService = apiService;
    }

    /// <summary>
    /// Retrieves and displays all questions from the API
    /// </summary>
    public async Task GetAndDisplayAllQuestions()
    {
        Console.WriteLine("Retrieving all questions from API...");
        
        // Call the method to get all questions
        var questions = await _apiService.GetAllQuestionsAsync();
        
        if (questions.Count == 0)
        {
            Console.WriteLine("No questions found or error occurred.");
            return;
        }
        
        Console.WriteLine($"Successfully retrieved {questions.Count} questions:");
        Console.WriteLine("---------------------------------------------");
        
        foreach (var question in questions)
        {
            Console.WriteLine($"Question ID: {question.QuestionId}");
            Console.WriteLine($"Question: {question.QuestionText}");
            Console.WriteLine("Answers:");
            
            foreach (var answer in question.Answers)
            {
                string correctIndicator = answer.Correct ? " (Correct)" : "";
                Console.WriteLine($"  - {answer.AnswerText}{correctIndicator}");
            }
            
            Console.WriteLine("---------------------------------------------");
        }
    }

    /// <summary>
    /// Interactively creates a new question with answers and sends it to the API
    /// </summary>
    public async Task CreateNewQuestion()
    {
        Console.Clear();
        Console.WriteLine("=== Create a New Question ===\n");
        
        // Get question text
        Console.Write("Enter the question text: ");
        string questionText = Console.ReadLine() ?? string.Empty;
        
        if (string.IsNullOrWhiteSpace(questionText))
        {
            Console.WriteLine("Question text cannot be empty. Operation cancelled.");
            return;
        }
        
        // Create the question request
        var questionRequest = new QuestionRequest
        {
            QuestionText = questionText,
            Answers = new List<AnswerRequest>()
        };
        
        // Get the correct answer
        Console.WriteLine("\nLet's add the correct answer first:");
        Console.Write("Enter the correct answer: ");
        string correctAnswer = Console.ReadLine() ?? string.Empty;
        
        if (string.IsNullOrWhiteSpace(correctAnswer))
        {
            Console.WriteLine("Correct answer cannot be empty. Operation cancelled.");
            return;
        }
        
        questionRequest.Answers.Add(new AnswerRequest 
        { 
            AnswerText = correctAnswer, 
            Correct = true 
        });
        
        // Get three incorrect answers
        for (int i = 1; i <= 3; i++)
        {
            Console.WriteLine($"\nNow, let's add incorrect answer #{i}:");
            Console.Write($"Enter incorrect answer #{i}: ");
            string incorrectAnswer = Console.ReadLine() ?? string.Empty;
            
            if (string.IsNullOrWhiteSpace(incorrectAnswer))
            {
                Console.WriteLine($"Incorrect answer #{i} cannot be empty. Skipping this answer.");
                continue;
            }
            
            questionRequest.Answers.Add(new AnswerRequest 
            { 
                AnswerText = incorrectAnswer, 
                Correct = false 
            });
        }
        
        // Confirm the submission
        Console.WriteLine("\nReview your question before submitting:");
        Console.WriteLine($"Question: {questionRequest.QuestionText}");
        Console.WriteLine("Answers:");
        foreach (var answer in questionRequest.Answers)
        {
            Console.WriteLine($"  - {answer.AnswerText} {(answer.Correct ? "(Correct)" : "")}");
        }
        
        Console.Write("\nDo you want to submit this question? (Y/N): ");
        string confirmation = Console.ReadLine()?.Trim().ToUpper() ?? "N";
        
        if (confirmation != "Y")
        {
            Console.WriteLine("Operation cancelled by user.");
            return;
        }
        
        // Submit the question
        Console.WriteLine("\nSubmitting question to API...");
        var createdQuestion = await _apiService.CreateQuestionAsync(questionRequest);
        
        if (createdQuestion != null)
        {
            Console.WriteLine($"Question successfully created with ID: {createdQuestion.QuestionId}");
        }
        else
        {
            Console.WriteLine("Failed to create question. Please try again later.");
        }
    }

    /// <summary>
    /// Retrieves and displays a question by its ID
    /// </summary>
    public async Task GetAndDisplayQuestionById()
    {
        Console.Clear();
        Console.WriteLine("=== Get Question by ID ===\n");
        
        // Get question ID from user
        Console.Write("Enter the question ID: ");
        if (!int.TryParse(Console.ReadLine(), out int questionId) || questionId <= 0)
        {
            Console.WriteLine("Invalid ID. Please enter a positive integer.");
            return;
        }
        
        Console.WriteLine($"Retrieving question with ID {questionId}...");
        
        // Call the API to get the question
        var question = await _apiService.GetQuestionByIdAsync(questionId);
        
        if (question == null)
        {
            Console.WriteLine($"Question with ID {questionId} was not found.");
            return;
        }
        
        // Display the question
        Console.WriteLine("\n---------------------------------------------");
        Console.WriteLine($"Question ID: {question.QuestionId}");
        Console.WriteLine($"Question: {question.QuestionText}");
        Console.WriteLine("Answers:");
        
        foreach (var answer in question.Answers)
        {
            string correctIndicator = answer.Correct ? " (Correct)" : "";
            Console.WriteLine($"  - {answer.AnswerText}{correctIndicator}");
        }
        
        Console.WriteLine("---------------------------------------------");
    }
}
