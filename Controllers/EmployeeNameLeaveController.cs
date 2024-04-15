using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoxCorp.Data;
using RoxCorp.Models;
using RoxCorp.Utility;

namespace RoxCorp.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class EmployeeNameLeaveController : Controller
    {
        private readonly ApplicationDbContext _context;
        string selectedEmployeeName = null;
        public EmployeeNameLeaveController(ApplicationDbContext context)
        {
            _context = context;   
        }
        public async Task<IActionResult> Index(int? employeeId)
        {
            var employees = await _context.Employees.ToListAsync();

            var applyEmployeeLeaveQuery = from applies in _context.ApplyForLeaves
                                          join emp in _context.Employees on applies.FkEmployeeId equals emp.EmployeeId
                                          join lea in _context.Leaves on applies.FkLeaveId equals lea.LeaveId
                                          join grant in _context.GrantLeaves on applies.ApplyForLeaveId equals grant.FkApplyForLeaveId
                                          select new { applies, emp, lea, grant };
            if (employeeId.HasValue)
            {
                applyEmployeeLeaveQuery = applyEmployeeLeaveQuery.Where(x => x.emp.EmployeeId == employeeId.Value);
            }

            var leaves = await applyEmployeeLeaveQuery.Select(x => new EmployeeNameLeaveInfo
            {
                EmployeeName = x.emp.EmployeeName,
                LeaveType = x.lea.LeaveType,
                ApplyNote = x.applies.ApplyNote,
                ApplyFromDate = x.applies.ApplyFromDate,
                ApplyToDate = x.applies.ApplyToDate,
                ApplyRegisteredDate = x.applies.ApplyRegisteredDate,
                Granted = x.grant.Granted

            }).ToListAsync();

            if (employeeId.HasValue)
            {
                var selectedEmployee = await _context.Employees.Where(x => x.EmployeeId == employeeId.Value).FirstOrDefaultAsync();
                if (selectedEmployee != null)
                {
                    selectedEmployeeName = selectedEmployee.EmployeeName;
                }
            }

            var viewModel = new EmployeeNameLeaveInfoViewModel()
            {
                Leaves = leaves,
                Employees = employees,
                selectedEmployee = selectedEmployeeName
            };

            return View(viewModel);
        }
    }
}
