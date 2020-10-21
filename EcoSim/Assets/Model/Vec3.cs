using System;

public class Vec3
{
    //SECTION: Attributes and properties
    private float _xCoord;
    public float XCoord { get => _xCoord; }

    private float _yCoord;
    public float YCoord { get => _yCoord; }

    private float _zCoord;
    public float ZCoord { get => _yCoord; }
    //END: Attributes and properties


    //SECTION: Constructor and main methods
    public Vec3(float x, float y, float z)
    {
        this._xCoord = x;
        this._yCoord = y;
        this._zCoord = z;
    }

    public Vec3()
    {
        this.RandomizeCoords();
    }

    public void RandomizeCoords()
    {
        var rand = new Random();
        this._xCoord = (float) rand.NextDouble() * 100;
        this._yCoord = (float)rand.NextDouble() * 100;
        this._zCoord = 0f;
    }
    //END: Constructor and main methods

}
