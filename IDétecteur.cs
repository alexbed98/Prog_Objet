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
internal interface IDétecteur
{
    float Rayon { get; set; }
    Cercle Zone { get; }
    Identifiant Id { get; }

    List<Point2D> Détecter(IProjetable projetable);

    bool PeutDétecter(Catégorie catégorie);
    bool PeutDétecter(char symbole);
}
