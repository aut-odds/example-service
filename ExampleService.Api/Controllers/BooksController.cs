using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using ExampleService.Api.Dtos;
using ExampleService.Api.Models;
using ExampleService.Api.Services;

namespace ExampleService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService) =>
        _bookService = bookService;

    [HttpGet]
    public async Task<List<Book>> Get() =>
        await _bookService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Book>> Get(string id)
    {
        var book = await _bookService.GetAsync(id);
        return book is null ? NotFound() : book;
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Post(BookDto bookDto)
    {
        var newBook = new Book
        {
            BookName = bookDto.BookName,
            Price = bookDto.Price,
            Category = bookDto.Category,
            Author = bookDto.Author
        };
        await _bookService.CreateAsync(newBook);
        return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
    }

    [HttpPut("{id:length(24)}")]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Put(string id, BookDto bookDto)
    {
        var updatedBook = new Book
        {
            Id = id,
            BookName = bookDto.BookName,
            Price = bookDto.Price,
            Category = bookDto.Category,
            Author = bookDto.Author
        };
        var success = await _bookService.UpdateAsync(updatedBook);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var success = await _bookService.RemoveAsync(id);
        return success ? NoContent() : NotFound();
    }
}
