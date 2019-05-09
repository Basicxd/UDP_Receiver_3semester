namespace UDP_Receiver
{
    public class Sensor
    {
        public string Motion { get; set; }
        public string Dato { get; set; }
        public string Tid { get; set; }

        public Sensor(string dato, string tid, string motion)
        {
            Motion = motion;
            Dato = dato;
            Tid = tid;
        }

        public override string ToString()
        {
            return $"{nameof(Dato)}: {Dato}, {nameof(Tid)}: {Tid}, {nameof(Motion)}: {Motion}";
        }

        public Sensor() { }


    }
}