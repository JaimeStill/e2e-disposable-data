using System.Text;
using Brainstorm.Data;
using Brainstorm.Models.Entities;
using Brainstorm.Models.Query;
using Microsoft.EntityFrameworkCore;

namespace Brainstorm.Services;
public class NoteService : ServiceBase<Note>
{
    public NoteService(AppDbContext db) : base(db) { }

    protected override Func<IQueryable<Note>, string, IQueryable<Note>> Search =>
        (values, term) =>
            values.Where(x =>
                x.Title.ToLower().Contains(term.ToLower())
                || x.Body.ToLower().Contains(term.ToLower())
                || x.Priority.ToString() == term
            );

    public async Task<QueryResult<Note>> QueryByTopic(int topicId, QueryParams queryParams) =>
        await Query(
            set.Where(x => x.TopicId == topicId),
            queryParams, Search
        );

    public async Task<bool> ValidateTitle(Note note) =>
        !await set.AnyAsync(x =>
            x.Id != note.Id
            && x.TopicId == note.TopicId
            && x.Title.ToLower() == note.Title.ToLower()
        );

    public override async Task<bool> Validate(Note note)
    {
        var exception = new StringBuilder();

        if (note.TopicId < 0)
            exception.AppendLine("A Note must be defined within a Topic");
        
        if (string.IsNullOrWhiteSpace(note.Title))
            exception.AppendLine("A Note must have a Title");

        if (string.IsNullOrWhiteSpace(note.Body))
            exception.AppendLine("A Note must have a Body");

        if (!await ValidateTitle(note))
            exception.AppendLine($"{note.Title} is already a Note");

        if (exception.Length > 0)
            throw new Exception(exception.ToString());

        return true;
    }
}