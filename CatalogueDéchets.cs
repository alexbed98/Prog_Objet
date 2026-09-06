// Alex Bedard
// TP03

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP02_Affichage;

namespace TP03;

public class SymboleIntrouvableException : Exception { }

public enum Catégorie { Vide, Métal, Plastique, Organique, Nucléaire, Explosif }

internal class CatalogueDéchets
{
    public static CatalogueDéchets Get { get; } = new();

    Dictionary<char, List<Catégorie>> associationSymboles = new();

    CatalogueDéchets() { }

    public void Associer(char symbole, Catégorie catégorie)
    {
        if (!Existe(symbole))
            associationSymboles.Add(symbole, new List<Catégorie> { catégorie });
        else if (!Est(symbole, catégorie))
            associationSymboles[symbole].Add(catégorie);
    }

    public bool Est(char symbole, Catégorie catégorie) =>
        Existe(symbole) && associationSymboles[symbole].Contains(catégorie);

    bool Existe(char symbole) =>
        associationSymboles.ContainsKey(symbole);
}
