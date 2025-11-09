using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveryTool.Models
{
    public class AnswerOption
    {
        public int Id { get; set; }
        public required string Text { get; set; }

        [ForeignKey(nameof(Question))]
        public int QuestionId { get; set; }
    }
}
