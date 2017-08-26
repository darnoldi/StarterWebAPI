using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StarterAPI.Dtos;
using StarterAPI.Entities;
using StarterAPI.QueryParameters;
using StarterAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterAPI.Controllers
{
    [Route("api/[controller]")]

    public class CustomersController : Controller

    {
        private readonly ILogger<CustomersController> _logger;
        private ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository, ILogger<CustomersController> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
            _logger.LogInformation("customerscontroler started");
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllCustomers(CustomerQueryParameters customerQueryParameters)
        {
            var allCustomers = _customerRepository.GetAll(customerQueryParameters).ToList();

            if (allCustomers.Count == 0)
            {
                return NotFound();
            }

            var allCustomersDto = allCustomers.Select(x => Mapper.Map<CustomerDto>(x));

            Response.Headers.Add("X-Pagination",
                JsonConvert.SerializeObject(new { totalCount = _customerRepository.Count() }));

            

            return Ok(allCustomersDto);
        }


        [HttpGet]
        [Route("{id}", Name = "GetSingleCustomer")]
        public IActionResult GetSingleCustomer(Guid id)
        {
            Customer customerFromRepo = _customerRepository.GetSingle(id);

            if (customerFromRepo == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<CustomerDto>(customerFromRepo));
        }



        [HttpPost]
        [ProducesResponseType(typeof(CustomerCreateDto), 201)]
        [ProducesResponseType(typeof(CustomerCreateDto), 400)]
        public IActionResult AddCustomer([FromBody] CustomerCreateDto customerCreateDto)
        {

            if (customerCreateDto == null)
            {
                return BadRequest("Empty Customer Object");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Customer ToAdd = Mapper.Map<Customer>(customerCreateDto);
            _customerRepository.Add(ToAdd);

            bool result = _customerRepository.Save();

            if (!result)
            {
                //return new StatusCodeResult(500);
                throw new Exception("Error Adding Customer");

            }

            // return Ok(Mapper.Map<CustomerDto>(ToAdd));
            return CreatedAtRoute("GetSingleCustomer", new { id = ToAdd.Id }, Mapper.Map<CustomerDto>(ToAdd));

        }

        [HttpPut]
        [ProducesResponseType(typeof(CustomerUpdateDto), 200)]
        [ProducesResponseType(typeof(CustomerUpdateDto), 400)]
        [ProducesResponseType(typeof(CustomerUpdateDto), 404)]
        [ProducesResponseType(typeof(CustomerUpdateDto), 500)]
        [Route("{id}")]
        public IActionResult UpdateCustomer(Guid id, [FromBody] CustomerUpdateDto updatedto)
        {
            if (updatedto  == null)
            {
                return BadRequest("Empty Customer Object");
            }

            var existingCustomer = _customerRepository.GetSingle(id);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(updatedto, existingCustomer);

            _customerRepository.Update(existingCustomer);

            bool result = _customerRepository.Save();

            if (!result)
            {
                //return new StatusCodeResult(500);
                throw new Exception("Error Updating Customer");
            }

            return Ok(Mapper.Map<CustomerDto>(existingCustomer));
        }

        [HttpPatch]
        [ProducesResponseType(typeof(CustomerUpdateDto), 200)]
        [ProducesResponseType(typeof(CustomerUpdateDto), 400)]
        [ProducesResponseType(typeof(CustomerUpdateDto), 404)]
        [ProducesResponseType(typeof(CustomerUpdateDto), 500)]
        [Route("{id}")]
        public IActionResult PartiallyUpdate(Guid id, [FromBody] JsonPatchDocument<CustomerUpdateDto> customerPatchDoc)
        {
            if (customerPatchDoc == null)
            {
                return BadRequest();
            }

            var existingCustomer = _customerRepository.GetSingle(id);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            var customerToPatch = Mapper.Map<CustomerUpdateDto>(existingCustomer);
            customerPatchDoc.ApplyTo(customerToPatch, ModelState);

            TryValidateModel(customerToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(customerToPatch, existingCustomer);

            _customerRepository.Update(existingCustomer);

            bool result = _customerRepository.Save();

            if (!result)
            {
                //return new StatusCodeResult(500);
                throw new Exception("Error Patching Customer");
            }

            return Ok(Mapper.Map<CustomerDto>(existingCustomer));
        }


        [HttpDelete]
        [ProducesResponseType(typeof(Guid), 204)]
        [ProducesResponseType(typeof(Guid), 404)]
        [ProducesResponseType(typeof(Guid), 500)]
        [Route("{id}")]
        public IActionResult Remove(Guid id)
        {
            var existingCustomer = _customerRepository.GetSingle(id);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            _customerRepository.Delete(id);

            bool result = _customerRepository.Save();

            if (!result)
            {
                //return new StatusCodeResult(500);
                throw new Exception("Error Deleting Customer");
            }

            return NoContent();
        }

    }
}
