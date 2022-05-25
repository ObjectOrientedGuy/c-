using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Npgsql;
using System.Data;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationsController : ControllerBase
    {
        private IConfiguration _configuration;

        public RegistrationsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // GET: api/<RegistrationsController>
        [HttpGet]
        public string Get()
        {
            DataTable table = new DataTable();
            NpgsqlDataReader myReader;

            using (NpgsqlConnection myConn = new NpgsqlConnection(_configuration.GetConnectionString("DataBaseConfig")))
            {
                myConn.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM registrations;", myConn))
                {
                    myReader = command.ExecuteReader();
                    table.Load(myReader); 

                    myReader.Close();
                    myConn.Close();
                }
            }

            return JsonConvert.SerializeObject(table);
        }

        // GET api/<RegistrationsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            DataTable table = new DataTable();
            NpgsqlDataReader myReader;

            using(NpgsqlConnection myConn = new NpgsqlConnection(_configuration.GetConnectionString("DataBaseConfig")))
            {
                myConn.Open();
                using(NpgsqlCommand command = new NpgsqlCommand(String.Format("SELECT * FROM registrations WHERE id = {0};",id),myConn))
                {
                    myReader = command.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConn.Close();

                }
            }

            if (table.Rows.Count == 0)
                return JsonConvert.SerializeObject(new { status = "400", message = "invalid id" });


            return  JsonConvert.SerializeObject(table);
        }

        [HttpGet("search")]
        public string Get([FromQuery] string? sort_by = "", [FromQuery] string? sort_type = "ASC", [FromQuery] string? search_string = "")
        {
            DataTable table = new DataTable();
            HttpResponseMessage response = new HttpResponseMessage();
            NpgsqlDataReader myReader;
            string query = "SELECT * FROM registrations ";

            if (sort_by.Length > 0)
                query+=String.Format("ORDER BY {0} {1};", sort_by, sort_type);

            using (NpgsqlConnection myConn = new NpgsqlConnection(_configuration.GetConnectionString("DataBaseConfig")))
            {
                myConn.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(query, myConn))
                {
                    try
                    {
                        myReader = command.ExecuteReader();
                    }catch (Exception ex)
                    {
                        return JsonConvert.SerializeObject(new {status = "200", message = "invalid params"});
                    }
                    
                    table.Load(myReader);

                    myReader.Close();
                    myConn.Close();

                }
            }

            if(search_string.Length < 1)
            {
                return JsonConvert.SerializeObject(table);
            }
                

            for (int i = 0; i < table.Rows.Count; i++)
            {
                bool deleteRow = true;

                for(int j = 0; j < table.Columns.Count; ++j)
                {
                    if(table.Rows[i][j].ToString().Contains(search_string))
                    {
                        deleteRow = false;
                    }
                }
                    

                if (deleteRow)
                {
                    table.Rows.Remove(table.Rows[i]);
                    i--;
                }
            }
            return JsonConvert.SerializeObject(table);

        }

        // POST api/<RegistrationsController>
        [HttpPost]
        public string Post([FromBody] REGISTRATION_CERTIFICATE value)
        {
            try
            {
                value.validateWholeClass();
            }catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { status = "400", message = ex.Message });
            }

            int count = 0;
            using (NpgsqlConnection myConn = new NpgsqlConnection(_configuration.GetConnectionString("DataBaseConfig")))
            {
                string guery = String.Format("INSERT INTO registrations (registration_number,vin_code,car,date_of_registration,year_of_manufacture)" +
                    "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')", value.registration_number, value.vin_code, value.car, value.date_of_registration, value.year_of_manufacture);

                myConn.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(guery, myConn))
                {
                    try
                    {
                        count = command.ExecuteNonQuery();
                    }catch(Exception ex)
                    {
                        return JsonConvert.SerializeObject(new { status = "400", message = "Invalid values." });
                    }

                    myConn.Close();

                }

            }

            if (count < 1)
            {
                return JsonConvert.SerializeObject(new { status = "200", message = "invalid id" });
            }

            return JsonConvert.SerializeObject(new { status = "200", message = "Customer has been successfully added" });
        }

        // PUT api/<RegistrationsController>/5
        [HttpPut("{id}")]
        public string Put(int id, [FromBody] REGISTRATION_CERTIFICATE value)
        {
            try
            {
                value.validateWholeClass();
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { status = "400", message = ex.Message });
            }

            int count = 0;
            using (NpgsqlConnection myConn = new NpgsqlConnection(_configuration.GetConnectionString("DataBaseConfig")))
            {
                string guery = String.Format("update registrations set registration_number = '{0}' , vin_code = '{1}' ," +
                    " car = '{2}' , date_of_registration = '{3}' , year_of_manufacture = '{4}' " +
                    "WHERE id = {5};", value.registration_number, value.vin_code, value.car, value.date_of_registration, value.year_of_manufacture, id);

                myConn.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(guery, myConn))
                {
                    count = command.ExecuteNonQuery();


                    myConn.Close();

                }

            }

            if (count < 1)
            {
                return JsonConvert.SerializeObject(new { status = "400", message = "invalid id" });
            }

            return JsonConvert.SerializeObject(new { status = "200", message = "Customer has been successfully updated" });
        }

        // DELETE api/<RegistrationsController>/5
        [HttpDelete("{id}")]
        public Task<HttpResponseMessage> Delete(int id)
        {
            int count = 0;
            using (NpgsqlConnection myConn = new NpgsqlConnection(_configuration.GetConnectionString("DataBaseConfig")))
            {
                myConn.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(String.Format("DELETE FROM registrations WHERE id = {0};", id), myConn))
                {
                    count = command.ExecuteNonQuery();
                    myConn.Close();

                }
            }
            HttpResponseMessage response = new HttpResponseMessage();
            if (count < 1)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("Customer is not found");
            }else
            {
                response.StatusCode = HttpStatusCode.OK;
                response.Content = new StringContent("Customer has been successfully deleted.");
            }
            return Task.FromResult(response);
        }
    }
}
