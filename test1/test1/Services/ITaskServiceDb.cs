using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test1.Services
{
    public interface ITaskServiceDb
    {
        public IActionResult GetMemberTask(int id);
        public IActionResult DeleteProject(int id);
    }
}
