using MediatR;
using Mensajeria.Domain.DIncidencias;
using Mensajeria.Persistence.Database;
using Mensajeria.Service.EventHandler.Commands.Incidencias;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using System.Linq;

namespace Mensajeria.Service.EventHandler.Handlers.Incidencias
{
    public class IncidenciaExcelCreateEventHandler : IRequestHandler<IncidenciaExcelCreateCommand, List<Incidencia>>
    {
        private readonly ApplicationDbContext _context;

        public IncidenciaExcelCreateEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Incidencia>> Handle(IncidenciaExcelCreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var path = CargaArchivoExcel(request.Excel, request.Folio);
                List<Incidencia> incidencias = GetDatosExcel(path, request);
                List<Incidencia> incidenciasCargadas = new List<Incidencia>();
                var proceed = incidencias.Where(i => !i.Observaciones.Equals("SI")).Count();
                
                if (proceed > 0)
                {
                    return incidencias;
                }
                else 
                {
                    foreach (var inc in incidencias)
                    {
                        var incidencia = GetCapturaIncidencia(inc);

                        await _context.AddAsync(incidencia);
                        await _context.SaveChangesAsync();

                        incidencia.Observaciones = inc.Observaciones;
                        incidenciasCargadas.Add(incidencia);
                    }
                }

                return incidenciasCargadas;
            }
            catch (Exception ex)
            {
                string message = ex.ToString();
                return null;
            }
        }

        private Incidencia GetCapturaIncidencia(Incidencia request)
        {
            var incidencia = new Incidencia
            {
                UsuarioId = request.UsuarioId,
                IncidenciaId = request.IncidenciaId,
                CedulaEvaluacionId = request.CedulaEvaluacionId,
                Pregunta = request.Pregunta,
                FechaProgramada = request.FechaProgramada,
                FechaEntrega = request.FechaEntrega,
                NumeroGuia = request.NumeroGuia,
                CodigoRastreo = request.CodigoRastreo,
                Acuse = request.Acuse,
                TipoServicio = request.TipoServicio,
                Sobrepeso = request.Sobrepeso,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now
            };

            return incidencia;
        }

