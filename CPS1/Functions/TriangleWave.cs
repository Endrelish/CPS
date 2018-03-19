namespace CPS1.Functions
{
    using System;

    public class TriangleWave : Function
    {
        public TriangleWave()
        {
            this.SignalFunction = (A, T, t1, kw, t) => { return 0; }; // TODO TU NIE MA JESZCZE SYGNALU!!!!!!
        }

        public static Required RequiredAttributes { get; set; } = new Required(true, true, true, true, true, true, true, false);
       
    }
}