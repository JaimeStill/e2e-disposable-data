namespace Brainstorm.Models.Entities;
public class Note : EntityBase
{
    public int TopicId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int Priority { get; set; }

    public Topic Topic { get; set; }

    public override void Settle() => Url = Encode(Title);
}