﻿using System;

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

    public void Add(Vec3 other)
    {
        this._xCoord += other.XCoord;
        this._yCoord += other.YCoord;
        this._zCoord += other.ZCoord;
    }

    public void Substract(Vec3 other)
    {
        this._xCoord -= other.XCoord;
        this._yCoord -= other.YCoord;
        this._zCoord -= other.ZCoord;
    }

    public void Multiply(double number)
    {
        this._xCoord *= number;
        this._yCoord *= number;
        this._zCoord *= number;
    }

    public void Divide(double number)
    {
        this._xCoord /= number;
        this._yCoord /= number;
        this._zCoord /= number;
    }

    public void Normalize() 
    {
        double vecModule = this.Module;
        this.Divide(vecModule);
    }

    //This method sets a maximum magnitude to the vector
    public void Trim(double squaredMagnitude)
    {
        if(this.SquaredModule > squaredMagnitude)
        {
            this.Normalize();
            double magnitude = Math.Sqrt(squaredMagnitude);
            this.Multiply(magnitude);
        }
    }

    public void Expand(double magnitude)
    {
        this.Normalize();
        this.Multiply(magnitude);
    }

    public Boolean IsZero()
    {
        return (this._xCoord == 0 & this._yCoord == 0 & this._zCoord == 0);
    }
    //END: Secondary methods


    //SECTION: Static methods
    public static Vec3 CalculateVectorsBetweenPoints(Vec3 pointA, Vec3 pointB) //Returns AB Vector
    {
        double xCoord = pointB.XCoord - pointA.XCoord;
        double yCoord = pointB.YCoord - pointA.YCoord;
        double zCoord = pointB.ZCoord - pointA.ZCoord;

        return new Vec3(xCoord, yCoord, zCoord);
    }

    public static Vec3 Zero()
    {
        return new Vec3(0, 0, 0);
    }
    //END: Static methods
}
