using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneSafety
{

    class HeatMap
    {
        private int[,] mHeatMap;
        private float mStartX;
        private float mStartY;
        private int mDecimalPlaces;
        private float mResolution;

        public float GetResolution()
        {
            return mResolution;
        }
        public HeatMap(float minX, float maxX, float minY, float maxY, int decimalPlaces)
        {
            float rangeX = maxX - minX;
            float rangeY = maxY - minY;

            mStartX = minX;
            mStartY = minY;

            mResolution = (float)(1 / Math.Pow(10, mDecimalPlaces));

            int width = (int)(rangeX * Math.Pow(10, mDecimalPlaces));
            int height = (int)(rangeY * Math.Pow(10, mDecimalPlaces));

            mHeatMap = new int[width, height];
            mDecimalPlaces = decimalPlaces;
        }

        private float[] indexToGPS(int x, int y)
        {
            float lat = (float)Math.Round(mStartX + mResolution * x, mDecimalPlaces);
            float lon = (float)Math.Round(mStartY + mResolution * y, mDecimalPlaces);
            return new float[] { lat, lon };
        }

        private int[] GPSToIndex(float x, float y)
        {
            x = (float)Math.Round(x, mDecimalPlaces);
            y = (float)Math.Round(x, mDecimalPlaces);
            return new int[] { (int)((x - mStartX) / mResolution), (int)((y - mStartY) / mResolution) };
        }

        public void AddHazard(float x, float y, int v)
        {
            int[] index = GPSToIndex(x, y);
            mHeatMap[index[0], index[1]] += v;
        }

        public string GetJsonConversion()
        {
            var list = new List<dynamic>();
            for (int i = 0; i < mHeatMap.GetLength(0); i++)
                for (int j = 0; j < mHeatMap.GetLength(1); j++)
                {
                    float[] GPS = indexToGPS(i, j);
                    dynamic jsonObject = new
                    {
                        x = GPS[0],
                        y = GPS[1],
                        value = mHeatMap[i, j]
                    };
                    list.Add(jsonObject);
                }
            return JsonConvert.SerializeObject(list);
        }



    }
}