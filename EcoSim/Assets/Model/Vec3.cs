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
        Vec3 v = Vec3.CalculateVectorsBetweenPoints(this, other);
        return v.SquaredModule;
    }

    public void Normalize()
    {
        double vecModule = this.Module;
        this._xCoord /= vecModule;
        this._yCoord /= vecModule;
        this._zCoord /= vecModule;
    }

    public void Add(Vec3 other)
    {
        this._xCoord += other.XCoord;
        this._yCoord += other.XCoord;
        this._zCoord += other.XCoord;
    }

    public void Substract(Vec3 other)
    {
        this._xCoord -= other.XCoord;
        this._yCoord -= other.XCoord;
        this._zCoord -= other.XCoord;
    }
    //END: Secondary methods


    //SECTION: Static methods
    public static Vec3 CalculateVectorsBetweenPoints(Vec3 pointA, Vec3 pointB)
    {
        double xCoord = pointB.XCoord - pointA.XCoord;
        double yCoord = pointB.YCoord - pointB.YCoord;
        double zCoord = pointB.ZCoord - pointB.ZCoord;

        return new Vec3(xCoord, yCoord, zCoord);
    }
    //END: Static methods
}
