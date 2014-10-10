using System;
using Oracle.DataAccess.Client;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;

namespace ExecutableProcess
{
    class Process
    {
        static private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["oracleconnection"].ConnectionString;
        }
        internal static string SetBatchParameters(int Process_Id, string Inv_Grp, int Tsp_list, DateTime gl_date, DateTime Acctdt)
        {
                      
            string connectionString = GetConnectionString();
            string ERP_ProcessLogID=string.Empty;
            
            OracleCommand commandloadparams=null;
            using (OracleConnection connection = new OracleConnection())
            {
               
                try
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    //calling loadparams stored proc

                    commandloadparams = connection.CreateCommand();
                    commandloadparams.CommandText = SqlConstants.Loadparameters;
                    commandloadparams.CommandType = CommandType.StoredProcedure;
                    commandloadparams.Parameters.Add("v_Process_Id", OracleDbType.Int32).Value = Process_Id;
                    commandloadparams.Parameters.Add("v_Inv_Grp", OracleDbType.Varchar2).Value = Inv_Grp;
                    commandloadparams.Parameters.Add("v_Tsp_list", OracleDbType.Varchar2).Value = Tsp_list;
                    commandloadparams.Parameters.Add("v_gl_date", OracleDbType.Date).Value = gl_date;
                    commandloadparams.Parameters.Add("v_Acctdt", OracleDbType.Date).Value = Acctdt;
                    commandloadparams.Parameters.Add("v_processlogid", OracleDbType.Varchar2).Direction = System.Data.ParameterDirection.Output;
                    commandloadparams.ExecuteNonQuery();
                    if (!commandloadparams.Parameters["v_processlogid"].Value.Equals(DBNull.Value))
                    {
                        ERP_ProcessLogID = commandloadparams.Parameters["v_processlogid"].Value.ToString();
                    }
                  
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
                finally
                {
                    commandloadparams.Dispose();
                    connection.Close();                    
                }
                return ERP_ProcessLogID;  
            }
        }



        internal static void PreProcessTransformations(string ERPProcessLogID, int v_Process_Id, string v_Inv_Grp, int v_Tsp_list, DateTime v_gl_date, DateTime v_Acctdt,
            out string batchtime, out string batchdate, out DateTime startdate)
        {
            batchdate = string.Empty;
            batchtime = string.Empty;
            startdate=Convert.ToDateTime(null);
        }
    }
}
