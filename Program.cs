using KMeans;
using CsvHelper;
using System.Security.Cryptography.X509Certificates;
using CsvHelper.Configuration;
using System.Text;
using System.Data;

// pegas os dados
// define N cluster
// seleciona N aleatorios
// calcula a distancia dos pontos pra N pontos
// classifica
// recalcula N cluster com a média de cada cluster
// classifica

var csvConfig = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture) {
    Delimiter = ",",
    Encoding = Encoding.UTF8,
};

using var file = File.OpenRead(@"C:\Users\Daniel Curi\Downloads\Iris.csv");
using var fileReader = new StreamReader(file);
using CsvReader reader = new CsvReader(fileReader, csvConfig);
CsvDataReader dataReader = new CsvDataReader(reader);

var dt = new DataTable();
dt.Load(dataReader);


int N = 3;
int iterations = 1000;

ElementData[] dados = ElementData.FromDataTable(dt, "Species", "SepalLengthCm", "SepalWidthCm", "PetalLengthCm", "PetalWidthCm");

var clusters = GetRandomClusters(N);

IterateClassification(iterations);

foreach(var c in clusters)
{
    Console.WriteLine($"Cluster dominant specie: {c.GetDominantIdentification()}");
    Console.WriteLine($"Cluster size: {c.DataPoints.Count}");
    Console.WriteLine($"Cluster Identifications: {string.Join(", ", c.DataPoints.Select(x => x.Identification).Distinct())}");
    Console.WriteLine();
}


Cluster[] GetRandomClusters(int clusterCount)
{
    var dataLength = dados.FirstOrDefault().Length;
    var indices = Enumerable.Range(0, dados.Length).OrderBy(Random.Shared.Next).TakeLast(clusterCount).ToArray();
    Cluster[] cl = new Cluster[clusterCount];
    for (int i = 0; i < clusterCount; i++)
    {
        cl[i] = new Cluster(dados[indices[i]]);
        //dados[indices[i]].CopyTo(cl[i]);
    }
    return cl;
}

void PopulateClusters(Cluster[] toPopulate, ElementData[] dados)
{
    foreach(var data in dados)
    {
        var closetsDist = float.MaxValue;
        Cluster closestCluster = null;
        foreach(var cluster in toPopulate)
        {
            var dist = cluster.DistanceTo(data);
            if (dist < closetsDist)
            {
                closetsDist = dist;
                closestCluster = cluster;
            }
        }

        closestCluster?.Add(data);
    }
}

Cluster[] GetNewClusters(Cluster[] oldClusters)
{
    Cluster[] newClusters = new Cluster[oldClusters.Length];
    for (int i = 0; i < oldClusters.Length; i++)
        newClusters[i] = new Cluster(oldClusters[i].GetAverage());
    return newClusters;
}

void IterateClassification(int iterations)
{
    for(int i = 0; i < iterations; i++)
    {
        PopulateClusters(clusters, dados);
        clusters = GetNewClusters(clusters);
    }
    PopulateClusters(clusters, dados);
}