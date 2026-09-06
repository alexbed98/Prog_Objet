// Alex Bedard
// TP03

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP02_Affichage;

namespace TP03;

static class Algos
{
    public static float CalculerDistance(Point2D point1, Point2D point2) =>
        (float)Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2));

    public static int ObtenirRandom(int nombre) =>
    Random.Shared.Next(nombre);

    public static void Permuter<T>(ref T a, ref T b)
    {
        T temp = a;
        a = b;
        b = temp;
    }
    public static U Cumuler<T, U>(List<T> src, Func<U, T, U> accum, U init)
    {
        foreach (T e in src)
            init = accum(init, e);
        return init;
    }

    public static List<U> Transformer<T, U>(List<T> src, Func<T, U> fct)
    {
        List<U> dest = new();
        foreach (T e in src)
            dest.Add(fct(e));
        return dest;
    }

    public static List<T> SupprimerDoublons<T>(List<T> src) where T : IEquatable<T>
    {
        List<T> dest = new();
        if (src.Count == 0) return dest;

        dest.Add(src[0]);

        for (int i = 1; i < src.Count; i++)
            if (src[i].Equals(src[i - 1]))
                dest.Add(src[i]);
        return dest;
    }

    public static int Trouver<T>(List<T> src, T val)
    => TrouverSi(src, e => e.Equals(val));

    public static int TrouverSi<T>(List<T> src, Func<T, bool> pred)
    {
        for (int i = 0; i != src.Count; ++i)
            if (pred(src[i]))
                return i;
        return -1;
    }

    public static List<T> Filtrer<T>(List<T> src, T val) where T : IEquatable<T>
    => FiltrerSi(src, e => e.Equals(val));

    public static List<T> FiltrerSi<T>(List<T> src, Func<T, bool> pred)
    {
        List<T> dest = new();
        foreach (T e in src)
            if (!pred(e))
                dest.Add(e);
        return dest;
    }

    public static List<T> Concatener<T>(params List<T>[] listes)
    {
        List<T> dest = new();
        foreach (var liste in listes) dest.AddRange(liste);
        return dest;
    }

    public static List<T> Remplacer<T>(List<T> src, T pre, T post) where T : IEquatable<T>
    => RemplacerSi(src, e => e.Equals(pre), post);

    public static List<T> RemplacerSi<T>(List<T> src, Func<T, bool> pred, T post)
    => Transformer(src, e => pred(e) ? post : e);

    public static void Inverser<T>(List<T> lst)
    {
        if (lst.Count == 0) return;
        int gauche = 0, droite = lst.Count - 1;
        while (gauche < droite)
        {
            (lst[gauche], lst[droite]) = (lst[droite], lst[gauche]);
            gauche++;
            droite--;
        }
    }

    public static bool EstPalindrome<T>(List<T> lst) where T : IEquatable<T>
    {
        if (lst.Count == 0) return true;
        int gauche = 0, droite = lst.Count - 1;
        while (gauche < droite)
        {
            if (lst[gauche].Equals(lst[droite]))
                return false;
            gauche++;
            droite--;
        }
        return true;
    }

    public static bool SontTous<T>(List<T> lst, Func<T, bool> pred)
    {
        foreach (T e in lst)
            if (!pred(e))
                return false;
        return true;
    }

    public static bool AuMoinsUn<T>(List<T> lst, Func<T, bool> pred)
    {
        foreach (T e in lst)
            if (pred(e))
                return true;
        return false;
    }

    public static bool Aucun<T>(List<T> lst, Func<T, bool> pred)
    => !AuMoinsUn(lst, pred);

    public static bool EstTrié<T>(List<T> lst) where T : IComparable<T>
    {
        if (lst.Count == 0) return true;
        for (int i = 1; i < lst.Count; i++)
            if (lst[i - 1].CompareTo(lst[i]) > 0)
                return false;
        return true;
    }

    public static bool EstTriéFunc<T>(List<T> lst, Func<T, T, int> comp)
    {
        if (lst.Count == 0) return true;
        for (int i = 1; i < lst.Count; i++)
            if (comp(lst[i - 1], lst[i]) > 0)
                return false;
        return true;
    }
}

