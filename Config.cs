// Alex Bedard
// TP03

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TP02_Affichage;

namespace TP03;

public static class Config
{
    static public ConfigInfo? LireConfig(string nomFichier)
    {
        string jsonString = LireFichier(nomFichier);
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };
        return JsonSerializer.Deserialize<ConfigInfo>(jsonString, options);
    }
    static public string LireFichier(string nomFichier)
    {
        StreamReader reader = new(nomFichier);
        string contenuFichier = reader.ReadToEnd();
        reader.Close();
        return contenuFichier;
    }
}

public class ConfigInfo
{
    public InfoSurface Surface { get; set; }

    public List<InfoRobot> Robot { get; set; }

    public List<CatégorieDéchet> CatégorieDéchet { get; set; }
}

public class InfoSurface
{
    public int Hauteur { get; set; }
    public int Largeur { get; set; }
}
public class CatégorieDéchet
{
    public int NbDéchetsParCat { get; set; }
    public char Symbole { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Catégorie Catégorie { get; set; }
}
public class InfoRobot
{
    public string Nom { get; set; }
    public char Symbole { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ConsoleColor Couleur { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Catégorie Catégorie { get; set; }
}
