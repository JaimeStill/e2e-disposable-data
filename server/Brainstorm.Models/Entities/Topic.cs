namespace Brainstorm.Models.Entities;
public class Topic : EntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<Note> Notes { get; set; }

    public override void Settle() => Url = Encode(Name);
}