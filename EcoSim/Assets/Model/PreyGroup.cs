using System;

public class PreyGroup : AnimalGroup
{
    public PreyGroup(int size)
    {
        this._size = size;

        for(int i=0; i<size; i++)
        {
            Animal a = new Animal();
            this._animals.Add(a);
        }
    }

}
