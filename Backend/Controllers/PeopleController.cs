using System.Xml;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PeopleController : ControllerBase
{
    // POST /api/people {body}
    // GET /api/people
    // GET /api/people/2
    // PUT /api/people/2 {body}
    // DELETE /api/people/2


    private readonly AppDbContext _context;

    public PeopleController(AppDbContext context)
    {
        _context = context;
    }
    [HttpPost] // POST /api/people
    public async Task<IActionResult> AddPerson(Person person)
    {
        try
        {
            _context.People.Add(person);
            await _context.SaveChangesAsync();
            return CreatedAtRoute("GetPerson", new { id = person.Id }, person); // 201 Created status code +  location of the resource
            //(http://localhost:3000//api//peopl/{id})
            //  person object in the response body
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);// 500 internal server error
            // + message in the response body 
        }
    }
    [HttpGet] // GET /api/people
    public async Task<IActionResult> GetPeople()
    {
        try
        {

            var people = await _context.People.ToListAsync();
            return Ok(people); // 200 Ok status code + person object
            // in the response body
        }
        catch (XmlException ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            /*
            500 internal server error + message in th response body
            */
        }
    }
    [HttpGet("{id:int}", Name = "GetPerson")] // GET /api/people/1
    public async Task<IActionResult> GetPerson(int id)
    {
        try
        {
            var person = await _context.People.FindAsync(id);

            if (person is null)
            {
                return NotFound();  // 404 NOT FOund status code
            }

            return Ok(person);
        }
        catch (Exception ex)
        {

            return StatusCode(StatusCodes.Status500InternalServerError,
            ex.Message);
        }

    }

    [HttpPut("{id:int}")] // PUT /api/people/1
    public async Task<IActionResult> UpdatePerson(int id, [FromBody] Person person)
    {
        try
        {

            if (id != person.Id)
            {
                return BadRequest("Id in url and body mismatches");
                // 400 + message in the body
            }
            if (!await _context.People.AnyAsync(p => p.Id == id)
            )
            {
                return NotFound(); //404 NOT FOUND STATUS CODE
            }
            _context.People.Update(person);
            await _context.SaveChangesAsync();
            return NoContent(); // 204  status code +  location of the resource
            //(http://localhost:3000//api//peopl/{id})
            //  person object in the response body
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);// 500 internal server error
            // + message in the response body 
        }
    }
    [HttpDelete("{id:int}")] // DELETE /api/people/1
    public async Task<IActionResult> DeletePerson(int id)
    {
        try
        {
            var person = await _context.People.FindAsync(id);



            if (person is null)

            {
                return NotFound(); //404 NOT FOUND STATUS CODE
            }
            _context.People.Remove(person);
            await _context.SaveChangesAsync();
            return NoContent(); // 204  status code 
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);// 500 internal server error
            // + message in the response body 
        }
    }
}

