using ConsoleApp4.Data;
using FluentAssertions;

namespace ModelTests
{
    public class ModelTests
    {
        [Fact]
        public void V1DataArrayFromFieldsTest()
        {
            V1DataArray V1DataArray = new V1DataArray("DataArray", DateTime.Now, 5, 1, 5, true, MyFunctions.MyFunction1);

            V1DataArray.NumOfNodes.Should().Be(5);
            V1DataArray.xArray.Length.Should().Be(5);
            V1DataArray.yArray[0].Length.Should().Be(5);
            V1DataArray.yArray[1].Length.Should().Be(5);
            for (int i = 0; i < 5; i++)
            {
                V1DataArray.xArray[i].Should().Be(1 + (5 - 1) * i / (5 - 1));
                V1DataArray.yArray[0][i].Should().Be(2*(1 + (5 - 1) * i / (5 - 1)));
            }
        }

        [Fact]
        public void V1DataArrayFromFileTest()
        {
            V1DataArray V1DataArray = new V1DataArray("DataArray", DateTime.Now, 5, 1, 5, true, MyFunctions.MyFunction1);
            V1DataArray.Save("SaveTest");
            V1DataArray V1DataArrayFile = new V1DataArray("DataArray", DateTime.Now,"SaveTest");
            V1DataArray.NumOfNodes.Should().Be(5);
            V1DataArray.xArray.Length.Should().Be(5);
            V1DataArray.yArray[0].Length.Should().Be(5);
            V1DataArray.yArray[1].Length.Should().Be(5);
            for (int i = 0; i < 5; i++)
            {
                V1DataArray.xArray[i].Should().Be(1 + (5 - 1) * i / (5 - 1));
                V1DataArray.yArray[0][i].Should().Be(2 * (1 + (5 - 1) * i / (5 - 1)));
            }
        }
        [Fact]
        public void Indexer_GivenValidIndex_ShouldReturnCorrectArray()
        {
            // Arrange
            double[] x = { 0, 1, 2 };
            V1DataArray dataArray = new V1DataArray("test", DateTime.Now, x, MyFunctions.MyFunction1);

            // Act & Assert
            Assert.NotNull(dataArray[0]);
            Assert.NotNull(dataArray[1]);
            Assert.Throws<ArgumentOutOfRangeException>(() => dataArray[2]);
        }
        [Fact]
        public void ListProperty_ShouldReturnCorrectV1DataListInstance()
        {
            // Arrange
            double[] x = { 0, 1, 2 };
            V1DataArray dataArray = new V1DataArray("test", DateTime.Now, x, MyFunctions.MyFunction1);
            int expectedCount = x.Length;

            // Act
            var list = dataArray.List;

            // Assert
            Assert.Equal(expectedCount, list.DataItems.Count);
        }

        [Fact]
        public void Constructor_WithKeyAndDate_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            string expectedKey = "TestData";
            DateTime expectedDate = DateTime.Now;

            // Act
            V1DataArray dataArray = new V1DataArray(expectedKey, expectedDate);

            // Assert
            Assert.Equal(expectedKey, dataArray.Key);
            Assert.Equal(expectedDate, dataArray.Date);
        }
    }
}