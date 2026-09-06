// Alex Bedard
// TP03

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP02_Affichage;

namespace TP03;
static class DimensionsÉcran
{
    const int NB_LIGNES = 25;
    const int NB_COLS = 80;
    public static bool EstDans(Point2D point) =>
        (point.X < NB_COLS && point.X >= 0 && point.Y < NB_LIGNES && point.Y >= 0);
}
