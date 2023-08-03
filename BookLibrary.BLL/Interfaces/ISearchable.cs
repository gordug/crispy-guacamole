namespace BookLibrary.BLL.Interfaces;

public interface ISearchable<T>
{
    public Task<IEnumerable<T?>?> SearchAsync(string query);
}
