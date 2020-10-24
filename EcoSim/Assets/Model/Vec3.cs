using System;

public class Vec3
{
    //SECTION: Attributes and properties
    private double _xCoord;
    public double XCoord { get => _xCoord; }

    private double _yCoord;
    public double YCoord { get => _yCoord; }

    private double _zCoord;
    public double ZCoord { get => _zCoord; }

    public double SquaredModule => this._xCoord * this._xCoord + this._yCoord * this._yCoord + this._zCoord * this._zCoord;
    public double Module => Math.Sqrt(this.SquaredModule);
    //END: Attributes and properties


    //SECTION: Constructor and main methods
    public Vec3(double x, double y, double z)
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

    public void RandomizeCoords(Random rand)
    {
        this._xCoord = rand.NextDouble() * 100;
        this._yCoord = 0f;
        this._zCoord = rand.NextDouble() * 100;
    }
    //END: Constructor and main methods

    //SECTION:Secondary methods
    public double SquaredDistanceTo(Vec3 other)
    {
        Vec3 v = Vec3.CalculateVectorBetweenPoints(this, other);
        return v.SquaredModule;
    }
    //END: Secondary methods

}
