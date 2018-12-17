using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Text;

namespace MoqTutorial.Tests
{
    [TestClass]
    public class ExportServiceTest
    {
        Mock<IFileSystem> fileSystemMock;
        Mock<ISettings> settingsMock;

        [TestInitialize]
        public void Setup()
        {
            fileSystemMock = new Mock<IFileSystem>();
            settingsMock = new Mock<ISettings>();
        }

        private ExportService CreateExportService()
        {
            return new ExportService(fileSystemMock.Object, settingsMock.Object);
        }

        [TestMethod]
        public void ExportService_Export_HappyPath_NewFile()
        {
            // Arrange 
            
            var memoryStream = new MemoryStream();
            var stream = new StreamWriter(memoryStream);
            fileSystemMock
                .Setup(x => x.CreateFile(It.IsAny<string>()))
                .Returns(stream);

            settingsMock
                .SetupGet(m => m.ExportPath)
                .Returns("C:\\");


            // Act
            var exportServiceTestee = CreateExportService();
            var result = exportServiceTestee.Export(new[] { "something" }, "file.txt");

            // Assert
            result.Result.Should().BeTrue();
            result.FullPath.Should().Be("C:\\file.txt");
            result.Error.Should().BeNull();

            // Get the memoryStream data as an Array and convert it to string.
            var fileResult = Encoding.UTF8.GetString(memoryStream.ToArray());
            fileResult.Should().Be("something\r\n");
        }

        [TestMethod]
        public void ExportService_Export_HappyPath_ExistingFileShouldDeleteOld()
        {
            // Arrange 
            fileSystemMock
                .Setup(x => x.FileExist(It.IsAny<string>()))
                .Returns(true);

            var memoryStream = new MemoryStream();
            fileSystemMock
                .Setup(x => x.CreateFile(It.IsAny<string>()))
                .Returns(new StreamWriter(memoryStream));

            settingsMock
                .SetupGet(m => m.ExportPath)
                .Returns("C:\\");

            // Act
            var exportServiceTestee = CreateExportService();
            var result = exportServiceTestee.Export(new[] { "something" }, "file.txt");

            // Assert
            result.Result.Should().BeTrue();
            result.FullPath.Should().Be("C:\\file.txt");
            result.Error.Should().BeNull();

            fileSystemMock.Verify(x => x.DeleteFile(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void ExportService_Export_DeleteThrowsError()
        {
            // Arrange 
            fileSystemMock
                .Setup(x => x.FileExist(It.IsAny<string>()))
                .Returns(true);

            fileSystemMock
                .Setup(x => x.DeleteFile(It.IsAny<string>()))
                .Throws(new Exception("Error deleting"));

            settingsMock
                .SetupGet(m => m.ExportPath)
                .Returns("C:\\");

            // Act
            var exportServiceTestee = CreateExportService();
            var result = exportServiceTestee.Export(new[] { "something" }, "file.txt");

            // Assert
            fileSystemMock.Verify(x => x.DeleteFile(It.IsAny<string>()), Times.Once);

            result.Result.Should().BeFalse();
            result.Error.Should().Be("Error deleting");
        }
    }
}
