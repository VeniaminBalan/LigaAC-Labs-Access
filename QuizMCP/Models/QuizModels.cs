using System;
using System.Collections.Generic;

namespace QuizMCP.Models
{
    public class PaginatedResponse<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }

    public class QuestionResponse
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = null!;
        public List<AnswerResponse> Answers { get; set; } = new();
    }

    public class AnswerResponse
    {
        public int AnswerId { get; set; }
        public string AnswerText { get; set; } = null!;
        public bool Correct { get; set; }
    }
}
