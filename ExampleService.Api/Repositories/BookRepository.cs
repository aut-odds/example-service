using MongoDB.Driver;
using ExampleService.Api.Models;

namespace ExampleService.Api.Repositories;

public class BookRepository : IBookRepository
{
    private readonly IMongoCollection<Book> _booksCollection;

    public BookRepository(IMongoDatabase mongoDatabase) =>
        _booksCollection = mongoDatabase.GetCollection<Book>("books");

    public async Task<List<Book>> GetAsync() =>
        await _booksCollection.Find(_ => true).ToListAsync();

    public async Task<Book> GetAsync(string id) =>
        await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Book newBook) =>
        await _booksCollection.InsertOneAsync(newBook);

    public async Task<ReplaceOneResult> UpdateAsync(Book updatedBook) =>
        await _booksCollection.ReplaceOneAsync(x => x.Id == updatedBook.Id, updatedBook);

    public async Task<DeleteResult> RemoveAsync(string id) =>
        await _booksCollection.DeleteOneAsync(x => x.Id == id);
}
