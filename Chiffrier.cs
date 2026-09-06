// Alex Bedard
// TP03

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP03;

static internal class Chiffrier 
{
    static Dictionary<char, int> Déchets { get; } = new();
    public static void ComptabiliserCollecte(char symbole)
    {
        lock (Déchets)
        {
            if (!Existe(symbole))
                Déchets.Add(symbole, 1);
            else
                Déchets[symbole] += 1;
        }
    }

    public static string ObtenirStatistiques()
    {
        lock (Déchets)
        {
            StringBuilder sb = new();
            sb.Append("Collectés : ");
            foreach (KeyValuePair<char, int> déchet in Déchets)
                sb.Append(déchet.Key + " X " + déchet.Value + "; ");
            return sb.ToString();
        }
    }

    // Précondition: l'accès à "Déchets" doit être vérrouillé...
    // ...AVANT l'appel du prédicat "Existe"
    static bool Existe(char symbole) =>
            Déchets.ContainsKey(symbole);
}
