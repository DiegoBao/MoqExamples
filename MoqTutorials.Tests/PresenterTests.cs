using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Windows.Forms;
using FluentAssertions;

namespace MoqTutorials.Tests
{
    /// <summary>
    /// Summary description for PresenterTests
    /// </summary>
    [TestClass]
    public class PresenterTests
    {
        Mock<IView> viewMock;

        [TestInitialize]
        public void Setup()
        {
            viewMock = new Mock<IView>();
        }

        private Presenter CreateTestee()
        {
            return new Presenter(viewMock.Object);
        }


        [TestMethod]
        public void Presenter_Show_DialogOKReturnTrue()
        {
            // Arrange
            viewMock
                .Setup(m => m.ShowDialog())
                .Returns(DialogResult.OK);

            var presenterTestee = CreateTestee();

            // Act
            var result = presenterTestee.Show();

            // Assert 
            result.Should().BeTrue("Show() should return true on OK");
        }

        [TestMethod]
        public void Presenter_Show_DialogCancelReturnFalse()
        {
            // Arrange
            viewMock
                .Setup(m => m.ShowDialog())
                .Returns(DialogResult.Cancel);

            var presenterTestee = CreateTestee();

            // Act
            var result = presenterTestee.Show();

            // Assert 
            result.Should().BeFalse("Show() should return fales on Cancel");
        }

        [TestMethod]
        public void Presenter_Show_HandleCancel()
        {
            // Arrange
            viewMock
                .Setup(m => m.ShowDialog())
                .Returns(DialogResult.Cancel)
                .Raises(m => m.HandleCancel += null, new EventArgs());

            var presenterTestee = CreateTestee();

            // Act
            var result = presenterTestee.Show();

            // Assert
            result.Should().BeFalse("Show() should return fales on Cancel");

            viewMock
                .VerifySet(m => m.DialogResult = DialogResult.Cancel);
        }

        [TestMethod]
        public void Presenter_Show_HandleEnterInputValid()
        {
            // Arrange
            viewMock
                .Setup(m => m.ShowDialog())
                .Returns(DialogResult.OK)
                .Raises(m => m.HandleEnter += null, new EventArgs());

            var close = false;

            viewMock
                .Setup(m => m.Close())
                .Callback(() =>
                {
                    close = true;
                });

            viewMock
                .SetupGet(m => m.Input)
                .Returns("Some value entered here");

            var presenterTestee = CreateTestee();

            // Act
            var result = presenterTestee.Show();

            // Assert
            result.Should().BeTrue("Show() should return true on OK");
            presenterTestee.Name.Should().Be("Some value entered here");

            viewMock
                .VerifySet(m => m.DialogResult = DialogResult.OK);

            viewMock
                .Verify(m => m.Close(), Times.Once);
        }

        [TestMethod]
        public void Presenter_Show_HandleEnterInputInvalid()
        {
            // Arrange
            viewMock
                .Setup(m => m.ShowDialog())
                .Returns(DialogResult.OK)
                .Raises(m => m.HandleEnter += null, new EventArgs());

            viewMock
                .SetupGet(m => m.Input)
                .Returns(string.Empty);

            var presenterTestee = CreateTestee();

            // Act
            var result = presenterTestee.Show();

            // Assert
            result.Should().BeTrue("Show() should return true on OK");

            viewMock
                .VerifySet(m => m.DialogResult = DialogResult.OK, Times.Never);

            viewMock
                .Verify(m => m.Close(), Times.Never);
        }
    }
}
