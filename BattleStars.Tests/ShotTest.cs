namespace BattleStars.Tests;

public class ShotTest
{

    /* --- Tests Shot class functionality
        * Tests the constructor
        * Tests the properties
            * Tests the damage
                * NaN or Infinity values are not allowed
                * Negative values are not allowed
                * Other values are allowed
            * Tests the position
                * NaN or Infinity values are not allowed
                * Other values are allowed
            * Tests the direction
                * NaN or Infinity values are not allowed
                * Values must be normalized
                * Other values are allowed
            * Tests the Speed property
                * NaN or Infinity values are not allowed
                * Negative values are not allowed
                * Other values are allowed
        * Tests the Update method
            * If the shot is dead, it should not update its position
            * If the direction is zero, it should not update its position
            * If the speed is zero, it should not update its position
            * Should update the position based on the direction and speed
    */




    [Fact]
    public void Test1()
    {

    }
}
