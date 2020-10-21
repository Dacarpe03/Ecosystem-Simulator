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

    public override void Survive()
    {
        for (Animal prey in this._animals)
        {
            prey.Update(this._animals);
        }
    }
    //END: Constructor and main methods
}
