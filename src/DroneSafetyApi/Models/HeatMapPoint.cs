namespace DroneSafetyApi.Models
{
    /// <summary>
    /// The HeatmapPoint class is a model to hold the state of heatmap points that are to be sent to the web application
    /// </summary>
    public class HeatmapPoint
    {
        /// <summary>
        /// The X property represents the Longitude coordinate of the heatmap point
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// The XY property represents the Latitude coordinate of the heatmap point
        /// </summary>
        public double Y { get; set; }
        /// <summary>
        /// The Value property represents the hazardous level of the heatmap point
        /// </summary>
        public int Value { get; set; }
    }
}
