using Brainstorm.Models.Url;

namespace Brainstorm.Models;
public abstract class EntityBase
{
    public int Id { get; set; }
    public string Url { get; set; }

    protected static string Encode(string prop) => UrlEncoder.Encode(prop);

    public virtual void Settle() => Url = string.Empty;
}