using ConsoleApp4.Data;
using Moq;
using ViewModel;

namespace ViewModelTests
{
    public class ViewModelTests
    {
        [Fact]
        public void Constructor_InitializesCommandsWithCanExecuteTrue()
        {
            var errorSenderMock = new Mock<IErrorSender>();
            var fileDialogMock = new Mock<IFileDialog>();
            var viewData = new ViewData(errorSenderMock.Object, fileDialogMock.Object);

            bool canSaveFile = viewData.SaveFileCommand.CanExecute(null);
            bool canLoadFromFile = viewData.LoadFromFileCommand.CanExecute(null);

            Assert.True(canSaveFile);
            Assert.True(canLoadFromFile);
        }

        [Fact]
        public void LoadFromFileCommandHandler_LoadsDataSuccessfully()
        {
            var errorSenderMock = new Mock<IErrorSender>();
            var fileDialogMock = new Mock<IFileDialog>();
            string expectedFilename = "testData.v1d";
            fileDialogMock.Setup(m => m.OpenFileDialog()).Returns(expectedFilename);

            var mockData = new V1DataArray("mockData", DateTime.Now);


            var viewData = new ViewData(errorSenderMock.Object, fileDialogMock.Object);

            viewData.LoadFromFileCommand.Execute(null);

            fileDialogMock.Verify(m => m.OpenFileDialog(), Times.Once);

        }

    }
}