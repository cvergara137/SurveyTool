using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveryTool.Data;
using SurveryTool.Models;

namespace SurveryTool.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SurveysController : ControllerBase
    {
        private readonly SurveyContext _surveyContext;

        public SurveysController(SurveyContext surveyContext)
        {
            _surveyContext = surveyContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Survey>>> GetSurveys()
        {
            return await _surveyContext.Surveys.Include(x => x.Questions).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Survey>>> GetSurvey(int id)
        {
            Survey? survey = await _surveyContext.Surveys
                .Include(x => x.Questions)
                .ThenInclude(y => y.Responses)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (survey == null) 
                return NotFound();

            return Ok(survey);
        }

        [HttpPost]
        public async Task<ActionResult<Survey>> CreateSurvey(Survey survey)
        {
            _surveyContext.Surveys.Add(survey);
            await _surveyContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSurvey), new { id = survey.Id }, survey);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSurvey(int id, Survey updatedSurvey)
        {
            if (id != updatedSurvey.Id)
                return BadRequest($"Survey IDs mismatch");

            Survey? survey = await _surveyContext.Surveys
                .Include(x => x.Questions)
                .FirstOrDefaultAsync(y => y.Id == id);

            if (survey == null) 
                return NotFound($"Survey with ID {id} not found.");

            survey.Title = updatedSurvey.Title;
            survey.Description = updatedSurvey.Description;

            if (updatedSurvey.Questions != null && updatedSurvey.Questions.Any())
            {
                _surveyContext.Questions.RemoveRange(survey.Questions);

                survey.Questions = updatedSurvey.Questions;
            }

            await _surveyContext.SaveChangesAsync();

            return Ok(survey);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Survey>>DeleteSurvey(int id)
        {
            Survey? survey = await _surveyContext.Surveys
                .Include(x => x.Questions)
                .ThenInclude(y => y.Responses)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (survey == null)
                return NotFound($"Survey with ID: {id} not found.");

            foreach (Question question in survey.Questions)
                _surveyContext.Responses.RemoveRange(question.Responses);

            _surveyContext.Questions.RemoveRange(survey.Questions);
            _surveyContext.Remove(survey);

            await _surveyContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}/score")]
        public async Task<IActionResult> GetSurveyScore(int id)
        {
            Survey? survey = await _surveyContext.Surveys
                .Include(x => x.Questions)
                .ThenInclude(y => y.Responses)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (survey == null) 
                return NotFound();

            var numericResponses = survey.Questions
                .SelectMany(x => x.Responses)
                .Where(y => double.TryParse(y.Answer, out _))
                .Select(y => double.Parse(y.Answer))
                .ToList();

            if (!numericResponses.Any())
                return Ok(new { totalScore = 0, message = "No Numeric responses found" });

            double totalScore = numericResponses.Sum();

            return Ok(new { totalScore });
        }
    }
}
