using System.Collections;
using System.Collections.Generic;

public class Resource
{
    private double _quantity;
    private double _growthFactor;

    public Resource (double initialQuantity, double increaseFactor)
    {
        this._quantity = initialQuantity;
        this._growthFactor = increaseFactor;
    }

    public bool resourcesAvailable()
    {
        if (this._quantity >= 1)
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

    public void Grow()
    {
        this._quantity *= this._growthFactor;
    }
}
