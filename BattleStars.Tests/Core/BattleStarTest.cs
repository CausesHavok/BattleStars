using Moq;
using System.Numerics;
using BattleStars.Core;
using BattleStars.Shapes;

namespace BattleStars.Tests.Core;

public class BattleStarTest
{

    #region Construction Tests

    [Fact]
    public void GivenNullShape_WhenConstructed_ThenThrowsArgumentNullException()
    {
        // Arrange
        IShape nullShape = null!;
        var mockShapeDrawer = new Mock<IShapeDrawer>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new BattleStar(nullShape, mockShapeDrawer.Object));
    }

    [Fact]
    public void GivenNullShapeDrawer_WhenConstructed_ThenThrowsArgumentNullException()
    {
        // Arrange
        var mockShape = new Mock<IShape>();
        IShapeDrawer nullShapeDrawer = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new BattleStar(mockShape.Object, nullShapeDrawer));
    }

    [Fact]
    public void GivenValidInput_WhenConstructed_ThenDoesNotThrow()
    {
        // Arrange
        var mockShape = new Mock<IShape>();
        var mockShapeDrawer = new Mock<IShapeDrawer>();

        // Act & Assert
        var exception = Record.Exception(() => new BattleStar(mockShape.Object, mockShapeDrawer.Object));
        Assert.Null(exception);
    }

    #endregion


    #region Draw Delegation Tests

    [Fact]
    public void GivenBattleStar_WhenDrawCalled_ThenDelegatesToShape()
    {
        // Arrange
        var mockShape = new Mock<IShape>();
        var mockShapeDrawer = new Mock<IShapeDrawer>();
        var battleStar = new BattleStar(mockShape.Object, mockShapeDrawer.Object);

        // Act
        battleStar.Draw();

        // Assert
        mockShape.Verify(s => s.Draw(It.IsAny<Vector2>(), mockShapeDrawer.Object), Times.Once);
    }

    [Fact]
    public void GivenBattleStar_WhenGetBoundingBoxCalled_ThenDelegatesToShape()
    {
        // Arrange
        var mockShape = new Mock<IShape>();
        var mockShapeDrawer = new Mock<IShapeDrawer>();
        var battleStar = new BattleStar(mockShape.Object, mockShapeDrawer.Object);
        var expectedBoundingBox = new BoundingBox();
        mockShape.Setup(s => s.BoundingBox).Returns(expectedBoundingBox);

        // Act
        var result = battleStar.GetBoundingBox();

        // Assert
        Assert.Equal(expectedBoundingBox, result);
    }

    #endregion
}