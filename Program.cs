
using System;
using System.Collections.Generic;
using System.Linq;

Console.WriteLine("=== SIMULADOR DE PLANIFICACION DE PROCESOS ===");

int cantidad = LeerEntero("Cantidad de procesos: ", 1);

List<PCB> procesos = new List<PCB>();

for (int i = 1; i <= cantidad; i++) {
    Console.WriteLine($"\n--- Proceso {i} ---");

    Console.Write("Nombre: ");
    string nombre = Console.ReadLine() ?? "";

    if (string.IsNullOrWhiteSpace(nombre))
        nombre = $"P{i}";

    int prioridad = LeerEntero("Prioridad: ", 0);
    int llegada = LeerEntero("Tiempo de llegada: ", 0);
    int rafaga = LeerEntero("Tiempo de rafaga: ", 1);

    procesos.Add(new PCB
    {
        PID = i,
        Nombre = nombre,
        Prioridad = prioridad,
        TiempoLlegada = llegada,
        TiempoRafaga = rafaga,
        TiempoRestante = rafaga,
        Estado = EstadoProceso.READY
    });
}

Console.WriteLine();
int quantum = LeerEntero("Quantum para Round Robin: ", 1);

ResultadoSimulacion fcfs = FCFS.Ejecutar(procesos);
ResultadoSimulacion rr = RoundRobin.Ejecutar(procesos, quantum);

Console.WriteLine("\n=== RESULTADOS FCFS ===");
MostrarResultado(fcfs);

Console.WriteLine("\n=== RESULTADOS ROUND ROBIN ===");
MostrarResultado(rr);

Console.WriteLine("\n=== TABLA COMPARATIVA ===");
Console.WriteLine(
    $"{"Algoritmo",-22} {"Espera prom.",-15} {"Retorno prom.",-16} Orden de despacho");

MostrarComparacion(fcfs);
MostrarComparacion(rr);


static int LeerEntero(string mensaje, int minimo)
{
    while (true)
    {
        Console.Write(mensaje);

        if (int.TryParse(Console.ReadLine(), out int numero)
            && numero >= minimo)
        {
            return numero;
        }

        Console.WriteLine($"Ingrese un numero entero mayor o igual a {minimo}.");
    }
}

static void MostrarResultado(ResultadoSimulacion resultado)
{
    Console.WriteLine(
        $"{"PID",-6} {"Nombre",-12} {"Llegada",-9} {"Rafaga",-9} {"Espera",-9} {"Retorno",-9} {"Estado",-12}");

    foreach (PCB p in resultado.Procesos.OrderBy(p => p.PID))
    {
        int retorno = p.TiempoFinalizacion - p.TiempoLlegada;
        int espera = retorno - p.TiempoRafaga;

        Console.WriteLine(
            $"{p.PID,-6} {p.Nombre,-12} {p.TiempoLlegada,-9} " +
            $"{p.TiempoRafaga,-9} {espera,-9} {retorno,-9} {p.Estado,-12}");
    }

    Console.WriteLine($"\nEspera promedio: {resultado.EsperaPromedio:F2}");
    Console.WriteLine($"Retorno promedio: {resultado.RetornoPromedio:F2}");

    Console.WriteLine(
        "Orden de despacho: " +
        string.Join(" -> ", resultado.OrdenDespacho));

    Console.WriteLine("\nDiagrama de Gantt:");

    foreach (TramoGantt tramo in resultado.Gantt)
    {
        Console.Write($"| {tramo.Proceso} ");
    }

    Console.WriteLine("|");

    Console.Write(resultado.Gantt[0].Inicio);

    foreach (TramoGantt tramo in resultado.Gantt)
    {
        Console.Write($" -> {tramo.Fin}");
    }

    Console.WriteLine();
}

static void MostrarComparacion(ResultadoSimulacion resultado) {
    Console.WriteLine(
        $"{resultado.Algoritmo,-22} " +
        $"{resultado.EsperaPromedio,-15:F2} " +
        $"{resultado.RetornoPromedio,-16:F2} " +
        string.Join(" -> ", resultado.OrdenDespacho));
}
