using Microsoft.EntityFrameworkCore;
using PanjerehDotNet.Domain.Entities;
using PanjerehDotNet.Domain.Interfaces;
using PanjerehDotNet.Infrastructure.Persistence;
using System.Linq.Expressions;
namespace PanjerehDotNet.Infrastructure.Repositories;
public class Repository<T> : IRepository<T> where T : class {
    protected readonly ApplicationDbContext _context;
    public Repository(ApplicationDbContext context) { _context = context; }
    public async Task<T?> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);
    public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) => await _context.Set<T>().Where(predicate).ToListAsync();
    public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);
    public void Update(T entity) => _context.Set<T>().Update(entity);
    public void Remove(T entity) => _context.Set<T>().Remove(entity);
}
public class UserRepository : Repository<User>, IUserRepository {
    public UserRepository(ApplicationDbContext context) : base(context) { }
    public async Task<User?> GetByPhoneNumberAsync(string phoneNumber) => await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
}
public class AdvertisementRepository : Repository<Advertisement>, IAdvertisementRepository {
    public AdvertisementRepository(ApplicationDbContext context) : base(context) { }
    public async Task<IEnumerable<Advertisement>> GetPagedAsync(int page, int pageSize, int? categoryId, string? searchQuery) {
        var query = _context.Advertisements.Include(a => a.Images).Include(a => a.Category).AsQueryable();
        if (categoryId.HasValue) query = query.Where(a => a.CategoryId == categoryId.Value);
        if (!string.IsNullOrEmpty(searchQuery)) query = query.Where(a => a.Title.Contains(searchQuery) || a.Description.Contains(searchQuery));
        return await query.OrderByDescending(a => a.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
    }
}
public class ChatRepository : Repository<ChatMessage>, IChatRepository {
    public ChatRepository(ApplicationDbContext context) : base(context) { }
    public async Task<IEnumerable<ChatMessage>> GetChatHistoryAsync(int user1Id, int user2Id, int adId) {
        return await _context.ChatMessages.Where(m => m.AdvertisementId == adId && ((m.SenderId == user1Id && m.ReceiverId == user2Id) || (m.SenderId == user2Id && m.ReceiverId == user1Id))).OrderBy(m => m.SentAt).ToListAsync();
    }
    public async Task<IEnumerable<ChatMessage>> GetUserConversationsAsync(int userId) {
        return await _context.ChatMessages.Where(m => m.SenderId == userId || m.ReceiverId == userId).OrderByDescending(m => m.SentAt).ToListAsync();
    }
}
public class UnitOfWork : IUnitOfWork {
    private readonly ApplicationDbContext _context;
    public UnitOfWork(ApplicationDbContext context) {
        _context = context;
        Users = new UserRepository(_context);
        Advertisements = new AdvertisementRepository(_context);
        Categories = new Repository<Category>(_context);
        Chats = new ChatRepository(_context);
    }
    public IUserRepository Users { get; private set; }
    public IAdvertisementRepository Advertisements { get; private set; }
    public IRepository<Category> Categories { get; private set; }
    public IChatRepository Chats { get; private set; }
    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
    public void Dispose() => _context.Dispose();
}
