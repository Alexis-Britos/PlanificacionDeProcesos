
public class TramoGantt
{
    public string Proceso { get; set; } = "";
    public int Inicio { get; set; }
    public int Fin { get; set; }
}

public class ResultadoSimulacion
{
    public string Algoritmo { get; set; } = "";
    public List<PCB> Procesos { get; set; } = new();
    public List<TramoGantt> Gantt { get; set; } = new();
    public List<string> OrdenDespacho { get; set; } = new();

    public double EsperaPromedio =>
        Procesos.Average(p =>
            p.TiempoFinalizacion - p.TiempoLlegada - p.TiempoRafaga);

    public double RetornoPromedio =>
        Procesos.Average(p =>
            p.TiempoFinalizacion - p.TiempoLlegada);
}
