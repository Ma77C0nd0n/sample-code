using DocumentProcessingService.app.Models;
using DocumentProcessingService.app.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentProcessingService.Test.Stores
{
    [TestFixture]
    public class LookupStoreTests
    {
        private Mock<DocumentContext> _contextMock;
        private Mock<ILogger<LookupStore>> _loggerMock;
        private LookupStore _sut;

        [SetUp]
        public void Setup()
        {
            var mockSet = new Mock<DbSet<DocumentItem>>();
            _contextMock = new Mock<DocumentContext>();
            _contextMock.Setup(c => c.DocumentItems).Returns(mockSet.Object);
            _loggerMock = new Mock<ILogger<LookupStore>>();
            _sut = new LookupStore(_contextMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task RecordAsync_ShouldPopulateDbWhenRequestIsValid()
        {
            // Act
            await _sut.RecordAsync("clientA", "doc1", new string[] { "word" });

            // Assert 
            _contextMock.Verify(x => x.DocumentItems.AddAsync(It.IsAny<DocumentItem>(), It.IsAny<CancellationToken>()), Times.Once);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
        
        [Test]
        public async Task RecordAsync_ShouldPopulateDbWhenRequestKeywordsAreEmpty()
        {
            // Act
            await _sut.RecordAsync("clientA", "doc1", new string[] {});

            // Assert 
            _contextMock.Verify(x => x.DocumentItems.AddAsync(It.IsAny<DocumentItem>(), It.IsAny<CancellationToken>()), Times.Once);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
        
        [Test]
        public async Task RecordAsync_ShouldNotPopulateDbWhenRequestKeywordsAreNull()
        {
            // Act
            await _sut.RecordAsync("clientA", "doc1", null);

            // Assert 
            _contextMock.Verify(x => x.DocumentItems.AddAsync(It.IsAny<DocumentItem>(), It.IsAny<CancellationToken>()), Times.Never);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
        
        [Test]
        public async Task RecordAsync_ShouldNotPopulateDbWhenRequestClientIsEmptyString()
        {
            // Act
            await _sut.RecordAsync("", "doc1", new string[] { "test" });

            // Assert 
            _contextMock.Verify(x => x.DocumentItems.AddAsync(It.IsAny<DocumentItem>(), It.IsAny<CancellationToken>()), Times.Never);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task RecordAsync_ShouldNotPopulateDbWhenRequestDocumentIdIsEmptyString()
        {
            // Act
            await _sut.RecordAsync("client", "", new string[] { "test" });

            // Assert 
            _contextMock.Verify(x => x.DocumentItems.AddAsync(It.IsAny<DocumentItem>(), It.IsAny<CancellationToken>()), Times.Never);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}