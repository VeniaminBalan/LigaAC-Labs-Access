using Api.Controllers;
using Api.Controllers.Answers;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;

public class AnswersRepository
{
    private readonly QuizContext _context;

    public AnswersRepository(QuizContext context)
    {
        _context = context;
    }

    public async Task<AnswerResponse?> GetAnswerByIdAsync(int questionId, int id)
    {
        return await _context.Answers
            .Where(a => a.Id == id && a.QuestionId == questionId)
            .Select(a => new AnswerResponse
            {
                AnswerId = a.Id,
                AnswerText = a.AnswerText,
                Correct = a.Correct
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<AnswerResponse>> GetAnswersByQuestionIdAsync(int questionId)
    {
        return await _context.Answers
            .Where(a => a.QuestionId == questionId)
            .Select(a => new AnswerResponse
            {
                AnswerId = a.Id,
                AnswerText = a.AnswerText,
                Correct = a.Correct
            })
            .ToListAsync();
    }

    public async Task<AnswerResponse> CreateAnswerAsync(int questionId, AnswerRequest request)
    {
        var answer = new Answer
        {
            QuestionId = questionId,
            AnswerText = request.AnswerText,
            Correct = request.Correct
        };

        _context.Answers.Add(answer);
        await _context.SaveChangesAsync();

        return new AnswerResponse
        {
            AnswerId = answer.Id,
            AnswerText = answer.AnswerText,
            Correct = answer.Correct
        };
    }

    public async Task<bool> UpdateAnswerAsync(int questionId, int id, AnswerRequest request)
    {
        var answer = await _context.Answers.FirstOrDefaultAsync(a => a.Id == id && a.QuestionId == questionId);

        if (answer == null)
        {
            return false;
        }

        answer.AnswerText = request.AnswerText;
        answer.Correct = request.Correct;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAnswerAsync(int questionId, int id)
    {
        var answer = await _context.Answers.FirstOrDefaultAsync(a => a.Id == id && a.QuestionId == questionId);

        if (answer == null)
        {
            return false;
        }

        _context.Answers.Remove(answer);
        await _context.SaveChangesAsync();
        return true;
    }
}

