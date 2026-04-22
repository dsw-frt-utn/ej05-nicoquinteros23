using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej5.Domain;

public class VehiculoCombustible: Vehiculo
{
    private double kilometrosPorLitro;
    private double litrosExtra;

    public VehiculoCombustible(string patente, string marca, string modelo, int anio, double capacidadCarga, 
        Sucursal sucursal, double kilometrosPorLitro, double litrosExtra) : base(VehiculoTipo.Combustible, patente, marca, modelo, anio, capacidadCarga, sucursal)
    {
        this.kilometrosPorLitro = kilometrosPorLitro;
        this.litrosExtra = litrosExtra;
    }

    public double GetKilometrosPorLitro()
    {
        return kilometrosPorLitro;
    }

    public double GetLitrosExtra()
    {
        return litrosExtra;
    }

    //calculo consumo fixeada
    public override double CalcularConsumo(double kilometros)
    {
        double consumo = kilometros / kilometrosPorLitro;
        int antiguedad = DateTime.Now.Year - GetAnio();

        if (antiguedad > 5)
        {
            consumo += Math.Floor(kilometros / 15) * litrosExtra;
        }

        return consumo;
    }
}
