using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqTutorials.Tests
{
    [TestClass]
    public class BasicTests
    {
        Mock<IService> serviceMock;

        [TestInitialize]
        public void Setup()
        {
            this.serviceMock = new Mock<IService>();
        }

        private JustAClass CreateTestee()
        {
            return new JustAClass(this.serviceMock.Object);
        }

        [TestMethod]
        public void JustAClass_Execute_ExecuteServiceShouldReturnTrue()
        {
            // Arrange
            this.serviceMock
                .Setup(m => m.IsValid())
                .Returns(true);

            var testee = CreateTestee();

            var parameter = 5;

            // Act
            var result = testee.ExecuteService(parameter);

            // Assert
            this.serviceMock
                 .Verify(m => m.Execute(It.Is<int>(v => v == parameter)), Times.Once);

            result.Should().BeTrue();
        }

        [TestMethod]
        public void JustAClass_Execute_ExecuteServiceShouldReturnFalse()
        {
            // Arrange
            this.serviceMock
                .Setup(m => m.IsValid())
                .Returns(false);

            var testee = CreateTestee();

            // Act
            var result = testee.ExecuteService(0);

            // Assert
            this.serviceMock
                 .Verify(m => m.Execute(0), Times.Once);

            result.Should().BeFalse();
        }

        [TestMethod]
        public void JastAClass_Execute_ExecuteServiceErrorFalse()
        {
            // Arrange
            this.serviceMock
                .SetupGet(m => m.Result)
                .Returns(0);

            var testee = CreateTestee();

            // Act
            var result = testee.Execute2(1);

            // Assert
            this.serviceMock
                 .Verify(m => m.Execute(It.IsAny<int>()), Times.Once);

            result.Should().BeFalse();
        }

        [TestMethod]
        public void JastAClass_Execute_ExecuteServiceErrorTrue()
        {
            // Arrange
            this.serviceMock
                .SetupGet(m => m.Result)
                .Returns(15);

            var testee = CreateTestee();

            // Act
            var result = testee.Execute2(1);

            // Assert
            this.serviceMock
                 .Verify(m => m.Execute(It.IsAny<int>()), Times.Once);

            result.Should().BeTrue();
        }
    } 
}
