// Alex Bedard
// TP03

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TP02_Affichage;

namespace TP03;

internal class Afficheur
{
    PipelineAffichage Pipeline { get; init; }
    Surface Surface { get; init; }
    Task Tache { get; set; }
    CanalComm<PipelineInfo> CanalComm { get; init; } 
    Messagerie Mess { get; init; }
    Messagerie Info { get; init; }

    CancellationTokenSource source = new();

    Afficheur(Surface surf)
    {
        Surface = surf;
        Pipeline = new(surf);
    }
    public Afficheur(Surface surf, CanalComm<PipelineInfo> canalComm, Messagerie mess, Messagerie info) 
    {
        Surface = surf;
        Pipeline = new(surf);
        CanalComm = canalComm;
        Mess = mess;
        Info = info;
    }

    public IProjetable Appliquer(Mutable mut)
    {
        return Pipeline.Appliquer(new Point2D(), mut);
    }

    public async Task Démarrer()
    {
        Tache = Task.Run(()
            => Exécuter(source.Token)
            );
    }

    public async Task Arrêter()
    {
        source.Cancel();
        Tache.Wait();
    }

    async Task Exécuter(CancellationToken jeton)
    {
        while (!jeton.IsCancellationRequested)
        {
            List<PipelineInfo> lst = CanalComm.Balayer();
            if (lst.Count > 0)
            {
                var mut = AlgosWAL1D.Dupliquer(Surface);

                foreach (var p in lst)
                    mut = AlgosWAL1D.GénérerHalo(p.Zone, p.Couleur, mut);

                Appliquer(mut);

                AfficherMessagerie();
            }
            await Task.Delay(500);
        }
        AfficherMessagerie();
    }

    void AfficherMessagerie()
    {
        Mess.Afficher();
        Info.Afficher();
    }
}