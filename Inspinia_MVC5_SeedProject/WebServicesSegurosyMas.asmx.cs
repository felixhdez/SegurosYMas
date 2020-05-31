using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Inspinia_MVC5_SeedProject.Models;
using System.Data.Entity;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Inspinia_MVC5_SeedProject
{
    /// <summary>
    /// Descripción breve de WebServicesSegurosyMas
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class WebServicesSegurosyMas : System.Web.Services.WebService
    {
        SeguroBD db = new SeguroBD();
        SeguridadUserBD dbs = new SeguridadUserBD();
        //Image image = new Image();
        //[WebMethod]
        //public string HelloWorld()
        //{
        //    return "Hola a todos";
        //}

        /*===========================================================================================
                                      WEBMETHOD LOGIN
============================================================================================*/

        [WebMethod]
        public List<Login> Logearse(string usuario, string contraseña)
        {


            var usuar = dbs.Usuarios.Where(x => x.User == usuario && x.Pass == contraseña && x.Estado == true).ToList();

            return (from item in usuar
                    select new Login
                    {
                        Id = item.Id,
                        Rol = item.Rol.Descripcion,
                        IdCliente = item.IdNUser
                    }
                ).ToList();
        }

        [WebMethod]
        public List<LoginWS> EditLogin(int id)
        {
            return dbs.Usuarios.Where(x => x.Id == id)
                .Select(
                x => new LoginWS
                {
                    Id = x.Id,
                    Usuario = x.User,
                    Password = x.Pass
                }
                ).ToList();
        }



        //Guardar Editado Login
        [WebMethod]
        public int GuardarEditLogin(int id, string pass)
        {
            var guardar = dbs.Usuarios.Where(x => x.Id == id).FirstOrDefault();
            guardar.Pass = pass;
            dbs.Entry(guardar).State = EntityState.Modified;
            return dbs.SaveChanges();

        }

        [WebMethod]
        public bool VerificarContra(int id, string pass)
        {
            bool resul = false;
            var l = dbs.Usuarios.Where(x => x.Id == id && x.Pass == pass).FirstOrDefault();
            if (l != null)
                resul = true;

            return resul;
        }



        /*===========================================================================================
                          GUARDAR EL RECLAMO
        ============================================================================================*/
        [WebMethod]
        public int saveReclamo(DateTime Fecha, string Lugar, string Comentario, int idC)
        {
            ReclamosTemp g = new ReclamosTemp();
            g.Fecha = Fecha.ToString("dd/MM/yyyy");
            g.Lugar = Lugar;
            g.Comentarios = Comentario;
            g.UsuarioId = idC;
            dbs.ReclamosTemps.Add(g);
            //AddNotification.AppendNotify(g);
            return dbs.SaveChanges();

        }
        /*===========================================================================================
                                            WEBMETHOD CLIENTE
     ============================================================================================*/

        [WebMethod]
        public List<PolizaCliente> MostrarPolizaCliente(int idC)
        {

            var a = db.Polizas.Where(x => x.ClienteId == idC).ToList();
            return (from item in a

                    select new PolizaCliente
                    {
                        Id = item.Id,
                        NumPolizaC = item.NumPoliza,
                        ProductoAdC = item.Producto.Descripcion,
                        Aseguradora = item.Producto.Aseguradora.Descripcion,
                        Nombre = item.Cliente.Nombres + " " + item.Cliente.Apellidos

                    }).ToList();
        }


        [WebMethod]
        public List<BienesCliente> MostrarBienesCliente(int idC)
        {

            var a = db.DetalleBienesAsegurados.Where(x => x.Poliza.ClienteId == idC).ToList();
            return (from item in a

                    select new BienesCliente
                    {
                        Id = item.Id,
                        NumPolizaC = item.Poliza.NumPoliza,
                        ProductoAdC = item.Poliza.Producto.Descripcion,
                        Aseguradora = item.Poliza.Producto.Aseguradora.Descripcion,
                        Nombre = item.Poliza.Cliente.Nombres + " " + item.Poliza.Cliente.Apellidos

                    }).ToList();
        }
        [WebMethod]
        public List<AdendaCliente> MostrarAdendaCliente(int idC)
        {

            var a = db.Adendas.Where(x => x.Poliza.ClienteId == idC).ToList();
            return (from item in a

                    select new AdendaCliente
                    {
                        Id = item.Id,
                        NumPolizaC = item.Poliza.NumPoliza,
                        NumAdenda = item.NumAdenda,
                        Aseguradora = item.Poliza.Producto.Aseguradora.Descripcion,
                        Nombre = item.Poliza.Cliente.Nombres + " " + item.Poliza.Cliente.Apellidos

                    }).ToList();
        }



        [WebMethod]
        public List<DetallePolizaCliente> MostrarDetallePolizaCliente(int idC, int idPo)
        {

            return db.Polizas.Where(x => x.ClienteId == idC && x.Id == idPo)
                .Select(
                x => new DetallePolizaCliente
                {
                    Id = x.Id,
                    NumPolizaC = x.NumPoliza,
                    NombreC = x.Cliente.Nombres + " " + x.Cliente.Apellidos,
                    Contratante = x.Persona.Nombres + " " + x.Persona.Apellidos,
                    ContactoIntermediario = x.ContactoIntermediario.Nombres + " " + x.ContactoIntermediario.Apellidos,
                    Aseguradora = x.Producto.Aseguradora.Descripcion,
                    Producto = x.Producto.Descripcion,
                    TipoMoneda = x.TipoMoneda,
                    Desde = x.FechaDesde,
                    Hasta = x.FechaHasta,
                    Telefono = x.Producto.Aseguradora.Asistencia
                }
                ).ToList();
        }



        [WebMethod]
        public List<DetalleBienesCliente> MostrarDetalleBienesCliente(int idC, int bienID)
        {

            return db.DetalleBienesAsegurados.Where(x => x.Poliza.ClienteId == idC && x.Id == bienID)
                .Select(
                x => new DetalleBienesCliente
                {
                    Id = x.Id,
                    NumeroPoliza = x.Poliza.NumPoliza,
                    NombreC = x.Poliza.Cliente.Nombres + " " + x.Poliza.Cliente.Apellidos,
                    Producto = x.Poliza.Producto.Descripcion,
                    ASeguradora = x.Poliza.Producto.Aseguradora.Descripcion,
                    NumCertificado = x.BienAsegurado.NumCertificado,
                    Telefono = x.Poliza.Producto.Aseguradora.Asistencia
                }
                ).ToList();
        }


        [WebMethod]
        public List<CaracteristicasBienesCliente> MostrarCaracteriBienesCliente(int idB)
        {
            return db.DetalleCaracteristicas.Where(x => x.BienAseguradoId == idB)
                .Select(
                x => new CaracteristicasBienesCliente

                {
                    Id = x.Id,
                    Caracteristica = x.Caracteristica.Descripcion,
                    Valor = x.Valor

                }
                ).ToList();
        }

        [WebMethod]
        public List<DetalleAdendaCliente> MostrarDetalleAdendaCliente(int idC, int adenid)
        {

            return db.Adendas.Where(x => x.Poliza.ClienteId == idC && x.Id == adenid)
                .Select(
                x => new DetalleAdendaCliente
                {
                    Id = x.Id,
                    NumeroPoliza = x.Poliza.NumPoliza,
                    NombreC = x.Poliza.Cliente.Nombres + " " + x.Poliza.Cliente.Apellidos,
                    NumeroAdenda = x.NumAdenda,
                    TipoAdenda = x.TipoAdenda,
                    Desde = x.FechaDesde,
                    Hasta = x.FechaHasta,
                    Telefono = x.Poliza.Producto.Aseguradora.Asistencia
                }
                ).ToList();
        }

        /*===========================================================================================
                                           WEBMETHOD ADMINISTRATIVO
         ============================================================================================*/



        //--------------------------------------POLIZA--------------------------------------------
        [WebMethod]

        public List<MostrarPoliza> MostrarListaPoliza()
        {

            return db.Polizas
                .Select(
                x => new MostrarPoliza
                {
                    Id = x.Id,
                    NumPoliza = x.NumPoliza,
                    NombreCliente = x.Cliente.Nombres + " " + x.Cliente.Apellidos,
                    producto = x.Producto.Descripcion

                }
                ).ToList();

        }

        [WebMethod]
        public List<DetallePoliza> MostrarDetallePoliza(int id)
        {

            return db.Polizas.Where(x => x.Id == id)
                .Select(
                x => new DetallePoliza
                {
                    Id = x.Id,
                    NumPoliza = x.NumPoliza,
                    NombreCliente = x.Cliente.Nombres + " " + x.Cliente.Apellidos,
                    Contratante = x.Persona.Nombres + " " + x.Persona.Apellidos,
                    ContactoIntermediario = x.ContactoIntermediario.Nombres + " " + x.ContactoIntermediario.Apellidos,
                    Aseguradora = x.Producto.Aseguradora.Descripcion,
                    Producto = x.Producto.Descripcion,
                    TipoMoneda = x.TipoMoneda,
                    FechaEmision = x.FechaEmision,
                    Desde = x.FechaDesde,
                    Hasta = x.FechaHasta
                }
                ).ToList();
        }

        [WebMethod]
        public List<CuotaPoliza> MostrarCuotaPoliza(int idP)
        {
            return db.DetalleCuotas.Where(x => x.Poliza.Id == idP)
                .Select(
                x => new CuotaPoliza
                {
                    Id = x.Id,
                    cuota = x.Cuota.NumCoutas,
                    vence = x.Vence,
                    monto = x.Monto,
                    estado = x.Estado
                }
                ).ToList();
        }

        [WebMethod]
        public List<ImagenPoliza> MostrarArchivosPólizas(int idp)
        {

            var mostra = db.ArchivosPolizas.Where(x => x.PolizaId == idp).ToList();
            var y = (from item in mostra
                     select new ImagenPoliza
                     {
                         Id = item.Id,
                         Imagen = ImageToByteArray(item.Foto),
                         nombre = nombreImagen(item.Foto)

                     }).ToList();
            return y;
        }

        [WebMethod]
        public List<MostrarPoliza> FiltroPoliza(string cadena)
        {
            var dato = db.Polizas.Where(x => x.NumPoliza.StartsWith(cadena) || x.Cliente.Nombres.StartsWith(cadena) || x.Producto.Descripcion.StartsWith(cadena)).ToList();

            var y = (from item in dato
                     select new MostrarPoliza
                     {
                         NumPoliza = item.NumPoliza,
                         NombreCliente = item.Cliente.Nombres,
                         producto = item.Producto.Descripcion
                     }).ToList();
            return y;

        }
        //--------------------------------------Bienes--------------------------------------------
        //[WebMethod]
        [WebMethod]
        public List<MostrarBienes> MostrarListaBienes()
        {

            return db.DetalleBienesAsegurados
                .Select(
                x => new MostrarBienes
                {
                    Id = x.Id,
                    NumPoliza = x.Poliza.NumPoliza,
                    NombreCliente = x.Poliza.Cliente.Nombres + " " + x.Poliza.Cliente.Apellidos
                }
                ).ToList();
        }


        [WebMethod]
        public List<DetalleBienes> MostrarDetalleBienes(int id)
        {

            return db.DetalleBienesAsegurados.Where(x => x.Id == id)
                .Select(
                x => new DetalleBienes
                {
                    Id = x.Id,
                    NumPoliza = x.Poliza.NumPoliza,
                    NombreCliente = x.Poliza.Cliente.Nombres + " " + x.Poliza.Cliente.Apellidos,
                    Producto = x.Poliza.Producto.Descripcion,
                    Aseguradora = x.Poliza.Producto.Aseguradora.Descripcion,
                    NumeroCertificado = x.BienAsegurado.NumCertificado
                }
                ).ToList();
        }

        [WebMethod]
        public List<CaracteristicasBienes> MostrarCaracteriBienes(int id)
        {
            return db.DetalleCaracteristicas.Where(x => x.BienAsegurado.Id == id)
                .Select(
                  x => new CaracteristicasBienes
                  {
                      Id = x.Id,
                      Caracteristica = x.Caracteristica.Descripcion,
                      Valor = x.Valor

                  }
                ).ToList();
        }

        [WebMethod]
        public List<CoberturaBienes> MostrarCoberturaBienes(int id)
        {
            return db.DetalleCoberturas.Where(x => x.BienAsegurados.Id == id)
                .Select(
                  x => new CoberturaBienes
                  {
                      Id = x.Id,
                      Cobertura = x.Coberturas.Descripcion,
                      SumaAsegurada = x.SumaAsegurada,
                      Deducible = x.Deducible,
                      prima = x.Prima

                  }
                ).ToList();
        }
        //--------------------------------------Adendas--------------------------------------------
        //[WebMethod]
        [WebMethod]

        public List<MostrarAdendas> MostrarListaAdendas()
        {

            return db.Adendas
                .Select(
                x => new MostrarAdendas
                {
                    Id = x.Id,
                    NumPoliza = x.Poliza.NumPoliza,
                    NombreCliente = x.Poliza.Cliente.Nombres + " " + x.Poliza.Cliente.Apellidos,
                    NumAdenda = x.NumAdenda

                }
                ).ToList();

        }

        [WebMethod]
        public List<DetalleAdenda> MostrarDetalleAdenda(int id)
        {

            return db.Adendas.Where(x => x.Id == id)
                .Select(
                x => new DetalleAdenda
                {
                    Id = x.Id,
                    NumPoliza = x.Poliza.NumPoliza,
                    NombreCliente = x.Poliza.Cliente.Nombres + " " + x.Poliza.Cliente.Apellidos,
                    NumeroAdenda = x.NumAdenda,
                    TipoAdenda = x.TipoAdenda,
                    FechaEmision = x.FechaEmision,
                    Desde = x.FechaDesde,
                    Hasta = x.FechaHasta,
                    SumaAsegurada = x.SumaAsegurada,
                    PrimaNeta = x.PrimaNeta,
                    IVA = x.Iva,
                    Otros = x.Otros,
                    ComisionEspecial = x.ComisionEspecial,
                    TipoCambio = x.TipoDeCambio
                }
                ).ToList();
        }

        [WebMethod]
        public List<CuotaAdenda> MostrarCuotaAdenda(int ida)
        {
            return db.DetalleCuotasAdenda.Where(x => x.Adenda.Id == ida)
                .Select(
                x => new CuotaAdenda
                {
                    Id = x.Id,
                    cuota = x.Cuota.NumCoutas,
                    vence = x.Vence,
                    monto = x.Monto,
                    estado = x.Estado
                }
                ).ToList();
        }

        //--------------------------------------Reclamos--------------------------------------------
        //[WebMethod]
        [WebMethod]

        public List<MostrarReclamos> MostrarListaReclamos()
        {

            return db.Reclamos
                .Select(
                x => new MostrarReclamos
                {
                    Id = x.Id,
                    NumPoliza = x.Poliza.NumPoliza,
                    NombreCliente = x.Poliza.Cliente.Nombres + " " + x.Poliza.Cliente.Apellidos,
                    NumReclamo = x.NumReclamo
                }
                ).ToList();

        }

        [WebMethod]
        public List<DetalleReclamos> MostrarDetalleReclamos(int id)
        {

            return db.Reclamos.Where(x => x.Id == id)
                .Select(
                x => new DetalleReclamos
                {
                    Id = x.Id,
                    NumPoliza = x.Poliza.NumPoliza,
                    NombreCliente = x.Poliza.Cliente.Nombres + " " + x.Poliza.Cliente.Apellidos,
                    NumeroReclamo = x.NumReclamo,
                    BienAsegurado = x.BienAsegurado.NumCertificado,
                    CoberturaReclamada = x.CoberturaReclamo.Descripcion,
                    TipoReclamo = x.TipoReclamo,
                    FechaSiniestro = x.FechaSiniestro,
                    LugarOcurrencia = x.LugarOcurrencia,
                    MontoReclamado = x.MontoReclamado

                }
                ).ToList();
        }

        [WebMethod]
        public List<DocumentosReclamos> MostrarDocumentosReclamos(int idr)
        {
            return db.DetalleDocumentos.Where(x => x.Reclamo.Id == idr)
                .Select(
                x => new DocumentosReclamos
                {
                    Id = x.Id,
                    Documento = x.Documento.Descripcion,
                    Fecha = x.Fecha,
                    Emisor = x.Emisor,
                    Numero = x.Numero,
                    Valor = x.Valor,
                    Comentarios = x.Comentarios
                }
                ).ToList();

        }

        [WebMethod]
        public List<DetalleDePagoReclamos> MostrarDetalleDePagoReclamos(int idr)
        {
            return db.DetallePagos.Where(x => x.Reclamo.Id == idr)
                .Select(
                x => new DetalleDePagoReclamos
                {
                    Id = x.Id,
                    TipoDePago = x.TipoDePago.Descripcion,
                    Fecha = x.Fecha,
                    Moneda = x.Moneda,
                    NumeroDeDocumento = x.NumDoc,
                    Valor = x.Valor,
                    BancoTaller = x.BancoTaller,
                    Notas = x.Nota
                }
                ).ToList();

        }


        [WebMethod]
        public List<ImagenReclamo> MostrarArchivosReclamo(int idR)
        {
            var mostra = db.ArchivosReclamos.Where(x => x.ReclamoId == idR).ToList();
            var y = (from item in mostra
                     select new ImagenReclamo
                     {
                         Id = item.Id,
                         Imagen = ImageToByteArray(item.Foto),
                         nombre = nombreImagen(item.Foto)

                     }).ToList();
            return y;
        }

        //--------------------------------------Tramites--------------------------------------------
        //[WebMethod]
        [WebMethod]

        public List<MostrarTramites> MostrarListaTramites()
        {

            return db.Tramites
                .Select(
                x => new MostrarTramites
                {
                    Id = x.Id,
                    NumPoliza = x.Poliza.NumPoliza,
                    NombreCliente = x.Poliza.Cliente.Nombres + " " + x.Poliza.Cliente.Apellidos,
                    TipoTramite = x.Tipo
                }
                ).ToList();
        }


        [WebMethod]
        public List<DetalleTramites> MostrarDetalleTramites(int id)
        {

            return db.Tramites.Where(x => x.Id == id)
                .Select(
                x => new DetalleTramites
                {
                    Id = x.Id,
                    NumPoliza = x.Poliza.NumPoliza,
                    NombreCliente = x.Poliza.Cliente.Nombres + " " + x.Poliza.Cliente.Apellidos,
                    TipoTramites = x.Tipo,
                    Modalidad = x.Modalidad,
                    FechaRecepcion = x.FechaRecepcion,
                    NombreEjecutivo = x.NombreEjecutivo,
                    FechaEnvio = x.FechaEnvio,
                    FechaRecibido = x.FechaRecibido,
                    Recibidopor = x.RecibidoPor

                }
                ).ToList();
        }


        [WebMethod]
        public List<ImagenTramite> MostrarArchivosTramite(int idT)
        {
            var mostra = db.ArchivosTramites.Where(x => x.TramiteId == idT).ToList();
            var y = (from item in mostra
                     select new ImagenTramite
                     {
                         Id = item.Id,
                         Imagen = ImageToByteArray(item.Foto),
                         nombre = nombreImagen(item.Foto)

                     }).ToList();
            return y;
        }
        /*-------------------------------------------------------clases----------------------------------------------------------------------*/

        public class MostrarPoliza
        {
            public int Id { get; set; }
            public string NumPoliza { get; set; }
            public string NombreCliente { get; set; }
            public string producto { get; set; }

        }


        public class MostrarBienes
        {
            public int Id { get; set; }
            public string NumPoliza { get; set; }
            public string NombreCliente { get; set; }

        }


        public class MostrarAdendas
        {
            public int Id { get; set; }
            public string NumPoliza { get; set; }
            public string NombreCliente { get; set; }
            public string NumAdenda { get; set; }

        }

        public class MostrarReclamos
        {
            public int Id { get; set; }
            public string NumPoliza { get; set; }
            public string NombreCliente { get; set; }
            public string NumReclamo { get; set; }
        }


        public class MostrarTramites
        {
            public int Id { get; set; }
            public string NumPoliza { get; set; }
            public string NombreCliente { get; set; }
            public string TipoTramite { get; set; }
        }


        public class DetallePoliza
        {
            public int Id { get; set; }
            public string NumPoliza { get; set; }
            public string NombreCliente { get; set; }
            public string Contratante { get; set; }
            public string ContactoIntermediario { get; set; }
            public string Aseguradora { get; set; }
            public string Producto { get; set; }
            public string TipoMoneda { get; set; }
            public DateTime FechaEmision { get; set; }
            public DateTime Desde { get; set; }
            public DateTime Hasta { get; set; }
        }


        public class DetalleBienes
        {
            public int Id { get; set; }
            public string NumPoliza { get; set; }
            public string NombreCliente { get; set; }
            public string Producto { get; set; }
            public string Aseguradora { get; set; }
            public int NumeroCertificado { get; set; }
        }

        public class DetalleAdenda
        {
            public int Id { get; set; }
            public string NumPoliza { get; set; }
            public string NombreCliente { get; set; }
            public string NumeroAdenda { get; set; }
            public string TipoAdenda { get; set; }
            public DateTime FechaEmision { get; set; }
            public DateTime Desde { get; set; }
            public DateTime Hasta { get; set; }
            public double SumaAsegurada { get; set; }
            public double PrimaNeta { get; set; }
            public double IVA { get; set; }
            public double Otros { get; set; }
            public double ComisionEspecial { get; set; }
            public double TipoCambio { get; set; }
        }

        public class DetalleReclamos
        {
            public int Id { get; set; }
            public string NumPoliza { get; set; }
            public string NombreCliente { get; set; }
            public string NumeroReclamo { get; set; }
            public int BienAsegurado { get; set; }
            public string CoberturaReclamada { get; set; }
            public string TipoReclamo { get; set; }
            public DateTime FechaSiniestro { get; set; }
            public string LugarOcurrencia { get; set; }
            public double MontoReclamado { get; set; }
        }

        public class DetalleTramites
        {
            public int Id { get; set; }
            public string NumPoliza { get; set; }
            public string NombreCliente { get; set; }
            public string TipoTramites { get; set; }
            public string Modalidad { get; set; }
            public DateTime FechaRecepcion { get; set; }
            public string NombreEjecutivo { get; set; }
            public DateTime FechaEnvio { get; set; }
            public DateTime FechaRecibido { get; set; }
            public string Recibidopor { get; set; }
        }

        public class CuotaPoliza
        {
            public int Id { get; set; }
            public int cuota { get; set; }
            public DateTime vence { get; set; }
            public double monto { get; set; }
            public string estado { get; set; }
        }

        public class CuotaAdenda
        {
            public int Id { get; set; }
            public int cuota { get; set; }
            public DateTime vence { get; set; }
            public double monto { get; set; }
            public string estado { get; set; }
        }

        public class CaracteristicasBienes
        {
            public int Id { get; set; }
            public string Caracteristica { get; set; }
            public string Valor { get; set; }
        }

        public class CoberturaBienes
        {
            public int Id { get; set; }
            public string Cobertura { get; set; }
            public double SumaAsegurada { get; set; }
            public double Deducible { get; set; }
            public double prima { get; set; }

        }

        public class DocumentosReclamos
        {
            public int Id { get; set; }
            public string Documento { get; set; }
            public DateTime Fecha { get; set; }
            public string Emisor { get; set; }
            public int Numero { get; set; }
            public double Valor { get; set; }
            public string Comentarios { get; set; }

        }

        public class DetalleDePagoReclamos
        {
            public int Id { get; set; }
            public string TipoDePago { get; set; }
            public DateTime Fecha { get; set; }
            public string Moneda { get; set; }
            public int NumeroDeDocumento { get; set; }
            public double Valor { get; set; }
            public string BancoTaller { get; set; }
            public string Notas { get; set; }
        }

        public class ImagenPoliza
        {
            public int Id { get; set; }
            public byte[] Imagen { get; set; }
            public string nombre { get; set; }
        }

        public class ImagenTramite
        {
            public int Id { get; set; }
            public byte[] Imagen { get; set; }
            public string nombre { get; set; }
        }

        public class ImagenReclamo
        {
            public int Id { get; set; }
            public byte[] Imagen { get; set; }
            public string nombre { get; set; }
        }

        //Logeo
        public class Login
        {
            public int Id { get; set; }
            public string Usuario { get; set; }
            public string Pass { get; set; }
            public string Rol { get; set; }
            public int? IdCliente { get; set; }

        }


        //CLASES DE Cliente-------------------------------------------------------------------------------------------
        public class PolizaCliente
        {
            public int Id { get; set; }
            public string NumPolizaC { get; set; }
            public string ProductoAdC { get; set; }
            public string Aseguradora { get; set; }
            public string Nombre { get; set; }
        }

        public class BienesCliente
        {
            public int Id { get; set; }
            public string NumPolizaC { get; set; }
            public string ProductoAdC { get; set; }
            public string Aseguradora { get; set; }
            public string Nombre { get; set; }
        }

        public class AdendaCliente
        {
            public int Id { get; set; }
            public string NumPolizaC { get; set; }
            public string NumAdenda { get; set; }
            public string Aseguradora { get; set; }
            public string Nombre { get; set; }
        }

        public class DetallePolizaCliente
        {
            public int Id { get; set; }
            public string NumPolizaC { get; set; }
            public string NombreC { get; set; }
            public string Contratante { get; set; }
            public string ContactoIntermediario { get; set; }
            public string Aseguradora { get; set; }
            public string Producto { get; set; }
            public string TipoMoneda { get; set; }
            public DateTime Desde { get; set; }
            public DateTime Hasta { get; set; }
            public string Telefono { get; set; }

        }

        public class DetalleBienesCliente
        {
            public int Id { get; set; }
            public string NumeroPoliza { get; set; }
            public string NombreC { get; set; }
            public string Producto { get; set; }
            public string ASeguradora { get; set; }
            public int NumCertificado { get; set; }
            public string Telefono { get; set; }

        }

        public class CaracteristicasBienesCliente
        {
            public int Id { get; set; }
            public string Caracteristica { get; set; }
            public string Valor { get; set; }
        }

        public class DetalleAdendaCliente
        {
            public int Id { get; set; }
            public string NumeroPoliza { get; set; }
            public string NombreC { get; set; }
            public string NumeroAdenda { get; set; }
            public string TipoAdenda { get; set; }
            public DateTime Desde { get; set; }
            public DateTime Hasta { get; set; }
            public string Telefono { get; set; }

        }

        public class LoginWS
        {
            public int Id { get; set; }
            public string Usuario { get; set; }
            public string Password { get; set; }
        }



        //METODOS DE CONVERSION DE IMAGEN
        public byte[] ImageToByteArray(string foto)
        {
            MemoryStream ms = new MemoryStream();
            foto = Server.MapPath(foto);
            Image.FromFile(foto).Save(ms, ImageFormat.Jpeg);

            return ms.ToArray();
        }

        public string nombreImagen(string foto)
        {
            //MemoryStream ms = new MemoryStream();
            foto = Server.MapPath(foto);
            string resul = Path.GetFileName(foto);
            //Image.FromFile(foto).Save(ms, ImageFormat.Jpeg);

            return resul;
        }

        /*===========================================================================================
                                        LISTAR RECLAMOS
  ============================================================================================*/
        public class ReclamoLista
        {
            public int Id { get; set; }
            public string FechaReclamo { get; set; }
            public string Lugar { get; set; }
            public string Reclamo { get; set; }
            public string NombreCliente { get; set; }
        }



        [WebMethod]
        public List<ReclamoLista> ListarReclamos(int Id)
        {
            return dbs.ReclamosTemps.Where(x => x.Usuario.IdNUser == Id)
                .Select(
                x => new ReclamoLista
                {
                    Id = x.Id,
                    FechaReclamo = x.Fecha,
                    Lugar = x.Lugar,
                    Reclamo = x.Comentarios,
                    NombreCliente = x.Usuario.Nombre + " " + x.Usuario.Apellido

                }
                ).ToList();
        }

        /*===========================================================================================
                                      LISTAR RECLAMOS
============================================================================================*/
        //public Image stringToImage(string inputString)
        //{
        //    byte[] imageBytes = Convert.FromBase64String(inputString);
        //    MemoryStream ms = new MemoryStream(imageBytes);

        //    Image image = Image.FromStream(ms, true, true);

        //    return image;
        //}
        //public byte[] ImageToByteArray(Image imagen, string foto)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    string path =Server.MapPath(foto);
        //    imagen.Save(path, forma);
        //    return ms.ToArray();
        //}
    }
}