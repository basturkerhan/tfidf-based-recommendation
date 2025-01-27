using CAG.Models;

namespace API.DialogRecommendation.Models;
public class Dialog : Entity
{
    public string Question { get; set; } = null!;
    public string Answer { get; set; } = null!;

    public override string GetCombinedText()
    {
        return $"{Question} {Answer}";
    }
}
