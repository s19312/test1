using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using test1.Models;

namespace test1.Services
{
    public class SqlServerTaskDbService : ControllerBase, ITaskServiceDb
    {
        public IActionResult DeleteProject(int id)
        {
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19312;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                com.Transaction = tran;
                com.Connection = con;

                int check = 0;
                com.Parameters.AddWithValue("@idProject", id);
                com.CommandText = "select idProject from project where idPRoject = @idProject";

                var dr = com.ExecuteReader();
                if (dr.Read()) {
                    check = (int)dr["IdProject"];

                }
                dr.Close();

                if (check == 0) {
                    return NotFound("Such Project not found!");
                }

                com.CommandText = "delete from Task where idProject = @idProject";
                com.ExecuteNonQuery();


                com.CommandText = "delete from project where idProject = @idProject";
                com.ExecuteNonQuery();


                tran.Commit();

            }
                return Ok("Project deleted!");
        }


       

        public IActionResult GetMemberTask(int id)
        {
            var list = new List<TeamMemberTask>();


            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19312;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                com.Transaction = tran;
                com.Connection = con;

                com.Parameters.AddWithValue("idTeamMember", id);


                int checkMember = 0;
                com.CommandText = "select idTeamMember from TeamMember where idTeamMember = @idTeamMember";

                var dr = com.ExecuteReader();
                if (dr.Read())
                {
                    checkMember = (int)dr["IdTeamMember"];
                    dr.Close();
                }

                if (checkMember == 0) {
                    return BadRequest("TeamMember Not Found!");
                }

                com.CommandText = "select t.*, tm.*, p.Name 'ProjectName' from Task t inner join TeamMember tm ON t.idAssignedTo = tm.idTeamMember " +
                                                        "inner join Project p ON p.idProject = t.idPRoject" +
                                                        "    where t.idAssignedTo = @idTeamMember " +
                                                             "  AND t.idCreator = @idTeamMember";

                 dr = com.ExecuteReader();
                

                while (dr.Read())
                {
                    var t = new TeamMemberTask();
                    t.IdTeamMember = (int)dr["IdTeamMember"];
                    t.FirstName = dr["FirstName"].ToString();
                    t.LastName = dr["LastName"].ToString();
                    t.Email = dr["Email"].ToString();

                    t.IdTask = (int)dr["IdTask"];
                    t.Name = dr["Name"].ToString();
                    t.Description = dr["Description"].ToString();
                    
                    t.IdTaskType = (int)dr["IdTaskType"];
                    t.ProjectName = dr["ProjectName"].ToString();
                    list.Add(t);
                }
                dr.Close();

                tran.Commit();
            }
            return Ok(list);
        }
    }
}
