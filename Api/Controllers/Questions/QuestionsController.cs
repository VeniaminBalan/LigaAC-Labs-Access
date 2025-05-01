using Api.Models;
using Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Questions;

[Route("api/[controller]")]
[ApiController]
public class QuestionsController : ControllerBase
{
    private readonly ILogger<QuestionsController> _logger;
    private readonly QuestionsRepository _repository;

    public QuestionsController(ILogger<QuestionsController> logger, QuestionsRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<QuestionResponse>>> GetQuestions([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page <= 0 || pageSize <= 0)
        {
            return BadRequest("Page and pageSize must be greater than zero.");
        }

        var paginatedResponse = await _repository.GetQuestionsAsync(page, pageSize);
        return Ok(paginatedResponse);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<QuestionResponse>> GetQuestion(int id)
    {
        var question = await _repository.GetQuestionByIdAsync(id);

        if (question == null)
        {
            return NotFound();
        }

        return Ok(question);
    }

    [HttpGet("random")]
    public async Task<ActionResult<List<QuestionResponse>>> GetRandomQuestions([FromQuery] int count = 10)
    {
        if (count <= 0)
        {
            return BadRequest("Count must be greater than zero.");
        }

        var randomQuestions = await _repository.GetRandomQuestionsAsync(count);
        return Ok(randomQuestions);
    }

    [HttpPost]
    public async Task<ActionResult<QuestionResponse>> CreateQuestion(QuestionRequest request)
    {
        var createdQuestion = await _repository.CreateQuestionAsync(request);
        return CreatedAtAction(nameof(GetQuestion), new { id = createdQuestion.QuestionId }, createdQuestion);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuestion(int id, QuestionRequest request)
    {
        var success = await _repository.UpdateQuestionAsync(id, request);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuestion(int id)
    {
        var success = await _repository.DeleteQuestionAsync(id);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}
