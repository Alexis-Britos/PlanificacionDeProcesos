
public static class RoundRobin
{
    public static ResultadoSimulacion Ejecutar(
        List<PCB> procesos, int quantum)
    {
        if (quantum <= 0)
            throw new ArgumentException("El quantum debe ser mayor que cero.");

        var resultado = new ResultadoSimulacion
        {
            Algoritmo = $"Round Robin (q={quantum})",
            Procesos = procesos.Select(p => p.Copiar()).ToList()
        };

        var pendientes = resultado.Procesos
            .OrderBy(p => p.TiempoLlegada)
            .ThenBy(p => p.PID)
            .ToList();

        Queue<PCB> colaReady = new Queue<PCB>();

        int tiempo = 0;
        int siguiente = 0;
        int terminados = 0;

        void AgregarLlegadas()
        {
            while (siguiente < pendientes.Count &&
                   pendientes[siguiente].TiempoLlegada <= tiempo)
            {
                colaReady.Enqueue(pendientes[siguiente]);
                siguiente++;
            }
        }

        while (terminados < pendientes.Count)
        {
            AgregarLlegadas();

            if (colaReady.Count == 0)
            {
                int proximaLlegada = pendientes[siguiente].TiempoLlegada;

                resultado.Gantt.Add(new TramoGantt
                {
                    Proceso = "IDLE",
                    Inicio = tiempo,
                    Fin = proximaLlegada
                });

                tiempo = proximaLlegada;
                continue;
            }

            PCB proceso = colaReady.Dequeue();
            proceso.Estado = EstadoProceso.RUNNING;

            int inicio = tiempo;
            int ejecucion = Math.Min(quantum, proceso.TiempoRestante);

            tiempo += ejecucion;
            proceso.TiempoRestante -= ejecucion;

            resultado.OrdenDespacho.Add(proceso.Nombre);

            resultado.Gantt.Add(new TramoGantt
            {
                Proceso = proceso.Nombre,
                Inicio = inicio,
                Fin = tiempo
            });

            AgregarLlegadas();

            if (proceso.TiempoRestante == 0)
            {
                proceso.TiempoFinalizacion = tiempo;
                proceso.Estado = EstadoProceso.TERMINATED;
                terminados++;
            }
            else
            {
                proceso.Estado = EstadoProceso.READY;
                colaReady.Enqueue(proceso);
            }
        }

        return resultado;
    }
}
