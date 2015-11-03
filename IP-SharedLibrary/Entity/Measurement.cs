using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_SharedLibrary.Entity
{
    public class Measurement
    {
        public int Pulse {get; private set;}
        public int PedalRpm { get; private set; }
        public int Speed { get; private set; }
        public int Distance { get; private set; }
        public int DestPower { get; private set; } // requested power
        public int Energy { get; private set; }
        public DateTime Time { get; private set; }
        public int RealPower { get; private set; } // actual power

        public Measurement(int pulse, int pedalRpm, int speed, int distance, int destPower, int energy, DateTime time, int realPower)
        {
            Pulse = pulse;
            PedalRpm = pedalRpm;
            Speed = speed;
            Distance = distance;
            DestPower = destPower;
            Energy = energy;
            Time = time;
            RealPower = realPower;
        }

        public Measurement(string dataString)
        {
            string[] parts = dataString.Split('\t');
            Pulse = int.Parse(parts[0]);
            PedalRpm = int.Parse(parts[1]);
            Speed = int.Parse(parts[2]);
            Distance = int.Parse(parts[3]);
            DestPower = int.Parse(parts[4]);
            Energy = int.Parse(parts[5]);
            Time = DateTime.Parse(parts[6]);
            RealPower = int.Parse(parts[7]);
        }

        public override string ToString()
        {
            return new StringBuilder()
                .Append("pulse: ")
                .Append(Pulse)
                .Append(",\tpedal rpm: ")
                .Append(PedalRpm)
                .Append(",\tspeed: ")
                .Append(Speed)
                .Append(",\tdistance: ")
                .Append(Distance).Append(",\tdest. power: ")
                .Append(DestPower)
                .Append(",\tenergy: ")
                .Append(Energy)
                .Append(",\ttime: ")
                .Append(Time.ToShortTimeString())
                .Append(",\treal power: ")
                .Append(RealPower)
                .ToString();
        }
    }
}
