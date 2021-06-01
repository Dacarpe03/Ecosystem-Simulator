using System.Collections;
using System.Collections.Generic;

public class Resource
{
    private double _quantity;
    private double _increaseFactor;

    public Resource (double initialQuantity, double increaseFactor)
    {
        this._quantity = initialQuantity;
        this._increaseFactor = increaseFactor;
    }

    public bool resourcesAvailable()
    {
        if (this._quantity > 1)
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

    public void Increase()
    {
        this._quantity *= this._increaseFactor;
    }
}
