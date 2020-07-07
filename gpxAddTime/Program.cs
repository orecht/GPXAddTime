using System;
using System.IO;
using Gpx;

namespace gpxAddTime
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = new FileStream(@"C:\perso\Night ride\Night ride 2nd part.gpx", FileMode.Open);
            var output = new FileStream(@"C:\perso\Night ride\Night ride 2nd part_with_timespamp.gpx", FileMode.Create);
            DateTime startTime = DateTime.Parse("2020-07-06T23:35:28");
            DateTime endTime = DateTime.Parse("2020-07-07T00:35:28");
            double totalPoints = 1379;
            int p = 0;

            using (GpxReader reader = new GpxReader(input))
            {
                using (GpxWriter writer = new GpxWriter(output))
                {
                    while (reader.Read())
                    {
                        switch (reader.ObjectType)
                        {
                            case GpxObjectType.Metadata:
                                writer.WriteMetadata(reader.Metadata);
                                break;
                            case GpxObjectType.WayPoint:
                                writer.WriteWayPoint(reader.WayPoint);
                                break;
                            case GpxObjectType.Route:
                                writer.WriteRoute(reader.Route);
                                break;
                            case GpxObjectType.Track:
                                var track = reader.Track;
                                foreach (var seg in track.Segments)
                                {
                                    foreach (var point in seg.TrackPoints)
                                    {
                                        point.Time = startTime.AddSeconds(p * (endTime - startTime).TotalSeconds / totalPoints);
                                        Console.WriteLine($"time = {point.Time}");
                                        p++;
                                    }
                                }
                                writer.WriteTrack(track);
                                break;
                        }
                    }
                }
            }
        }
    }
}
