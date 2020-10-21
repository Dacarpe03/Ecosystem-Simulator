using System;

public class PreyGroup : AnimalGroup
{
    //SECTION: Constructor and main methods
    public PreyGroup(int size)
    {
        this._size = size;

        for(int i=0; i<size; i++)
        {
            Animal a = new Animal();
            this._animals.Add(a);
        }
    }
    //END: Constructor and main methods
}
