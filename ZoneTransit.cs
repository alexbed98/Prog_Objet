using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP03;

internal class ZoneTransit<T>
{
    List<T> données = new();
    object Mutex = new();

    public void Ajouter(List<T> lst)
    {
        lock (Mutex)
        {
            foreach (T t in lst)
                données.Add(t);
        }
    }
    public List<T> Extraire()
    {
        lock (Mutex)
        {
            List<T> lst = new();
            Algos.Permuter(ref lst, ref données);
            return lst;
        }
    }

    public void Vider()
    {
        lock (Mutex)
        {
            données = new();
        }
    }
}
