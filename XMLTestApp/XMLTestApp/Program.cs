using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XMLTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Employee[] employees = new Employee[4];
            employees[0] = new Employee(1, "David", "Smith", 10000);
            employees[1] = new Employee(3, "Mark", "Drinkwater", 30000);
            employees[2] = new Employee(4, "Norah", "Miller", 20000);
            employees[3] = new Employee(12, "Cecil", "Walker", 120000);

            using (XmlWriter writer = XmlWriter.Create("employees.xml"))
            {
                writer.WriteStartDocument();
                
                writer.WriteStartElement("Employees");

                writer.WriteStartElement("Header");
                writer.WriteElementString("Headertext", "123");
                writer.WriteEndElement();
                DataSet dsHeader = GetHeaderData();
                //foreach (Employee employee in employees)
                foreach (DataRow dr in dsHeader.Tables[0].Rows) //for multiple invoice headers
                {
                    DataSet dsLineItems = GetLineItems(dr["inv_id"].ToString());
                    foreach(DataRow dr1 in dsLineItems.Tables[0].Rows)  //for multiple invoice line items
                    {
                        writer.WriteElementString("Source_System_ID", dr["Src_Sys_ID"].ToString());   // <-- These are new
                        writer.WriteElementString("FirstName", employee.FirstName);
                        writer.WriteElementString("LastName", employee.LastName);
                    }
                    writer.WriteStartElement("Employee");
                    writer.WriteElementString("Source_System_ID", dr["Src_Sys_ID"].ToString() );   // <-- These are new
                    writer.WriteElementString("FirstName", employee.FirstName);dr["col1"]
                    writer.WriteElementString("LastName", employee.LastName);
                    writer.WriteElementString("Salary", employee.Salary.ToString());

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
                Console.ReadLine();
            }
        }

        private static DataSet GetLineItems(string inv_id)
        {
            throw new NotImplementedException();
        }
 
    
        static private string GetConnectionString() 
        { 
            // To avoid storing the connection string in your code, 
            // you can retrieve it from a configuration file. 
            //return "Data Source=DQDISTR2;Persist Security Info=True;" + 
            //       "User ID=etl_onk;Password=Friday13th;Unicode=True"; 
            return "";
        }


        static private DataSet GetHeaderData() 
        { 
            string connectionString = GetConnectionString();
            DataSet ds = null;
            using (OracleConnection connection = new OracleConnection()) 
            { 
                connection.ConnectionString = connectionString; 
                connection.Open();

                string sql = "";
                OracleCommand command = connection.CreateCommand(); 
    
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                
                OracleDataAdapter oda = new OracleDataAdapter(command);
                ds = new DataSet();
                oda.Fill(ds);
                //OracleDataReader reader = command.ExecuteReader(); 
            }
            return ds;
        }
    }
    
    class Employee
    {
        int _id;
        string _firstName;
        string _lastName;
        int _salary;

        public Employee(int id, string firstName, string lastName, int salary)
        {
            this._id = id;
            this._firstName = firstName;
            this._lastName = lastName;
            this._salary = salary;
        }

        public int Id { get { return _id; } }
        public string FirstName { get { return _firstName; } }
        public string LastName { get { return _lastName; } }
        public int Salary { get { return _salary; } }
    }
}
