// Alex Bedard
// TP03

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GénérateurId;
using TP02_Affichage;

namespace TP03;

public class AucunDétecteurException : Exception { }

internal class NullDétecteur : IDétecteur
{
    public float Rayon 
    {
        get => throw new AucunDétecteurException();
        set => throw new AucunDétecteurException();
    }

    public Cercle Zone 
    { 
        get => throw new AucunDétecteurException();
    }
    public Identifiant Id 
    { 
        get => throw new AucunDétecteurException();
    }

    public List<Point2D> Détecter(IProjetable projetable) => 
        throw new AucunDétecteurException();

    public bool PeutDétecter(Catégorie catégorie) =>
        throw new AucunDétecteurException();
    public bool PeutDétecter(char symbole) =>
        throw new AucunDétecteurException();
}