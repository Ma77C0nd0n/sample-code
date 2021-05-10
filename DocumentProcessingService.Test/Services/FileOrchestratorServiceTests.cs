using DocumentProcessingService.app.Models;
using DocumentProcessingService.app.Queries;
using DocumentProcessingService.app.Repositories;
using DocumentProcessingService.app.Services;
using DocumentProcessingService.app.Stores;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocumentProcessingService.Test.Services
{
    [TestFixture]
    public class FileOrchestratorServiceTests
    {
        private Mock<IFileShareQuery> _fileShareQueryMock;
        private Mock<IFileProcessingService> _fileProcessingServiceMock;
        private Mock<ILookupStore> _lookupStoreMock;
        private Mock<IFileDeletionRepository> _fileDeletionRepositoryMock;
        private Mock<ILogger<FileOrchestratorService>> _loggerMock;
        private FileOrchestratorService _sut;

        [SetUp]
        public void Setup()
        {
            _fileShareQueryMock = new Mock<IFileShareQuery>();
            _loggerMock = new Mock<ILogger<FileOrchestratorService>>();
            _fileProcessingServiceMock = new Mock<IFileProcessingService>();
            _lookupStoreMock = new Mock<ILookupStore>();
            _fileDeletionRepositoryMock = new Mock<IFileDeletionRepository>();

            _sut = new FileOrchestratorService(
                _fileShareQueryMock.Object,
                _fileProcessingServiceMock.Object,
                _lookupStoreMock.Object,
                _fileDeletionRepositoryMock.Object,
                _loggerMock.Object);
        }

        [Test]
        public async Task DoWork_ShouldNotProcessWhenGetFileNamesReturnsInvalidFileName()
        {
            //Arrange
            _fileShareQueryMock.Setup(c => c.GetDirectories(It.IsAny<string>())).Returns(new string[] { "" });
            _fileShareQueryMock.Setup(c => c.GetFileNamesForNetworkLocationAsync(It.IsAny<string>())).ReturnsAsync(new string[] { "" });

            // Act
            await _sut.DoWork();

            // Assert 
            _fileShareQueryMock.Verify(x => x.GetDirectories(It.IsAny<string>()), Times.Once);
            _fileShareQueryMock.Verify(x => x.GetFileNamesForNetworkLocationAsync(It.IsAny<string>()), Times.Once);
            _fileProcessingServiceMock.Verify(x => x.ProcessFile(It.IsAny<string>()), Times.Never);
            _lookupStoreMock.Verify(x => x.Record(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()), Times.Never);
            _fileDeletionRepositoryMock.Verify(x => x.DeleteFileAsync(It.IsAny<string>()), Times.Never);
        }
        
        [Test]
        public async Task DoWork_ShouldNotCallRecordWhenProcessFileReturnsNull()
        {
            //Arrange
            _fileShareQueryMock.Setup(c => c.GetDirectories(It.IsAny<string>())).Returns(new string[] { "" });
            _fileShareQueryMock.Setup(c => c.GetFileNamesForNetworkLocationAsync(It.IsAny<string>())).ReturnsAsync(new string[] { "123_Test.text" });
            _fileProcessingServiceMock.Setup(c => c.ProcessFile(It.IsAny<string>())).ReturnsAsync(null as string[]);

            // Act
            await _sut.DoWork();

            // Assert 
            _fileShareQueryMock.Verify(x => x.GetDirectories(It.IsAny<string>()), Times.Once);
            _fileShareQueryMock.Verify(x => x.GetFileNamesForNetworkLocationAsync(It.IsAny<string>()), Times.Once);
            _fileProcessingServiceMock.Verify(x => x.ProcessFile(It.IsAny<string>()), Times.Once);
            _lookupStoreMock.Verify(x => x.Record(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()), Times.Never);
            _fileDeletionRepositoryMock.Verify(x => x.DeleteFileAsync(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task DoWork_ShouldRecordAndDeleteFileWhenProcessedSuccessfully()
        {
            //Arrange
            _fileShareQueryMock.Setup(c => c.GetDirectories(It.IsAny<string>())).Returns(new string[] { "" });
            _fileShareQueryMock.Setup(c => c.GetFileNamesForNetworkLocationAsync(It.IsAny<string>())).ReturnsAsync(new string[] { "123_Test.text" });
            _fileProcessingServiceMock.Setup(c => c.ProcessFile(It.IsAny<string>())).ReturnsAsync(new string[] { "" });

            // Act
            await _sut.DoWork();

            // Assert 
            _fileShareQueryMock.Verify(x => x.GetDirectories(It.IsAny<string>()), Times.Once);
            _fileShareQueryMock.Verify(x => x.GetFileNamesForNetworkLocationAsync(It.IsAny<string>()), Times.Once);
            _fileProcessingServiceMock.Verify(x => x.ProcessFile(It.IsAny<string>()), Times.Once);
            _lookupStoreMock.Verify(x => x.Record(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()), Times.Once);
            _fileDeletionRepositoryMock.Verify(x => x.DeleteFileAsync(It.IsAny<string>()), Times.Once);
        }
    }
}