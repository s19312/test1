using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test1.Services;

namespace test1.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class MemberTaskController : ControllerBase
    {

        private ITaskServiceDb taskServiceDb;
        public MemberTaskController(ITaskServiceDb taskServiceDb) {
            this.taskServiceDb = taskServiceDb;
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteProject(int id ) {
            return taskServiceDb.DeleteProject(id);
        }
        [HttpGet("{id}")]
        public IActionResult GetMemberTask(int id) {
            return taskServiceDb.GetMemberTask(id);
        }
    }
}
