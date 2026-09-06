// Alex Bedard
// TP03

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP03;

public class PortInexistantException : Exception { }

internal class CanalComm<T> 
{
    public int NbPorts { get; }
    Dictionary<int, ZoneTransit<T>> Ports { get; } = new();

    public CanalComm(int nbPorts)
    {
        NbPorts = nbPorts;

        lock (Ports)
        {
            for (int i = 0; i < NbPorts; i++)
                Ports.Add(i, new ZoneTransit<T>());
        }
    }

    public ZoneTransit<T> ObtenirPort(int numeroPort)
    {
        lock (Ports)
        {
            return Ports.ContainsKey(numeroPort) ? Ports[numeroPort] :
                throw new PortInexistantException();
        }
    }

    public void PublierSur(int numeroPort, T pipelineInfo)
    {
        lock (Ports)
        {
            if (Ports.ContainsKey(numeroPort))
            { 
                List<T> lst = new();
                lst.Add(pipelineInfo);
                ObtenirPort(numeroPort).Ajouter(lst);
            }
            else
                throw new PortInexistantException();
        }
    }

    public List<T> LireSur(int numeroPort)
    {
        lock (Ports)
        {
            return ObtenirPort(numeroPort).Extraire();
        }
    }

    public List<T> Balayer()
    {
        List<T> lst = new();

        lock (Ports)
        {

            for (int i = 0; i < NbPorts; i++)
            {
                List<T> pipelineInfo = ObtenirPort(i).Extraire();

                if (pipelineInfo.Count != 0)
                    lst.Add(pipelineInfo.Last());
            }

            return lst;
        }
    }

    public void Vider()
    {
        lock (Ports)
        {
            for (int i = 0; i < Ports.Count; i++)
            {
                ObtenirPort(i).Vider();
            }
        }
    }
}
