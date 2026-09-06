// Alex Bedard
// TP03

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP02_Affichage;
using GénérateurId;

namespace TP03;

internal class Déchet : IIconifiable
{
    Identifiant Id { get; }
    public char Symbole { get; }
    public Point2D Pos { get; }
    public Catégorie Famille { get; }

    public Déchet(char symbole, Catégorie famille, Point2D pos)
    {
        FabriqueGénérateurs fabrique = new();
        IGénérateurId generateur = fabrique.Créer(TypeGénérateur.Partagé, "MET");
        Id = generateur.Prendre();

        Symbole = symbole;
        Famille = famille;
        Pos = pos;
    }
}
