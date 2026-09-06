// Alex Bedard
// TP03

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP02_Affichage;

namespace TP03;

class PipelineAffichage
{
    List<Func<Mutable, Mutable>> Transfos { get; } = new();

    public PipelineAffichage(Surface surface)
    {
        Ajouter(Appliquer(surface.Cadre));
    }

    public void Ajouter(Func<Mutable, Mutable> transfo) =>
    Transfos.Add(transfo);
    public IProjetable Appliquer(Point2D pos, Mutable p)
    {
        foreach (var transfo in Transfos)
            p = transfo(p);
        return Afficher(pos, p);
    }
    public IProjetable Appliquer(Mutable p) =>
    Appliquer(new(), p);
    public IProjetable Afficher(Point2D pos, Mutable p)
    {
        for (int ligne = 0; ligne != p.Hauteur; ++ligne)
            for (int col = 0; col != p.Largeur; ++col)
            {
                Console.SetCursorPosition(col + pos.X, ligne + pos.Y);
                var préF = Console.ForegroundColor;
                var préB = Console.BackgroundColor;
                Console.ForegroundColor = p[ligne, col].Avant;
                Console.BackgroundColor = p[ligne, col].Arrière;
                Console.Write
                (
                    p[ligne, col].Symbole == default ?
                        ' ' : p[ligne, col].Symbole
                );
                Console.BackgroundColor = préB;
                Console.ForegroundColor = préF;
            }
        return p;
    }
    // retourne une fonction qui transforme un mutable pour
    // y ajouter un cadre
    public Func<Mutable, Mutable> Appliquer(Cadre p)
    {
        return m =>
        {
            Mutable res = m.Dupliquer();
            for (int i = 0; i != p.Hauteur; ++i)
                for (int j = 0; j != p.Largeur; ++j)
                {
                    var c = p[i, j];
                    if (c.Symbole != default)
                        res[i, j] = c;
                }
            return res;
        };
    }
}
