using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMeans
{
    public class ElementData
    {   
        public static ElementData[] FromDataTable(DataTable dataTable, string nameLabel,  params string[] columns)
        {
            ElementData[] result = new ElementData[dataTable.Rows.Count];
            for(int i = 0; i < dataTable.Rows.Count; i++)
            {
                var data = new float[columns.Length];
                var row = dataTable.Rows[i];

                for (int o = 0; o < columns.Length; o++)
                    data[o] = float.Parse((string)row[columns[o]]);
                
                result[i] = new ElementData(data, (string)row[nameLabel]);
            }
            return result;
        }

        public string Identification { get; private set; }
        public int Length { get => Data is null ? 0 : Data.Length; }
        public float[] Data { get; private set; }
        public ElementData(float[] data) {
            Data = data;
        }

        public ElementData(float[] data, string name) : this(data)
        {
            Identification = name;
        }

        public ElementData(int length)
        {
            Data = new float[length];
        }


        public float DistanceTo(ElementData data)
        {
            if (Length != data.Length) throw new Exception("Tamanhos de dados desiguais!");
            float dist = 0;
            for(int i = 0; i < Length; i++)
            {
                var diff = Data[i] - data.Data[i];
                dist += diff * diff;
            }
            return dist; 
        }

    }
}
