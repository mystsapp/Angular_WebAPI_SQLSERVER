using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs;
using WebAPI.Models;
using WebAPI.Repositories;

namespace WebAPI.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase {
        private readonly NorthwindContext _context;
        private readonly IMapper _mapper;
        private readonly IDataRepository<Suppliers> _repo;
        public SupplierController (NorthwindContext context, IMapper mapper, IDataRepository<Suppliers> repo) {
            _context = context;
            _mapper = mapper;
            _repo = repo;
        }

        // GET: api/Supplier
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Suppliers>>> GetSuppliers () {
            return await _context.Suppliers.ToListAsync ();
        }

        // GET: api/Supplier/5
        [HttpGet ("{id}")]
        public async Task<ActionResult<Suppliers>> GetSuppliers (int id) {
            var suppliers = await _context.Suppliers.FindAsync (id);

            if (suppliers == null) {
                return NotFound ();
            }

            return suppliers;
        }

        // PUT: api/Supplier/5
        [HttpPut ("{id}")]
        public async Task<IActionResult> PutSuppliers ([FromRoute] int id, [FromBody] EditSupplierDto editSupplierDto) {
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }

            if (id != editSupplierDto.SupplierId) {
                return BadRequest ();
            }

            var preSupplier = _mapper.Map<Suppliers> (editSupplierDto);
            _repo.Update (preSupplier);
            await _repo.SaveAsync (preSupplier);

            return NoContent ();
        }

        // POST: api/Supplier
        [HttpPost]
        public async Task<ActionResult<Suppliers>> PostSuppliers ([FromBody] AddSupplierDto addSupplierDto) {
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }

            var preSupplier = _mapper.Map<Suppliers> (addSupplierDto);
            _repo.Add (preSupplier);
            var saveSupplier = await _repo.SaveAsync (preSupplier);
            var supplierResponse = _mapper.Map<SupplierResponseDto> (saveSupplier);

            return StatusCode (201, new { supplierResponse });
        }

        // DELETE: api/Supplier/5
        [HttpDelete ("{id}")]
        public async Task<IActionResult> DeleteSuppliers ([FromRoute] int id) {
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }

            var suppliers = await _context.Suppliers.FindAsync (id);

            if (suppliers == null) {
                return NotFound ();
            }

            _context.Database.ExecuteSqlCommand ("DELETE FROM [Order Details] WHERE ProductID IN (SELECT ProductID FROM Products WHERE SupplierID = @supplierId)",
                new SqlParameter ("@supplierId", suppliers.SupplierId));

            _context.Database.ExecuteSqlCommand ("DELETE FROM Products WHERE SupplierID = @supplierId",
                new SqlParameter ("@supplierId", suppliers.SupplierId));

            _context.Suppliers.Remove (suppliers);
            await _context.SaveChangesAsync ();

            return Ok (suppliers);
        }

        private bool SuppliersExists (int id) {
            return _context.Suppliers.Any (e => e.SupplierId == id);
        }
    }
}