// Alex Bedard
// TP03

using TP02_Affichage;

namespace TP03;

internal class Messagerie
{
    public Point2D PointRéférence { get; }
    public Messagerie(Point2D pos) { PointRéférence = pos; }
    List<(string, ConsoleColor)> Messages { get; } = new();
    Point2D Pos { get; }
    int Hauteur { get; set; }
    int Largeur { get; set; }
    int MaxHauteur { get; }

    public Messagerie(Point2D point, int maxHauteur) 
    {
        Hauteur = 0;
        Largeur = 0;
        MaxHauteur = maxHauteur;
        PointRéférence = point;
    }

    public void Afficher()
    {
        Effacer();

        List<(string, ConsoleColor)> snapshot;
        int y = PointRéférence.Y;

        lock (Messages) 
        {
            snapshot = new(Messages);
        }

        int largeurAffichage = Algos.Cumuler(snapshot, (maxCourant, message) => 
            Math.Max(maxCourant, message.Item1.Length), Largeur);

        Hauteur = Math.Max(snapshot.Count, MaxHauteur);

        for (int i = 0; i < snapshot.Count; i++)
        {
            ConsoleColor couleurInitiale = Console.ForegroundColor;
            Console.ForegroundColor = snapshot[i].Item2;

            Console.SetCursorPosition(PointRéférence.X, y);
            Console.Write(snapshot[i].Item1);

            if (snapshot[i].Item1.Length > Largeur)
                Largeur = snapshot[i].Item1.Length;

                Console.ForegroundColor = couleurInitiale;
            y++;
        }

        if (snapshot.Count > Largeur)
            Largeur = snapshot.Count;
    }

    public void Effacer()
    {
        int y = PointRéférence.Y;
        string chaineVide = new string(' ', Largeur);

        for (int i = 0; i < Hauteur; i++)
        {
            Console.SetCursorPosition(PointRéférence.X, y);
            Console.Write(chaineVide);

            y++;
        }
    }

    public void Ajouter(ConsoleColor couleur, params string[] messages)
    {
        lock (Messages)
        {
            foreach (var m in messages)
                Messages.Add((m, couleur));

            while (Messages.Count > MaxHauteur)
                Messages.RemoveAt(0);
        }
    }
}
