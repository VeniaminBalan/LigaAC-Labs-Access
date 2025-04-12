using Api.Controllers;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace Api.Repositories;

public class QuestionsRepository
{
    private readonly QuizContext _context;

    public QuestionsRepository(QuizContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResponse<QuestionResponse>> GetQuestionsAsync(int page, int pageSize)
    {
        var totalItems = await _context.Questions.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var questions = await _context.Questions
            .Include(q => q.Answers)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(q => new QuestionResponse
            {
                QuestionId = q.Id,
                QuestionText = q.QuestionText,
                Answers = q.Answers.Select(a => new AnswerResponse
                {
                    AnswerId = a.Id,
                    AnswerText = a.AnswerText,
                    Correct = a.Correct
                }).ToList()
            })
            .ToListAsync();

        return new PaginatedResponse<QuestionResponse>
        {
            Items = questions,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages
        };
    }

    public async Task<QuestionResponse?> GetQuestionByIdAsync(int id)
    {
        return await _context.Questions
            .Include(q => q.Answers)
            .Where(q => q.Id == id)
            .Select(q => new QuestionResponse
            {
                QuestionId = q.Id,
                QuestionText = q.QuestionText,
                Answers = q.Answers.Select(a => new AnswerResponse
                {
                    AnswerId = a.Id,
                    AnswerText = a.AnswerText,
                    Correct = a.Correct
                }).ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<QuestionResponse> CreateQuestionAsync(QuestionRequest request)
    {
        var question = new Question
        {
            QuestionText = request.QuestionText,
            Answers = request.Answers.Select(a => new Answer
            {
                AnswerText = a.AnswerText,
                Correct = a.Correct,
            }).ToList()
        };

        _context.Questions.Add(question);
        await _context.SaveChangesAsync();

        return new QuestionResponse
        {
            QuestionId = question.Id,
            QuestionText = question.QuestionText,
            Answers = question.Answers.Select(a => new AnswerResponse
            {
                AnswerId = a.Id,
                AnswerText = a.AnswerText,
                Correct = a.Correct
            }).ToList()
        };
    }

    public async Task<bool> UpdateQuestionAsync(int id, QuestionRequest request)
    {
        var question = await _context.Questions.Include(q => q.Answers).FirstOrDefaultAsync(q => q.Id == id);

        if (question == null)
        {
            return false;
        }

        question.QuestionText = request.QuestionText;
        question.Answers.Clear();
        question.Answers.AddRange(request.Answers.Select(a => new Answer
        {
            AnswerText = a.AnswerText,
            Correct = a.Correct
        }));

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteQuestionAsync(int id)
    {
        var question = await _context.Questions.Include(q => q.Answers).FirstOrDefaultAsync(q => q.Id == id);

        if (question == null)
        {
            return false;
        }

        _context.Questions.Remove(question);
        await _context.SaveChangesAsync();
        return true;
    }
}
