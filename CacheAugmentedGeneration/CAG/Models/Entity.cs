namespace CAG.Models
{
    public abstract class Entity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public abstract string GetCombinedText();
    }
}
