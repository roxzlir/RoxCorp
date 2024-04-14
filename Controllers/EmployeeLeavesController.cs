using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoxCorp.Data;
using RoxCorp.Models;
using RoxCorp.Utility;

namespace RoxCorp.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class EmployeeLeavesController : Controller
    {
        //Lägger med min kontext för att få kontakt med databasen
        private readonly ApplicationDbContext _context;
        
        string selectedLeaveType = null;

        public EmployeeLeavesController(ApplicationDbContext context)
        {
            _context = context;
        }

        

        public async Task<IActionResult> Index(int? leaveId)
        {
            var leaves = await _context.Leaves.ToListAsync();

            var applyEmployeeLeaveQuery = from applies in _context.ApplyForLeaves
                                          join emp in _context.Employees on applies.FkEmployeeId equals emp.EmployeeId
                                          join lea in _context.Leaves on applies.FkLeaveId equals lea.LeaveId
                                          join grant in _context.GrantLeaves on applies.ApplyForLeaveId equals grant.FkApplyForLeaveId
                                          select new {applies, emp, lea, grant};

            //skapa ett filter med det leaveId som man kan skicka med in till denna Task om det finns
            //Här kör jag så om man skulle få ett leaveId med skickat in till min Index så vill jag sorterar om min query till att gå efter det värdet
            if (leaveId.HasValue)
            {
                applyEmployeeLeaveQuery = applyEmployeeLeaveQuery.Where(x => x.lea.LeaveId == leaveId.Value);
            }

            //Här tar jag då alltså och samlar all data som finns i min query (datan från databaserna) och sparar ner det mot mina olika properties i
            //EmployeeWithLeaveType och lagrar det i var employees.
            //Så det som nu finns kopplat i var employees är alltså både EmployeeName och DepartmentName i list format
            var employees = await applyEmployeeLeaveQuery.Select(x => new EmployeeWithLeaveType
            {
                EmployeeName = x.emp.EmployeeName,
                EmployeePhone = x.emp.EmployeePhone,
                EmployeeEmail = x.emp.EmployeeEmail,
                ApplyRegisteredDate = x.applies.ApplyRegisteredDate,
                ApplyNote = x.applies.ApplyNote,
                LeaveType = x.lea.LeaveType,
                Granted = x.grant.Granted
            }).ToListAsync();

            //Detta skapar vi för att kunna styra mer vad som syns OM man inte valt något "leaveId" än, alltså 
            //när man kommer till första sidan för Index så vill jag inte displaya alla namn etc. 
            if (leaveId.HasValue)
            {
                var selectedLeave = await _context.Leaves.Where(x => x.LeaveId == leaveId.Value).FirstOrDefaultAsync();
                if (selectedLeave != null)
                {
                    selectedLeaveType = selectedLeave.LeaveType;
                }
            }

            //För att kunna använda ENDAST allt som finns i min Leave klass så skapade jag en EmployeeLeaveViewModel. I den har jag 2 st Enumerable
            //listor, en heter Leaves och en heter Employees. Så väljer jag här att lagra båda dem i var viewModel. Så denna viewModel innehåller nu:
            //Leaves = Allt som finns i databasen för Leaves
            //Employees = Alla EmployeeName + LeaveType som är kopplat till den.
            var viewModel = new EmployeeLeaveViewModel()
            {
                Employees = employees,
                Leaves = leaves,
                selectedLeave = selectedLeaveType
            };
            Console.WriteLine(viewModel);

            return View(viewModel);
        }
    }
}
