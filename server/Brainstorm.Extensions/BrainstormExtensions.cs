namespace Brainstorm.Extensions;
public static class BrainstormExtensions
{
    public static IQueryable<T> SetupSearch<T>(
        this IQueryable<T> values,
        string search,
        Func<IQueryable<T>, string, IQueryable<T>> action,
        char split = '|'
    )
    {
        if (search.Contains(split))
        {
            var searches = search.Split(split);

            foreach (var s in searches)
            {
                values = action(values, s.Trim());
            }

            return values;
        }
        else
            return action(values, search);
    }
}
