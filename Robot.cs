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

public class NomInvalideException : Exception { }

internal class Robot : IIconifiable
{
    // fait sans utiliser de pile, car vous m'avez dit
    // que je n'était pas obligé de le faire avec cette méthode
    public char Symbole => Nom[0];
    public string Nom { get; }
    public Point2D Pos { get; private set; }
    IDétecteur Détecteur { get; set; } = new NullDétecteur();
    public Cercle Zone => Détecteur.Zone;
    public Identifiant IdDétecteur => Détecteur.Id;
    public Robot(string? nom, Point2D pos)
    {
        if (nom == null || nom.Length < 0 || char.IsWhiteSpace(nom[0]))
            throw new NomInvalideException();

        Nom = nom;
        Pos = pos;
    }

    public void Équiper(IDétecteur détecteur)
    {
        Détecteur = détecteur;
    }

    public bool PeutDétecter(char symbole) =>
        Détecteur.PeutDétecter(symbole);

    public List<Point2D> Détecter(IProjetable projetable) =>
        Détecteur.Détecter(projetable);

    public List<Point2D> Détecter(IProjetable projetable, Catégorie catégorie)
    {
        if (!Détecteur.PeutDétecter(catégorie))
            return new List<Point2D>();
        else
            return Détecteur.Détecter(projetable);
    }

    public void AugmenterPuissance() { Détecteur.Rayon++; }

    public void RéinitialiserPuissance() { Détecteur.Rayon = 1; }
     
    public void DéplacerVers(Point2D destination, Surface surface)
    {
        surface.Retirer(this);
        Pos = ObtenirProchainPoint(destination);
        surface.Ajouter(this);

        RéinitialiserPuissance();
    }

    public Point2D TrouverPassage(Surface surface)
    {
        IProjetable proj = AlgosWAL1D.Dupliquer(surface);

        List<Point2D> casesVoisines = new(8);

        for (int ligne = 0; ligne != proj.Hauteur; ligne++)
        {
            for (int colonne = 0; colonne != proj.Largeur; colonne++)
            {
                Point2D point = new(ligne, colonne);
                if (proj.EstDans(point) && proj[point].EstVide && EstVoisin(point))
                    casesVoisines.Add(point);
            }
        }

        if (casesVoisines.Count == 0)
            return Pos;
        else
            return casesVoisines[Algos.ObtenirRandom(casesVoisines.Count)];
    }

    bool EstVoisin(Point2D point)
    {
        int écartX = Pos.X - point.X;
        int écartY = Pos.Y - point.Y;

        return ((écartX >= -1 && écartX <= 1) && (écartY >= -1 && écartY <= 1));
    }

    public Point2D CalculerDéplacementVers(Point2D destination)
    {
        if (destination.X == Pos.X && destination.Y == Pos.Y)
            return Pos;
        else
            return ObtenirProchainPoint(destination);
    }

    Point2D ObtenirProchainPoint(Point2D destination)
    {
        int x = Pos.X, y = Pos.Y;

        void VérifierDirection(ref int z, int destination)
        {
            if (z < destination)
                z += 1;
            else if (z > destination)
                z -= 1;
        }

        VérifierDirection(ref x, destination.X);
        VérifierDirection(ref y, destination.Y);

        return new Point2D(x, y);
    }
}
