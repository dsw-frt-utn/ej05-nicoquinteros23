using Dsw2026Ej5.Data;
using Dsw2026Ej5.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dsw2026Ej5.Views;

public class Controlador
{
    public static List<VehiculoViewModel> GetVehiculos()
    {
        List<VehiculoViewModel> vehiculos = new List<VehiculoViewModel>();
        foreach (Vehiculo vehiculo in Persistencia.GetVehiculos())
        {
            vehiculos.Add(new VehiculoViewModel(vehiculo));
        }
        return vehiculos;
    }

    public static (double, double) CalcularConsumos(Dictionary<string, double> vehiculos)
    {
        double consumoElectricos = 0;
        double consumoCombustible = 0;
        foreach (KeyValuePair<string, double> entry in vehiculos)
        {
            double consumo = 0;
            Vehiculo? vehiculo = Persistencia.GetVehiculo(entry.Key);
            if (vehiculo != null)
            {
                consumo = vehiculo.CalcularConsumo(entry.Value);
                consumoElectricos += vehiculo.EsDe(VehiculoTipo.Electrico) ? consumo : 0;
                consumoCombustible += vehiculo.EsDe(VehiculoTipo.Combustible) ? consumo : 0;
            }
        }
        return (consumoElectricos, consumoCombustible);
    }

    public static List<string> GetCodigosSucursales()
    {
        return Persistencia.GetSucursales().Select(s => s.GetCodigo()).ToList();
    }

    public static (bool, string) AgregarVehiculo(
        string patente,
        string marca,
        string modelo,
        int anio,
        double capacidadCarga,
        string codigoSucursal,
        VehiculoTipo tipo,
        double? kwhBase,
        double? kilometrosPorLitro,
        double? litrosExtra)
    {
        if (Persistencia.GetVehiculo(patente) != null)
        {
            return (false, "Ya existe un vehículo con esa patente.");
        }

        Sucursal? sucursal = Persistencia.GetSucursal(codigoSucursal);
        if (sucursal == null)
        {
            return (false, "La sucursal ingresada no existe.");
        }

        Vehiculo vehiculo;
        if (tipo == VehiculoTipo.Electrico)
        {
            if (kwhBase == null)
            {
                return (false, "Debe ingresar kWh base para un vehículo eléctrico.");
            }
            vehiculo = new VehiculoElectrico(patente, marca, modelo, anio, capacidadCarga, sucursal, kwhBase.Value);
        }
        else
        {
            if (kilometrosPorLitro == null || litrosExtra == null)
            {
                return (false, "Debe ingresar kilómetros por litro y litros extra para un vehículo de combustible.");
            }
            vehiculo = new VehiculoCombustible(
                patente,
                marca,
                modelo,
                anio,
                capacidadCarga,
                sucursal,
                kilometrosPorLitro.Value,
                litrosExtra.Value);
        }

        Persistencia.AgregarVehiculo(vehiculo);
        return (true, "Vehículo agregado correctamente.");
    }
}
