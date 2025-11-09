using Microsoft.AspNetCore.Mvc.ModelBinding;
using SurveryTool.Controllers;
using SurveryTool.Models;
using System.Text.Json.Serialization;

namespace SurveryTool.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public QuestionType Type { get; set; }
        public int SurveyId { get; set; }
        public List<AnswerOption> Options { get; set; } = [];
        public List<Response> Responses { get; set; } = [];
    }

    public enum QuestionType
    {
        MultipleChoice,
        FreeText,
        Numeric,
        SingleChoice
    }
}
