using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutableProcess
{
      
    
    class Program
    {
        public static int v_Process_Id;
        public static string v_Inv_Grp;
        public static int v_Tsp_list;
        public static DateTime v_gl_date;
        public static DateTime v_Acctdt;

        private static readonly Lazy<Program> lazy = new Lazy<Program>(() => new Program());

        public static Program Instance { get { return lazy.Value; } }
        private Program() 
        {

        }

        static void Main(string[] args)
        {

            if (ProcessCommandLineArguments(args))
            {

                string ERPProcessLogID = Process.SetBatchParameters(v_Process_Id,v_Inv_Grp,v_Tsp_list,v_gl_date,v_Acctdt);
                if (!string.IsNullOrEmpty(ERPProcessLogID))
                {
                    string batchtime,batchdate;
                    DateTime startdate;

                    Process.PreProcessTransformations(ERPProcessLogID, v_Process_Id, v_Inv_Grp, v_Tsp_list, v_gl_date, v_Acctdt, out batchtime, out batchdate, out startdate);

                }
                else
                {
                    Console.WriteLine("Invalid logid received");
                }
                
            }
            else
            {
                Console.WriteLine("error");
            }
        }


        private static bool ProcessCommandLineArguments(string[] args)
        {

                            
              bool parameters_isvalid = true;

            Console.WriteLine("{0} Command Line Arguments", args.Length);

            if (args.Length == 5)
            {
                if (!Int32.TryParse(args[0], out v_Process_Id))
                {
                    parameters_isvalid = false;
                    Console.WriteLine("process id is not valid");

                }
                
                if (string.IsNullOrEmpty(args[1]))
                {
                    parameters_isvalid = false;
                    Console.WriteLine("invoice group is not valid");
                }
                else
                {
                    v_Inv_Grp = args[1];
                }
               
                if (!Int32.TryParse(args[2], out v_Tsp_list))
                {
                    parameters_isvalid = false;
                    Console.WriteLine("TspList is not valid");
                }
               
                if (DateTime.TryParse(args[3], out v_gl_date))
                {
                    parameters_isvalid = false;
                    Console.WriteLine("GLDate is not valid");
                }
                
                if (DateTime.TryParse(args[4], out v_Acctdt))
                {
                    parameters_isvalid = false;
                    Console.WriteLine("Accounting date is not valid");
                }
               
                if (parameters_isvalid)
                {
                    Console.WriteLine("{0}:\t{1}:\t{2}:\t{3}:\t{4}", v_Process_Id, v_Inv_Grp, v_Tsp_list, v_gl_date, v_Acctdt);
                }
                else
                {
                    Console.WriteLine("process terminated");
                }


            }
            else
            {
                Console.WriteLine("please enter all 5 parameters");
                Console.WriteLine("user input not matching the number of parameters");
            }
            return parameters_isvalid;
        }
    }
}
