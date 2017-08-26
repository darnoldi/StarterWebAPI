using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using StarterAPI.Entities;
using StarterWebAPI.Dtos;
using StarterWebAPI.Helpers;
using StarterWebAPI.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterWebAPI.Controllers
{ 

[Route("api/[controller]")]
public class AccountsController : Controller
{
    private readonly PacktDbContext _appDbContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager <AppRole > _roleManager;
    private readonly IMapper _mapper;

    public AccountsController(UserManager<AppUser> userManager, RoleManager<AppRole > roleManager, IMapper mapper, PacktDbContext appDbContext)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _appDbContext = appDbContext;
    }

    // POST api/accounts
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]RegistrationDto model)
    {

            //await createRolesandUsers();

            //return Ok();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentity = _mapper.Map<AppUser>(model);

            var result = await _userManager.CreateAsync(userIdentity, model.Password);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            await _appDbContext.JobSeekers.AddAsync(new JobSeeker { IdentityId = userIdentity.Id, Location = model.Location });
            await _appDbContext.SaveChangesAsync();

            return new OkObjectResult("Account created");
        }



        // POST api/accounts
        [HttpPost]
        private async Task createRolesandUsers()
        {

            bool x = await _roleManager.RoleExistsAsync("Admin");
            if (!x)
            {

                // first we create Admin rool    
                var role = new AppRole ();
                role.Name = "Admin";
                await _roleManager.CreateAsync(role);

                //Here we create a Admin super user who will maintain the website                   

                var user = new AppUser();
                user.UserName = "default";
                user.Email = "default@default.com";

                string userPWD = "somepassword";

                IdentityResult chkUser = await _userManager.CreateAsync(user, userPWD);

                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    var result1 = _userManager.AddToRoleAsync(user, "Admin");
                }
            }

            // creating Creating Manager role     
            x = await _roleManager.RoleExistsAsync("Manager");
            if (!x)
            {
                var role = new AppRole();
                role.Name = "Manager";
                await _roleManager.CreateAsync(role);

            }

            // creating Creating Employee role     
            x = await _roleManager.RoleExistsAsync("Employee");
            if (!x)
            {
                var role = new AppRole();
                role.Name = "Employee";
                await _roleManager.CreateAsync(role);
            }
        }
    } }

