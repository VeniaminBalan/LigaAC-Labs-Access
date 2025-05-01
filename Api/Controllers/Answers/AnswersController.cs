using Api.Models;
using Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Answers;

[Route("api/questions/{questionId}/answers")]
[ApiController]
public class AnswersController : ControllerBase
{
    private readonly ILogger<AnswersController> _logger;
    private readonly AnswersRepository _repository;

    public AnswersController(ILogger<AnswersController> logger, AnswersRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<List<AnswerResponse>>> GetAnswers(int questionId)
    {
        var answers = await _repository.GetAnswersByQuestionIdAsync(questionId);

        if (!answers.Any())
        {
            return NotFound();
        }

        return Ok(answers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AnswerResponse>> GetAnswer(int questionId, int id)
    {
        var answer = await _repository.GetAnswerByIdAsync(questionId, id);

        if (answer == null)
        {
            return NotFound();
        }

        return Ok(answer);
    }

    [HttpPost]
    public async Task<ActionResult<AnswerResponse>> CreateAnswer(int questionId, AnswerRequest request)
    {
        var createdAnswer = await _repository.CreateAnswerAsync(questionId, request);
        return CreatedAtAction(nameof(GetAnswer), new { questionId, id = createdAnswer.AnswerId }, createdAnswer);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAnswer(int questionId, int id, AnswerRequest request)
    {
        var success = await _repository.UpdateAnswerAsync(questionId, id, request);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAnswer(int questionId, int id)
    {
        var success = await _repository.DeleteAnswerAsync(questionId, id);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}
