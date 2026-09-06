// Alex Bedard
// TP03

using TP02_Affichage;
using TP03;

ConfigInfo config = Config.LireConfig("../../../config_tp2.json");

// préparer la surface d'affichage
Surface surf = FabriqueSurface.Créer(config);

// préparer la zone de messagerie
Messagerie messagerie = new(new(surf.Largeur, 0), surf.Hauteur);
// préparer la zone d'informations
Messagerie information = new(new(0, surf.Hauteur), 2);

List<Robot> robots = FabriqueSurface.CréerRobots(config.Robot, surf);
CancellationTokenSource src = new();
Task saboteur = CréerSaboteur
(
    robots, surf, src.Token
);

CanalComm<PipelineInfo> canal = new(robots.Count);
Afficheur pipeline = new(surf, canal, messagerie, information);

await pipeline.Démarrer();

Task tachesRobot = CréerTâches
(
   robots, surf, canal, messagerie, information, src.Token
);

Task lireTouche = Task.Run(() =>
{
    Console.ReadKey(true);
    src.Cancel();
});

await Task.WhenAny(tachesRobot, lireTouche);
await pipeline.Arrêter();

static Task CréerTâches
(
   List<Robot> robots, Surface surf, CanalComm<PipelineInfo> canal,
   Messagerie messagerie, Messagerie information, CancellationToken jeton
)
{

    Task[] taches = new Task[robots.Count];
    for (int r = 0; r < robots.Count; r++)
    {
        var robot = robots[r];
        int portNo = r;

        taches[r] = Task.Run(async () =>
        {
            bool décédé = false;
            // tant qu'il reste des déchets à ramasser
            while (!décédé && !jeton.IsCancellationRequested &&
                  surf.TrouverSi(c => robot.PeutDétecter(c)).Count > 0)
            {
                bool trouvé = false;
                // Trouver et ramasser le déchet

                while (!décédé && !jeton.IsCancellationRequested && !trouvé)
                {
                    (trouvé, décédé) = await TrouverEtRamasserAsync(
                        robot, canal, surf, portNo, messagerie, information, jeton
                    );
                }
                robot.RéinitialiserPuissance();
            }
            if (!décédé && surf.TrouverSi(c => robot.PeutDétecter(c)).Count == 0)
                messagerie.Ajouter(ConsoleColor.White,
                                   $"{robot.Nom} : nettoyage complété, mise au repos");
        });
    }
    return Task.WhenAll(taches);
}

static async Task<(bool, bool)> TrouverEtRamasserAsync(Robot robot, CanalComm<PipelineInfo> canal, Surface surf,
                                                       int portNo, Messagerie messagerie, Messagerie information,
                                                       CancellationToken jeton)
{
    bool trouvé = false;
    bool décédé = false;

    canal.PublierSur(portNo, new(robot.Zone, CatalogueCouleurs.Get.ObtenirCouleur(robot.Symbole,
                                                                ConsoleColor.Green), robot.Symbole));
    var pts = robot.Détecter(surf);
    if (pts.Count > 0)
    {
        trouvé = true;
        Point2D nouvellePosi = robot.CalculerDéplacementVers(pts[0]);
        if (pts[0] == nouvellePosi)
        {
            char c = surf[pts[0]].Symbole;

            Chiffrier.ComptabiliserCollecte(c);

            messagerie.Ajouter
            (
               CatalogueCouleurs.Get.ObtenirCouleur(robot.Symbole,
                                                    ConsoleColor.Green),
               $"Déchet collecté à la position {nouvellePosi}"
            );
            surf.Retirer(pts[0]);
        }
        else
        {
            if (surf[nouvellePosi].EstVide)
                messagerie.Ajouter
                (
                   CatalogueCouleurs.Get.ObtenirCouleur(robot.Symbole,
                                                        ConsoleColor.Green),
                   $"Trouvé {pts.Count} déchet(s)",
                   $"Déplacement vers {pts[0]}"
                );
            else
            {
                nouvellePosi = robot.TrouverPassage(surf);
                messagerie.Ajouter
                (
                  CatalogueCouleurs.Get.ObtenirCouleur(robot.Symbole,
                                                       ConsoleColor.Green),
                  $"Déplacement impossible vers {pts[0]}",
                  $"Contournement par {nouvellePosi}"
                );
            }
            await Task.Delay(100);
        }
        try
        {
            robot.DéplacerVers(nouvellePosi, surf);
        }
        catch (DéplacementFatalException ex)
        {
            messagerie.Ajouter(ConsoleColor.White, $"{robot.Nom} : {ex.Message}");
            décédé = true;
        }

        information.Ajouter(ConsoleColor.White, Chiffrier.ObtenirStatistiques(),
                                "Pressez une touche pour terminer");
    }
    else
    {
        robot.AugmenterPuissance();
    }
    await Task.Delay(400);
    return (trouvé, décédé);
}

static async Task CréerSaboteur
   (List<Robot> robots, Surface surf, CancellationToken jeton)
{
    await Task.Run(async () =>
    {
        Random rnd = new();
        bool terminé = false;
        Bombe[] zeBombes;
        List<char> symRobots = new();
        foreach (var r in robots)
            symRobots.Add(r.Symbole);
        while (!terminé)
        {
            lock (surf)
            {
                // Sélectionner un robot au hasard
                int idxRobot = rnd.Next(robots.Count);
                var posiLibres = surf.TrouverSi
                (
                   c => c == default || c == ' ',
                   surf.Cadre.Exclure
                );
                List<Bombe> bombes = new();
                foreach (var pl in posiLibres)
                    if (pl.Distance(robots[idxRobot].Pos) < 2)
                        bombes.Add(new('.', pl));

                zeBombes = bombes.ToArray();
                surf.Ajouter(zeBombes);
            }
            // Laisser les bombes un temps aléatoire
            await Task.Delay(rnd.Next(100, 201));
            lock (surf)
                surf.Retirer(zeBombes);
            if (jeton.IsCancellationRequested)
                terminé = true;
            else
                await Task.Delay(2000); // Répéter à toutes les 2 secondes
        }
    });
}


