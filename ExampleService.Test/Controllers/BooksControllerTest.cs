using Moq;
using ExampleService.Api.Controllers;
using ExampleService.Api.Models;
using ExampleService.Api.Services;

namespace ExampleService.Test.Controllers;

public class BooksControllerTest
{
    private readonly Mock<IBookService> _mockedBookService;

    public BooksControllerTest() =>
        _mockedBookService = new Mock<IBookService>();
        
    [Fact]
    public async Task Test1()
    {
        _mockedBookService.Setup(service => service.GetAsync()).ReturnsAsync(new List<Book>());
        var booksController = new BooksController(_mockedBookService.Object);

        var expected = new List<Book>
        {
            new Book
            {
                Id = "123456789012345678901234",
                BookName = "How to Design Programs",
                Price = 60,
                Category = "Computers",
                Author = "Unknown"
            }
        };
        var actual = await booksController.Get();
        Assert.Equal(expected, actual);
    }
}
