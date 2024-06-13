using MediatR;
using Mensajeria.Domain.DIncidencias;
using Mensajeria.Persistence.Database;
using Mensajeria.Service.EventHandler.Commands.Incidencias;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mensajeria.Service.EventHandler.Handlers.Incidencias
{
    public class SoportePagoCreateEventHandler : IRequestHandler<SoportePagoCreateCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public SoportePagoCreateEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(SoportePagoCreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string file = await Copia(request);

                int rows = await RealizaCargaMasiva(file, request.TXT.FileName.Replace(".txt",""), request.UsuarioId, request.Anio, request.Mes);

                eliminaArchivo(request);

                return rows;
            }
            catch (Exception ex)
            {
                string message = ex.ToString();
                return -1;
            }
        }

        private async Task<int> RealizaCargaMasiva(string path, string file, string usuarioId, int anio, string mes)
        {
            try
            {
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                var pPath = cmd.CreateParameter();
                var pFile = cmd.CreateParameter();
                var pUser = cmd.CreateParameter();
                var pAnio = cmd.CreateParameter();
                var pMes = cmd.CreateParameter();
                
                pPath.ParameterName = "@path";
                pPath.Value = path;
                cmd.Parameters.Add(pPath);
                
                pFile.ParameterName = "@file";
                pFile.Value = file;
                cmd.Parameters.Add(pFile);
                
                pUser.ParameterName = "@usuarioId";
                pUser.Value = usuarioId;
                cmd.Parameters.Add(pUser);
                
                pAnio.ParameterName = "@anio";
                pAnio.Value = anio;
                cmd.Parameters.Add(pAnio);
                
                pMes.ParameterName = "@mes";
                pMes.Value = mes;
                cmd.Parameters.Add(pMes);
                
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[Mensajeria].[sp_insertaSoportePago]";

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                int rows = await cmd.ExecuteNonQueryAsync();

                cmd.Connection.Close();

                return rows;
            }
            catch (Exception ex)
            {
                 string msg = ex.Message;
                return -1;
            }
        }
        
        private async Task<string> Copia(SoportePagoCreateCommand request)
        {
            try
            {
                string newPath = Directory.GetCurrentDirectory() + "\\Soportes de Pago\\" + request.Anio + "\\" + request.Mes;
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                using (var stream = new FileStream(newPath + "\\" + request.TXT.FileName, FileMode.Create))
                {
                    try
                    {
                        await request.TXT.CopyToAsync(stream);
                        return request.TXT.FileName;
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message.ToString();
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return msg;
            }
        }

        public bool eliminaArchivo(SoportePagoCreateCommand request)
        {
            string newPath = Directory.GetCurrentDirectory() + "\\Soportes de Pago\\" + request.Anio + "\\" + request.Mes;
            string dest_path = Path.Combine(newPath, "Excel Incidencias");

            try
            {
                File.Delete(dest_path + "\\" + request.TXT.FileName);
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
