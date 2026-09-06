// Alex Bedard
// TP03

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP02_Affichage;

namespace TP03;

public class SurfacePleineException : Exception { }
public class CasesLibresInsuffisant : Exception { }

static class FabriqueSurface
{
    public static Surface Créer(ConfigInfo configInfo)
    {
        int hauteur = configInfo.Surface.Hauteur;
        int largeur = configInfo.Surface.Largeur;

        Surface surface = new(hauteur, largeur);
        surface.Cadre = new Cadre(hauteur, largeur, ConsoleColor.Cyan);

        InitialiserPollution(configInfo.CatégorieDéchet, surface);

        return surface;
    }

    static void InitialiserPollution(List<CatégorieDéchet> listeCat, Surface surface)
    {
        List<Déchet> déchets = new();
        List<Point2D> casesLibres = TrouverCasesLibres(surface);

        foreach (CatégorieDéchet catégorie in listeCat)
        {
            InitialiserDéchets(catégorie, déchets, casesLibres);
        }

        surface.Ajouter(déchets.ToArray());
    }

    static void InitialiserDéchets(CatégorieDéchet catégorieDéchet, List<Déchet> déchets, List<Point2D> casesLibres)
    {
        int nombreDéchets = catégorieDéchet.NbDéchetsParCat;

        if (casesLibres.Count < nombreDéchets)
            throw new CasesLibresInsuffisant();

        CatalogueDéchets.Get.Associer(catégorieDéchet.Symbole, catégorieDéchet.Catégorie);

        for (int i = 0; i < nombreDéchets; i++)
        {
            int indiceRandom = Algos.ObtenirRandom(casesLibres.Count);
            Point2D pointRandom = casesLibres[indiceRandom];
            Déchet déchet = new(catégorieDéchet.Symbole, catégorieDéchet.Catégorie, pointRandom);

            déchets.Add(déchet);
            casesLibres.RemoveAt(indiceRandom);
        }
    }

    public static List<Point2D> TrouverCasesLibres(Surface surface)
    {
        List<Point2D> libres = surface.TrouverSi
        (
            c => c == default || c == ' ',
            surface.Cadre.Exclure
        );
        return libres;
    }

    public static List<Robot> CréerRobots(List<InfoRobot> infoRobots, Surface surface)
    {
        List<Point2D> casesLibres = TrouverCasesLibres(surface);
        int nbRobots = infoRobots.Count;

        if (casesLibres.Count < nbRobots)
            throw new SurfacePleineException();

        List<Robot> robots = new();

        for (int i = 0; i < nbRobots; i++)
        {
            int indiceRandom = Algos.ObtenirRandom(casesLibres.Count);

            Robot robot = new(infoRobots[i].Nom, casesLibres[indiceRandom]);
            Détecteur détecteur = new(robot, 1, infoRobots[i].Catégorie);

            CatalogueCouleurs.Get.Associer(infoRobots[i].Symbole, infoRobots[i].Couleur);

            robot.Équiper(détecteur);
            surface.Ajouter(robot);

            casesLibres.RemoveAt(indiceRandom);
            robots.Add(robot);
        }

        return robots;
    }
}