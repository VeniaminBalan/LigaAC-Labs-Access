using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;

namespace FunctionCalls;

public class QuizPlugin
{
    public List<QuizQuestion> Questions { get; set; } = [];


    [KernelFunction]
    public string AddQuizQuestion(string question, string answer) 
    {
        Console.WriteLine($"Log: Adding the question: {question}");
        var questionQuestion = new QuizQuestion
        {
            Question = question,
            Answer = answer
        };


        Questions.Add(questionQuestion);
        return $"Question '{question}' added successfully";
    }

    //[KernelFunction]
    public string AddQuizQuestion2(string question, string answer)
    {
        Console.WriteLine($"Log: Adding the question: {question} to second methd");
        var questionQuestion = new QuizQuestion
        {
            Question = question,
            Answer = answer
        };


        Questions.Add(questionQuestion);
        return $"Question '{question}' added successfully";
    }

    [KernelFunction]
    [Description("Removes a question from the quiz by quiz nuber, number should be between 0 and number of question -1")]
    public string RemoveQuizQuestion(int questionNumber) 
    {
      
        if (questionNumber < 0 || questionNumber >= Questions.Count)
        {
            return $"Question number '{questionNumber}' is out of range";
        }

        var question = Questions[questionNumber];
        Questions.RemoveAt(questionNumber);
        Console.WriteLine($"Log: removing the question: {question}");
        return $"Question '{question.Question}' removed successfully";

    }

    [KernelFunction]
    public List<QuizQuestion> GetQuestions() 
    {
        Console.WriteLine($"Log: Retrieving all questions");
        return Questions ;
    }


}

public class QuizQuestion 
{
    public string Question { get; set; }
    public string Answer { get; set; }
}
