using DocumentProcessingService.app.Models;
using DocumentProcessingService.app.Queries;
using DocumentProcessingService.app.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentProcessingService.Test.Services
{
    [TestFixture]
    public class FileProcessingServiceTests
    {
        private Mock<IFileShareQuery> _fileShareQueryMock;
        private Mock<ILogger<FileProcessingService>> _loggerMock;
        private FileProcessingService _sut;

        [SetUp]
        public void Setup()
        {
            _fileShareQueryMock = new Mock<IFileShareQuery>();
            _loggerMock = new Mock<ILogger<FileProcessingService>>();
            _sut = new FileProcessingService(_fileShareQueryMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task ProcessFile_ShouldReturnNullWhenReadFileReturnsNull()
        {
            //Arrange
            _fileShareQueryMock.Setup(c => c.ReadFile(It.IsAny<string>())).ReturnsAsync(null as DocumentContent);

            // Act
            var res = await _sut.ProcessFile("fileA");

            // Assert 
            Assert.IsNull(res);
        }
        
        [Test]
        public async Task ProcessFile_ShouldReturnNullWhenProcessingTypeIsNotLookup()
        {
            //Arrange
            var doc = new DocumentContent { 
                ProcessingType = "nothing",
                Body = new string[] { "test" },
                Parameters = new Dictionary<string, bool> { { "test", false } }
            };
            _fileShareQueryMock.Setup(c => c.ReadFile(It.IsAny<string>())).ReturnsAsync(doc);

            // Act
            var res = await _sut.ProcessFile("test");

            // Assert 
            Assert.IsNull(res);
        }
        
        [Test]
        public async Task ProcessFile_ShouldReturnEmptyListWhenParametersIsEmpty()
        {
            //Arrange
            var doc = new DocumentContent { 
                ProcessingType = "lookup",
                Body = new string[] { "test" },
                Parameters = new Dictionary<string, bool> {  }
            };
            _fileShareQueryMock.Setup(c => c.ReadFile(It.IsAny<string>())).ReturnsAsync(doc);

            // Act
            var res = await _sut.ProcessFile("test");

            // Assert 
            Assert.IsNotNull(res);
            Assert.AreEqual(0, res.ToList().Count);
        }

        [Test]
        public async Task ProcessFile_ShouldReturnEmptyListWhenBodyIsEmpty()
        {
            //Arrange
            var doc = new DocumentContent { 
                ProcessingType = "lookup",
                Body = new string[] { },
                Parameters = new Dictionary<string, bool> { { "test", false } }
            };
            _fileShareQueryMock.Setup(c => c.ReadFile(It.IsAny<string>())).ReturnsAsync(doc);

            // Act
            var res = await _sut.ProcessFile("test");

            // Assert 
            Assert.IsNotNull(res);
            Assert.AreEqual(0, res.ToList().Count);
        }
        
        [Test]
        public async Task ProcessFile_ShouldReturnNotProcessedSubstrings()
        {
            //Arrange
            var doc = new DocumentContent { 
                ProcessingType = "lookup",
                Body = new string[] { "thisshouldnotwork" },
                Parameters = new Dictionary<string, bool> { { "not", false }, { "work", false } }
            };
            _fileShareQueryMock.Setup(c => c.ReadFile(It.IsAny<string>())).ReturnsAsync(doc);

            // Act
            var res = await _sut.ProcessFile("test");

            // Assert 
            Assert.IsNotNull(res);
            Assert.AreEqual(0, res.ToList().Count);
        }

        [Test]
        public async Task ProcessFile_ShouldReturnListOfProcessedParamsWhenValid()
        {
            //Arrange
            var doc = new DocumentContent { 
                ProcessingType = "lookup",
                Body = new string[] { "test all of this string" },
                Parameters = new Dictionary<string, bool> { { "test", false }, { "this", false }, { "cat", false } }
            };
            _fileShareQueryMock.Setup(c => c.ReadFile(It.IsAny<string>())).ReturnsAsync(doc);

            // Act
            var res = await _sut.ProcessFile("test");

            // Assert 
            Assert.IsNotNull(res);
            Assert.AreEqual(2, res.ToList().Count);
        }
    }
}