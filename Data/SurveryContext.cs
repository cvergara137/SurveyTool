using Microsoft.EntityFrameworkCore;
using SurveryTool.Models;

namespace SurveryTool.Data
{
    public class SurveyContext : DbContext
    {
        public SurveyContext(DbContextOptions<SurveyContext> options) : base(options) { }

        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Response> Responses { get; set; }

        public DbSet<AnswerOption> AnswerOptions { get; set; }
    }
}
