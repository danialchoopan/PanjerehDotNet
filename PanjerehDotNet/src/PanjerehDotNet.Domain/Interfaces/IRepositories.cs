using PanjerehDotNet.Domain.Entities;
using System.Linq.Expressions;
namespace PanjerehDotNet.Domain.Interfaces;
public interface IRepository<T> where T : class {
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
}
public interface IUserRepository : IRepository<User> {
    Task<User?> GetByPhoneNumberAsync(string phoneNumber);
}
public interface IAdvertisementRepository : IRepository<Advertisement> {
    Task<IEnumerable<Advertisement>> GetPagedAsync(int page, int pageSize, int? categoryId, string? searchQuery);
}
public interface IChatRepository : IRepository<ChatMessage> {
    Task<IEnumerable<ChatMessage>> GetChatHistoryAsync(int user1Id, int user2Id, int adId);
    Task<IEnumerable<ChatMessage>> GetUserConversationsAsync(int userId);
}
public interface IUnitOfWork : IDisposable {
    IUserRepository Users { get; }
    IAdvertisementRepository Advertisements { get; }
    IRepository<Category> Categories { get; }
    IChatRepository Chats { get; }
    IRepository<AdReport> Reports { get; }
    Task<int> CompleteAsync();
}
