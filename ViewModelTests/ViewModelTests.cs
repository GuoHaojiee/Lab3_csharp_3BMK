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
            // Настройка мока, чтобы симулировать выбор пользователя открывать несуществующий файл
            var mockData = new V1DataArray("mockData", DateTime.Now);


            var viewData = new ViewData(errorSenderMock.Object, fileDialogMock.Object);

            viewData.LoadFromFileCommand.Execute(null);

            fileDialogMock.Verify(m => m.OpenFileDialog(), Times.Once);

        }

        //Доп. задания  Проверка когда файл не существует
        [Fact]
        public void LoadFromFileCommandHandler_FileNotExist_ShouldSendError()
        {
            var errorSenderMock = new Mock<IErrorSender>();
            var fileDialogMock = new Mock<IFileDialog>();
            string nonexistentFilename = "nonexistent.v1d";// Имитация выбора несуществующего файла

            fileDialogMock.Setup(m => m.OpenFileDialog()).Returns(nonexistentFilename);
            // то что вы мне спрашивали:
            // Это настройка мока, чтобы симулировать выбор пользователя открывать несуществующий файл
            var viewData = new ViewData(errorSenderMock.Object, fileDialogMock.Object);

            viewData.LoadFromFileCommand.Execute(null);

            errorSenderMock.Verify(m => m.SendError(It.IsAny<string>()), Times.AtLeastOnce);
            // Проверка, что метод отправки ошибки был вызван хотя бы один раз
        }

    }
}