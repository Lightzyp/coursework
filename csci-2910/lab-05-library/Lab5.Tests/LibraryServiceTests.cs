using Xunit;
using Lab5.Services;
using Lab5.Models;
using Microsoft.visualstudio.TestTools.UnitTesting;

public class LibraryServiceTests
{
    [Fact]
    public void AddBook_ShouldIncreaseBookCount()
    {
        // Arrange
        var service = new LibraryService();

        // Act
        service.AddBook("Test Book");

        // Assert
        Assert.Single(service.ReadBooks());
    }

    [Fact]
    public void DeleteBook_ShouldRemoveBook()
    {
        // Arrange
        var service = new LibraryService();
        service.AddBook("Test Book");

        // Act
        service.DeleteBook(1);

        // Assert
        Assert.Empty(service.ReadBooks());
    }
}