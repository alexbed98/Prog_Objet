// Alex Bedard
// TP03

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP02_Affichage;

namespace TP03;

public class RayonIllégalException : Exception { }

internal class Cercle
{
    public float Rayon { get; }
    public Point2D Centre { get; }

    public Cercle() : this(1, new()) { }

    public Cercle(float rayon) : this(rayon, new()) { }

    public Cercle(float rayon, Point2D centre)
    {
        if (rayon < 1)
            throw new RayonIllégalException();

        Rayon = rayon;
        Centre = centre;
    }

    public bool Contient(Point2D point) =>
        Rayon >= Algos.CalculerDistance(Centre, point);
}
