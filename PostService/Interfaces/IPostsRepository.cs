using PostService.Models;

namespace PostService.Interfaces
{
    public interface IPostsRepository
    {
        Task<IEnumerable<Post>> All(Guid userId);
        Task Add(PostInputModel entity);
        Task Remove(PostInputModel posts);
    }
}
