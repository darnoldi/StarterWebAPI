﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
    private readonly IMapper _mapper;

    public AccountsController(UserManager<AppUser> userManager, IMapper mapper, PacktDbContext appDbContext)
    {
        _userManager = userManager;
        _mapper = mapper;
        _appDbContext = appDbContext;
    }

    // POST api/accounts
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]RegistrationDto model)
    {
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
} }
