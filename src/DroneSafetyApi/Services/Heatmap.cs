﻿using DroneSafetyApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TODO: Change namespace
namespace DroneSafetyApi.Services
{

    public class HeatMap
    {
        private int[,] mHeatMap;
        public double mStartX { get; set; }
        public double mEndX { get; set; }
        public double mStartY { get; set; }
        public double mEndY { get; set; }
        private int mDecimalPlaces;
        private double mResolution;

        public double GetResolution()
        {
            return mResolution;
        }
        public HeatMap(double minX, double maxX, double minY, double maxY, int decimalPlaces)
        {
            double rangeX = maxX - minX;
            double rangeY = maxY - minY;

            mStartX = minX;
            mEndX = maxX;
            mStartY = minY;
            mEndY = maxY;
            mDecimalPlaces = decimalPlaces;
            mResolution = (double)(1 / Math.Pow(10, mDecimalPlaces));

            int width = (int)(rangeX * Math.Pow(10, mDecimalPlaces));
            int height = (int)(rangeY * Math.Pow(10, mDecimalPlaces));

            mHeatMap = new int[width, height];
            
        }

        public double[] indexToGPS(int x, int y)
        {
            double lat = (double)Math.Round(mStartX + mResolution * x, mDecimalPlaces);
            double lon = (double)Math.Round(mStartY + mResolution * y, mDecimalPlaces);
            return new double[] { lat, lon };
        }

        public int[] GPSToIndex(double x, double y)
        {
            x = (double)Math.Round(x, mDecimalPlaces);
            y = (double)Math.Round(y, mDecimalPlaces);
            return new int[] { (int)((x - mStartX) / mResolution), (int)((y - mStartY) / mResolution) };
        }

        public void AddHazard(double x, double y, int v)
        {
            int[] index = GPSToIndex(x, y);
            if (index[0] < 0 || index[1] < 0 || index[0] >= mHeatMap.GetLength(0) || index[1] >= mHeatMap.GetLength(1))
                return;
            mHeatMap[index[0], index[1]] += v;
        }

        public IEnumerable<Models.HeatmapPoint> GetHeatMapPoints()
        {
            List<HeatmapPoint> list = new List<HeatmapPoint>();
            for (int i = 0; i < mHeatMap.GetLength(0); i++)
                for (int j = 0; j < mHeatMap.GetLength(1); j++)
                {
                    if (mHeatMap[i, j] > 0)
                    {
                        double[] GPS = indexToGPS(i, j);
                        HeatmapPoint point = new HeatmapPoint();
                        point.Y = GPS[0];
                        point.X = GPS[1];
                        point.Value = mHeatMap[i, j];
                        list.Add(point);
                    }
                }
            return list;
        }
    }
}