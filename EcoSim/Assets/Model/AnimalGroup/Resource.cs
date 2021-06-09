using System.Collections;
using System.Collections.Generic;

public class Resource
{
    private double _quantity;
    public double Quantity { get => this._quantity; }
    private double _growthFactor;
    private double _threshold;

    public Resource (double initialQuantity, double increaseFactor, double threshold)
    {
        this._quantity = initialQuantity;
        this._growthFactor = increaseFactor;
        this._threshold = threshold;
    }

    public bool resourcesAvailable()
    {
        if (this._quantity >= (1 + this._threshold))
        {
            this._quantity -= 1;
            return true;
        }
        else
        {
            this._quantity = 0;
            return false;
        }
    }

    public void addUnits(int n)
    {
        this._quantity += n;
    }

    public void Grow()
    {
        if (this._quantity < this._threshold)
        {
            this._quantity = this._threshold;
        }

        this._quantity *= this._growthFactor;
    }
}
