// Alex Bedard
// TP03

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP03;

internal struct PipelineInfo
{
    public Cercle Zone { get; }
    public ConsoleColor Couleur { get; }
    public char Symbole { get; }
    public PipelineInfo(Cercle zone, ConsoleColor couleur, char symbole)
    {
        Zone = zone;
        Couleur = couleur;
        Symbole = symbole;
    }
}
