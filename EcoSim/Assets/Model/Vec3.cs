using System;

public class Vec3
{
    //SECTION: Attributes and properties
    private float _xCoord;
    public float XCoord { get => _xCoord; }

    private float _yCoord;
    public float YCoord { get => _yCoord; }

    private float _zCoord;
    public float ZCoord { get => _zCoord; }
    //END: Attributes and properties


    //SECTION: Constructor and main methods
    public Vec3(float x, float y, float z)
    {
        this._xCoord = x;
        this._yCoord = y;
        this._zCoord = z;
    }

    public Vec3(System.Random rand)
    {
        this.RandomizeCoords(rand);
        this.RandomizeCoords(rand);
    }

    public void RandomizeCoords(System.Random rand)
    {
        this._xCoord = (float) rand.NextDouble() * 100;
        this._yCoord = 0f;
        this._zCoord = (float)rand.NextDouble() * 100;
    }
    //END: Constructor and main methods

    //SECTION:Secondary methods
    public float SquareDistanceTo(Vec3 other)
    {
        v = CalculateVectorBetweenPoints(this, other);
        v = 
    }
    //END: Secondary methods
}
