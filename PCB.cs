
public enum EstadoProceso {
    READY,
    RUNNING,
    TERMINATED
}

public class PCB {
    public int PID { get; set; }
    public string Nombre { get; set; } = "";
    public int Prioridad { get; set; }
    public int TiempoLlegada { get; set; }
    public int TiempoRafaga { get; set; }
    public EstadoProceso Estado { get; set; } = EstadoProceso.READY;

    public int TiempoRestante { get; set; }
    public int TiempoFinalizacion { get; set; }

    public PCB Copiar()
    {
        return new PCB
        {
            PID = this.PID,
            Nombre = this.Nombre,
            Prioridad = this.Prioridad,
            TiempoLlegada = this.TiempoLlegada,
            TiempoRafaga = this.TiempoRafaga,
            Estado = EstadoProceso.READY,
            TiempoRestante = this.TiempoRafaga
        };
    }
}
