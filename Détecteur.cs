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

internal class Détecteur : IDétecteur
{
    public float Rayon { get; set; }
    public Cercle Zone => new Cercle(Rayon, Pos);
    public Identifiant Id { get; } = Gen.Prendre();
    public Point2D Pos => (Source.Pos);
    public IPositionnable Source { get; }
    public Catégorie Cat { get; }
    static IGénérateurId Gen { get; } = new FabriqueGénérateurs().Créer(TypeGénérateur.Séquentiel, "DET");

    public Détecteur(IPositionnable source, float rayon, Catégorie cat)
    {
        Source = source;

        if (rayon < 1)
            throw new RayonIllégalException();

        Rayon = rayon;
        Cat = cat;
    }
    public List<Point2D> Détecter(IProjetable projetable)
    {
        List<Point2D> points = new();

        for (int i = 0; i < projetable.Hauteur; i++)
        {
            for (int j = 0; j < projetable.Largeur; j++)
            {
                Point2D point = new(j, i);
                Case c = projetable[point];

                if (Zone.Contient(point) && CatalogueDéchets.Get.Est(c.Symbole, Cat))
                    points.Add(point);
            }
        }
        return points;
    }

    public bool PeutDétecter(Catégorie catégorie) =>
        Cat == catégorie;

    public bool PeutDétecter(char symbole) =>
        CatalogueDéchets.Get.Est(symbole, Cat);
}
