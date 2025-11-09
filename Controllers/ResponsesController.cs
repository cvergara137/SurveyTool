using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveryTool.Data;
using SurveryTool.Models;

namespace SurveryTool.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResponsesController : Controller
    {
        private readonly SurveyContext _surveyContext;

        public ResponsesController(SurveyContext surveyContext)
        {
            _surveyContext = surveyContext;
        }

        [HttpPost]
        public async Task<ActionResult<Response>> SubmitResponse(Response response)
        {
            Question? question = await _surveyContext.Questions
                .Include(x => x.Options)
                .SingleOrDefaultAsync(x => x.Id == response.QuestionId);

            if (question == null) 
                return NotFound($"Question with ID: {response.QuestionId} was not found.");

            string? validationError = ValidateAnswer(question, response.Answer);

            if (validationError != null) 
                return BadRequest(validationError);

            _surveyContext.Responses.Add(response);
            await _surveyContext.SaveChangesAsync();

            return CreatedAtAction(nameof(SubmitResponse), new { id = response.Id }, response);
        }

        private string? ValidateAnswer(Question question, string answer) 
        {
            switch (question.Type)
            {
                case QuestionType.SingleChoice:
                    if (question.Options.Count() != 1)
                        return "Question options are not configured properly.";
                    if (!question.Options.Select(x => x.Text).Contains(answer))
                        return $"Answer must be the option: { question.Options.FirstOrDefault()?.Text }";
                    break;
                case QuestionType.MultipleChoice:
                    if (question.Options.Count() <= 1)
                        return "Question options are not configured properly.";
                    if (!question.Options.Select(x => x.Text).Contains(answer))
                        return $"Answer must be one of the following options: {string.Join(", ", question.Options.Select(x => x.Text))}";
                    break;
                case QuestionType.FreeText:
                    if (string.IsNullOrWhiteSpace(answer))
                        return "Answer cannot be empty for a text question";
                    break;
                case QuestionType.Numeric:
                    if (!int.TryParse(answer, out _))
                        return "Answer must be a valid number";
                    break;

                default:
                    return $"Unsupported question type: {question.Type}";
            }

            return null;
        }
    }
}
