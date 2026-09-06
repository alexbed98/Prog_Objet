// Alex Bedard
// TP03

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP02_Affichage;

namespace TP03;

static class AlgosWAL1D
{
    public static float Distance(this Point2D p0, Point2D p1) =>
        (float)Math.Sqrt(Math.Pow(p1.X - p0.X, 2) + Math.Pow(p1.Y - p0.Y, 2));

    public static Mutable Dupliquer(this Surface surf)
    {
        lock (surf)
        {
            return surf.Dupliquer();
        }
    }

    public static Mutable GénérerHalo(Cercle c, ConsoleColor couleur, Mutable p)
    {
        Mutable res = p.Dupliquer();
        for (int ligne = 0; ligne != res.Hauteur; ++ligne)
            for (int col = 0; col != res.Largeur; ++col)
            {
                Point2D pt = new(col, ligne);
                if (Algos.CalculerDistance(c.Centre, pt) <= c.Rayon)
                    res[pt] = new(res[pt].Symbole, res[pt].Avant, couleur);
            }
        return res;
    }

    public static bool EstDans(this IProjetable proj, Point2D point) =>
        (point.X < proj.Largeur - 1 && point.X > 0
            && point.Y < proj.Hauteur - 1 && point.Y > 0);
}
