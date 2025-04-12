using Api.Models;
using Api.Services;

var questionService = new QuestionService();
int correctAnswers = 0;
int wrongAnswers = 0;

try
{
    var questions = await questionService.GetQuestionsAsync();

    foreach (var question in questions)
    {
        Console.WriteLine($"Question: {question.QuestionText}");
        for (int i = 0; i < question.Answers.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {question.Answers[i].AnswerText}");
        }

        Console.Write("Choose the correct answer (enter the number): ");
        if (int.TryParse(Console.ReadLine(), out int choice) &&
            choice > 0 && choice <= question.Answers.Count)
        {
            var selectedAnswer = question.Answers[choice - 1];
            if (selectedAnswer.Correct)
            {
                Console.WriteLine("Correct!");
                correctAnswers++;
            }
            else
            {
                Console.WriteLine("Wrong answer.");
                wrongAnswers++;
            }
        }
        else
        {
            Console.WriteLine("Invalid choice.");
        }

        Console.WriteLine();
    }

    Console.WriteLine($"You answered {correctAnswers} question(s) correctly and {wrongAnswers} question(s) incorrectly.");
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}
