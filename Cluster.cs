using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMeans
{
    public class Cluster
    {
        public List<ElementData> DataPoints;
        public ElementData Center;

        public Cluster(ElementData center) {
            DataPoints = new();
            Center = center;
        }

        public void Add(ElementData dado)
        {
            DataPoints.Add(dado);
        }

        public float DistanceTo(ElementData data)
        {
            return Center.DistanceTo(data);
        }

        public ElementData GetAverage()
        {
            float[] avg = new float[Center.Length];
            var totalPoints = DataPoints.Count;

            for(int i = 0; i < Center.Length; i++)
            {
                float sum = 0;
                foreach (var data in DataPoints)
                    sum += data.Data[i];

                avg[i] = sum / totalPoints;
            }
            
            return new ElementData(avg);
        }

        public string GetDominantIdentification()
        {
            Dictionary<string, int> valuePairs = new();

            foreach(var element in DataPoints)
                if (!valuePairs.TryAdd(element.Identification, 1))
                    valuePairs[element.Identification]++;

            return valuePairs.Count > 0 ? valuePairs.MaxBy(x => x.Value).Key : "Vazio";
        }
    }
}
