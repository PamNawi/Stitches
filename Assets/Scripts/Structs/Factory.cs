using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Factory<T>
{
    static Dictionary<string, Func<T>> _dict = new Dictionary<string, Func<T>>();

    public Factory() { }

    public T Create(string id)
    {
        Func<T> constructor = null;
        if (_dict.TryGetValue(id, out constructor))
            return constructor();
        throw new ArgumentException("No type registered for this id:" + id );
    }

    public void Register(string id, Func<T> ctor)
    {
        _dict.Add(id, ctor);
    }
}