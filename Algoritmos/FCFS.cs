
public static class FCFS
{
    public static ResultadoSimulacion Ejecutar(List<PCB> procesos)
    {
        var resultado = new ResultadoSimulacion
        {
            Algoritmo = "FCFS",
            Procesos = procesos.Select(p => p.Copiar()).ToList()
        };

        Queue<PCB> colaReady = new Queue<PCB>(
            resultado.Procesos
                .OrderBy(p => p.TiempoLlegada)
                .ThenBy(p => p.PID)
        );

        int tiempo = 0;

        while (colaReady.Count > 0)
        {
            PCB proceso = colaReady.Dequeue();

            if (tiempo < proceso.TiempoLlegada)
            {
                resultado.Gantt.Add(new TramoGantt
                {
                    Proceso = "IDLE",
                    Inicio = tiempo,
                    Fin = proceso.TiempoLlegada
                });

                tiempo = proceso.TiempoLlegada;
            }

            proceso.Estado = EstadoProceso.RUNNING;

            int inicio = tiempo;
            tiempo += proceso.TiempoRafaga;

            proceso.TiempoRestante = 0;
            proceso.TiempoFinalizacion = tiempo;
            proceso.Estado = EstadoProceso.TERMINATED;

            resultado.OrdenDespacho.Add(proceso.Nombre);

            resultado.Gantt.Add(new TramoGantt
            {
                Proceso = proceso.Nombre,
                Inicio = inicio,
                Fin = tiempo
            });
        }

        return resultado;
    }
}
