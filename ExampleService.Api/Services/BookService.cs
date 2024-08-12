using ExampleService.Api.Models;
using ExampleService.Api.Repositories;

namespace ExampleService.Api.Services;

public interface IBookService
{
    public Task<List<Book>> GetAsync();

    public Task<Book> GetAsync(string id);

    public Task CreateAsync(Book newBook);

    public Task<Boolean> UpdateAsync(Book updatedBook);

    public Task<Boolean> RemoveAsync(string id);
}

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository) =>
        _bookRepository = bookRepository;

    public async Task<List<Book>> GetAsync() =>
        await _bookRepository.GetAsync();

    public async Task<Book> GetAsync(string id) =>
        await _bookRepository.GetAsync(id);

    public async Task CreateAsync(Book newBook) =>
        await _bookRepository.CreateAsync(newBook);

    public async Task<Boolean> UpdateAsync(Book updatedBook)
    {
        var result = await _bookRepository.UpdateAsync(updatedBook);
        return result.MatchedCount == 1;
    }

    public async Task<Boolean> RemoveAsync(string id)
    {
        var result = await _bookRepository.RemoveAsync(id);
        return result.DeletedCount == 1;
    }
}
