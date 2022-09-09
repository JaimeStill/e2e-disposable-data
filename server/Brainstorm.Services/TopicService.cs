using System.Text;
using Brainstorm.Data;
using Brainstorm.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brainstorm.Services;
public class TopicService : ServiceBase<Topic>
{
    public TopicService(AppDbContext db) : base(db) { }

    protected override Func<IQueryable<Topic>, string, IQueryable<Topic>> Search =>
        (values, term) =>
            values.Where(x =>
                x.Name.ToLower().Contains(term.ToLower())
                || x.Description.ToLower().Contains(term.ToLower())
                || x.Notes.Any(n => n.Title.ToLower().Contains(term.ToLower()))
            );

    protected override IQueryable<Topic> SetGraph(DbSet<Topic> data) =>
        data.Include(x => x.Notes);

    public async Task<bool> ValidateName(Topic topic) =>
        !await set.AnyAsync(x =>
            x.Id != topic.Id
            && x.Name.ToLower() == topic.Name.ToLower()
        );

    public override async Task<bool> Validate(Topic topic)
    {
        var exception = new StringBuilder();

        if (string.IsNullOrWhiteSpace(topic.Name))
            exception.AppendLine("A Topic must have a Name");

        if (!await ValidateName(topic))
            exception.AppendLine($"{topic.Name} is already a Topic");

        return true;            
    }
}