        public List<Incidencia> GetDatosExcel(string path, IncidenciaExcelCreateCommand request)
        {
            var constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=YES'";
            DataTable datatable = new DataTable();

            constr = string.Format(constr, path);

            using (OleDbConnection excelconn = new OleDbConnection(constr))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    using (OleDbDataAdapter adapterexcel = new OleDbDataAdapter())
                    {

                        excelconn.Open();
                        cmd.Connection = excelconn;
                        DataTable excelschema;
                        excelschema = excelconn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        var sheetname = excelschema.Rows[0]["Table_Name"].ToString();
                        excelconn.Close();

                        excelconn.Open();
                        cmd.CommandText = "SELECT * From [" + sheetname + "]";
                        adapterexcel.SelectCommand = cmd;
                        adapterexcel.Fill(datatable);
                        excelconn.Close();
                    }
                }
            }
            return GetModelIncidencias(datatable, request);
        }

        public List<Incidencia> GetModelIncidencias(DataTable excel, IncidenciaExcelCreateCommand request)
        {
            List<Incidencia> incidencias = new List<Incidencia>();

            foreach (DataRow row in excel.Rows)
            {
                Incidencia incidencia = new Incidencia();
                incidencia.UsuarioId = request.UsuarioId;
                incidencia.CedulaEvaluacionId = request.CedulaEvaluacionId;
                incidencia.IncidenciaId = request.IncidenciaId;
                incidencia.Pregunta = request.Pregunta;
                incidencia.FechaProgramada = row["Fecha Programada"] != DBNull.Value ? Convert.ToDateTime(row["Fecha Programada"]) : Convert.ToDateTime("01/01/1990");
                incidencia.FechaEntrega = row["Fecha de Entrega"] != DBNull.Value ? Convert.ToDateTime(row["Fecha de Entrega"]) : Convert.ToDateTime("01/01/1990");
                incidencia.NumeroGuia = row["Numero de Guia"] != DBNull.Value ? row["Numero de Guia"].ToString().ToUpper() : "0";
                incidencia.CodigoRastreo = row["Codigo de Rastreo"] != DBNull.Value ? row["Codigo de Rastreo"] + "" : "0";
                incidencia.Acuse = row["Acuse"] != DBNull.Value ? row["Acuse"] + "" : "";
                incidencia.TipoServicio = row["Tipo de Servicio"] != DBNull.Value ?  CultureInfo.CurrentCulture.TextInfo.ToTitleCase(row["Tipo de Servicio"].ToString().ToLower())  : "";
                incidencia.Sobrepeso = row["Sobrepeso"] != DBNull.Value ? Convert.ToDecimal(row["Sobrepeso"]) : 0;
                incidencia.Observaciones = GetValidacionCampo(row).Equals("No se encontraron inconsistencias") ? "SI" : GetValidacionCampo(row);
                incidencias.Add(incidencia);
            }
            return incidencias;
        }

        private string GetValidacionCampo(DataRow row)
        {
            string msg = "No se encontraron inconsistencias";
            if (row["Fecha Programada"] == DBNull.Value)
            {
                msg = "La fecha programada no está capturada.";
            }
            else if (row["Fecha de Entrega"] == DBNull.Value)
            {
                msg = "La fecha de entrega no está capturada.";
            }
            else if (Convert.ToDateTime(row["Fecha Programada"]) > Convert.ToDateTime(row["Fecha de Entrega"]) || 
                        Convert.ToDateTime(row["Fecha Programada"]) == Convert.ToDateTime(row["Fecha de Entrega"]))
            {
                msg = "La fecha programada es mayor o igual a la fecha de entrega.";
            }
            else if (row["Numero de Guia"] == DBNull.Value)
            {
                msg = "El número de guía no está capturado.";
            }
            else if (row["Numero de Guia"].ToString().Length != 22 || !row["Numero de Guia"].ToString().ToUpper().Contains("A"))
            {
                msg = "El número de guía no es válido.";
            }
            else if (row["Codigo de Rastreo"] != DBNull.Value && row["Codigo de Rastreo"].ToString().Length != 10)
            {
                msg = "El código de rastreo no es válido, si no cuenta con el deje el valor en 0.";
            }
            else if (row["Acuse"] == DBNull.Value)
            {
                msg = "El acuse no está capturado.";
            }
            else if (!row["Acuse"].ToString().Equals("SI") && !row["Acuse"].ToString().Equals("NO"))
            {
                msg = "El valor del acuse no es válido. Debe escribir \"SI\" O \"NO\" según sea el caso.";
            }
            else if (row["Tipo de Servicio"] == DBNull.Value)
            {
                msg = "El tipo de servicio no está capturado.";
            }
            else if (!CultureInfo.CurrentCulture.TextInfo.ToTitleCase(row["Tipo de Servicio"].ToString().ToLower()).Equals("Nacional") &&
                     !CultureInfo.CurrentCulture.TextInfo.ToTitleCase(row["Tipo de Servicio"].ToString().ToLower()).Equals("Nacional Sobrepeso") &&
                     !CultureInfo.CurrentCulture.TextInfo.ToTitleCase(row["Tipo de Servicio"].ToString().ToLower()).Equals("Internacional") &&
                     !CultureInfo.CurrentCulture.TextInfo.ToTitleCase(row["Tipo de Servicio"].ToString().ToLower()).Equals("Internacional Sobrepeso")
                     )
            {
                msg = "El valor del tipo de servicio no es válido. Debe escribir \"Nacional\", \"Nacional Sobrepeso\", \"Internacional\" o \"Internacional Sobrepeso\"  según sea el caso.";
            }
            else if (row["Sobrepeso"] == DBNull.Value)
            {
                msg = "El sobrepeso no está capturado.";
            }

            return msg;
        }

        private string CargaArchivoExcel(IFormFile fromFiles, string folio)
        {
            string newPath = Directory.GetCurrentDirectory() + "\\Incidencias Excel\\" +folio;
            string dest_path = Path.Combine(newPath, "Excel Incidencias");

            if (!Directory.Exists(dest_path))
            {
                Directory.CreateDirectory(dest_path);
            }
            string sourcefile = Path.GetFileName(fromFiles.FileName);
            string path = Path.Combine(dest_path, sourcefile);

            using (FileStream filestream = new FileStream(path, FileMode.Create))
            {
                fromFiles.CopyTo(filestream);
            }
            return path;
        }

        private bool eliminaArchivoActual(string folio, string archivo)
        {
            string newPath = Directory.GetCurrentDirectory() + "\\Incidencias Excel\\" + folio;
            string dest_path = Path.Combine(newPath, "Excel Incidencias");

            try
            {
                File.Delete(dest_path + "\\" + archivo);
                return true;
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                return false;
            }
        }
    }
}
