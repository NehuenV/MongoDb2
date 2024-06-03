using BD2.Common.Entities;
using Bogus;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB2.Service.Implementation
{
    public class Builder
    {
        public Builder()
        {

        }
        public static async Task<List<Factura>> GetFacturasAsync(int cantidad)
        {
            var listaFacturas = new ConcurrentBag<Factura>();
            var tasks = new List<Task>();

            for (int i = 0; i < cantidad; i++)
            {
                tasks.Add(GenerarFacturaAsync(listaFacturas));
            }

            await Task.WhenAll(tasks);

            return listaFacturas.ToList();
        }

        private static async Task GenerarFacturaAsync(ConcurrentBag<Factura> listaFacturas)
        {
            var faker = new Faker<Factura>()
                .RuleFor(f => f.IdFactura, f => f.IndexFaker)
                .RuleFor(f => f.IdTicket, f => f.Random.Number(1000, 9999))
                .RuleFor(f => f.FechaHora, f => f.Date.Between(DateTime.Now.AddDays(-30), DateTime.Now))
                .RuleFor(f => f.TotalVenta, f => f.Random.Decimal(1, 1000))
                .RuleFor(f => f.FormasDePago, f => Builder.GetFormasDePago())
                .RuleFor(f => f.EmpleadoCaja, f => Builder.GetEmpleados())
                .RuleFor(f => f.EmpleadoVenta, f => Builder.GetEmpleados())
                .RuleFor(f => f.Encargado, f => Builder.GetEmpleados())
                .RuleFor(f => f.Cliente, f => Builder.GetPersonas())
                .RuleFor(f => f.Sucursal, f => Builder.GetSucursales())
                .RuleFor(f => f.DetalleFactura, f => Builder.GetDetallesFactura());

            await Task.Run(() =>
            {
                listaFacturas.Add(faker.Generate());
            });
        }
        public static ObraSocial GetObraSocial()
        {
            var obraSociales = new List<string> { "OSDE", "Swiss Medical", "Medifé", "IOMA", "Galeno" };

            return new Faker<ObraSocial>()
                .RuleFor(o => o.Id, f => f.IndexFaker)
                .RuleFor(o => o.Nombre, f => f.PickRandom(obraSociales));
        }
        public static Localidad GetLocalidad()
        {
            var localidades = new List<string> { "Temperley", "Lomas de zamora", "Adrogue", "Banfield", "Remedios de escalada","Lanus","Gerli" ,"Avellaneda"};

            return new Faker<Localidad>()
                .RuleFor(o => o.Id, f => f.IndexFaker)
                .RuleFor(o => o.Nombre, f => f.PickRandom(localidades));
        }
        public static Provincia GetProvincias()
        {
            var provincias = new List<string>
        {
            "Buenos Aires", "Catamarca", "Chaco", "Chubut", "Córdoba", "Corrientes", "Entre Ríos",
            "Formosa", "Jujuy", "La Pampa", "La Rioja", "Mendoza", "Misiones", "Neuquén", "Río Negro",
            "Salta", "San Juan", "San Luis", "Santa Cruz", "Santa Fe", "Santiago del Estero", "Tierra del Fuego", "Tucumán"
        };

            return new Faker<Provincia>()
                .RuleFor(o => o.Id, f => f.IndexFaker)
                .RuleFor(o => o.Nombre, f => f.PickRandom(provincias));
        }
        public static TipoProducto GetTiposProducto()
        {
            var productos = new List<string>
        {
            "Desodorante",
            "Crema Corporal",
            "Champú",
            "Acondicionador",
            "Colonia",
            "Paracetamol",
            "Ibuprofeno",
            "Amoxicilina",
            "Omeprazol",
            "Loratadina"
        };

            return new Faker<TipoProducto>()
                .RuleFor(tp => tp.Id, f => f.IndexFaker)
                .RuleFor(tp => tp.Nombre, f => f.PickRandom(productos));
        }
        public static Laboratorio GetLaboratorios()
        {
            var laboratorios = new List<string>
            {
                "Pfizer",
                "Roche",
                "Novartis",
                "Sanofi",
                "GlaxoSmithKline",
                "Bayer",
                "AstraZeneca",
                "Merck",
                "AbbVie",
                "Johnson & Johnson"
            };

            var faker = new Faker<Laboratorio>()
                .RuleFor(l => l.Id, f => f.IndexFaker)
                .RuleFor(l => l.Nombre, f => f.PickRandom(laboratorios));
            return faker.Generate();
        }
        public static Domicilio GetDomicilio()
        {
            var faker = new Faker<Domicilio>()
                .RuleFor(p => p.Provincia, f => Builder.GetProvincias())
                .RuleFor(p => p.Localidad, f => Builder.GetLocalidad())
                .RuleFor(p => p.Calle, f => f.Lorem.Word())
                .RuleFor(p => p.Numero, f => f.Random.Number(300))
                .RuleFor(p => p.Id, f => f.IndexFaker);
            return faker.Generate();
        }
        public static Persona GetPersonas()
        {
            var faker = new Faker<Persona>()
                .RuleFor(p => p.IdPersona, f => f.IndexFaker)
                .RuleFor(p => p.Nombre, f => f.Name.FirstName())
                .RuleFor(p => p.Apellido, f => f.Name.LastName())
                .RuleFor(p => p.Dni, f => f.Random.Int(10000000, 99999999))
                .RuleFor(p => p.NumeroAfiliado, f => f.Random.Int(100000, 999999))
                .RuleFor(p => p.Domicilio, f => Builder.GetDomicilio())
                .RuleFor(p => p.ObraSocial, f => Builder.GetObraSocial());
            return faker.Generate();
        }

        public static FormasDePago GetFormasDePago()
        {
            var formasDePago = new List<string>
            {
                "Efectivo",
                "Tarjeta de Crédito",
                "Tarjeta de Débito",
                "Transferencia Bancaria",
                "Cheque",
                "PayPal",
                "MercadoPago",
                "Crédito",
                "Depósito",
                "Billetera Electrónica"
            };

            var faker = new Faker<FormasDePago>()
                .RuleFor(f => f.Id, f => f.IndexFaker)
                .RuleFor(f => f.Nombre, f => f.PickRandom(formasDePago));

            return faker.Generate();
        }

        public static Sucursal GetSucursales()
        {
            var faker = new Faker<Sucursal>()
                .RuleFor(s => s.IdSucursal, f => f.IndexFaker)
                .RuleFor(s => s.Calle, f => f.Address.StreetName())
                .RuleFor(s => s.Localidad, f => Builder.GetLocalidad())
                .RuleFor(s => s.Altura, f => Convert.ToInt32(f.Address.BuildingNumber()))
                .RuleFor(s => s.NumeroSucursal, f => f.Random.Number(1, 5));
            return faker.Generate();
        }
        public static Producto GetProductos()
        {
            var faker = new Faker<Producto>()
                .RuleFor(p => p.IdProducto, f => f.IndexFaker)
                .RuleFor(p => p.TipoProducto, f => Builder.GetTiposProducto())
                .RuleFor(p => p.Nombre, f => f.Commerce.ProductName())
                .RuleFor(p => p.Descripcion, f => f.Commerce.ProductAdjective())
                .RuleFor(p => p.Laboratorio, f => Builder.GetLaboratorios())
                .RuleFor(p => p.CodigoNumerico, f => f.Random.Number(1000, 9999))
                .RuleFor(p => p.Precio, f => f.Random.Decimal(1, 1000));

            return faker.Generate();
        }

        public static Empleado GetEmpleados()
        {
            var faker = new Faker<Empleado>()
                .RuleFor(e => e.Cuil, f => f.Random.Number(100000000, 999999999))
                .RuleFor(e => e.Encargado, f => f.Random.Bool())
                .RuleFor(e => e.Persona, f => Builder.GetPersonas())
                .RuleFor(e => e.Sucursal, f => Builder.GetSucursales());

           return faker.Generate();
        }

        public static DetalleFactura GetDetallesFactura()
        {
            var faker = new Faker<DetalleFactura>()
                .RuleFor(df => df.IdDetalleFactura, f => f.IndexFaker)
                .RuleFor(df => df.Cantidad, f => f.Random.Number(1, 10))
                .RuleFor(df => df.PrecioLista, f => f.Random.Decimal(1, 100))
                .RuleFor(df => df.PrecioVenta, f => f.Random.Decimal(1, 100))
                .RuleFor(df => df.Producto, f => Builder.GetProductos());

            return faker.Generate();
        }


    }
}
