using Catalog.API.Dtos;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Get a list of all books.
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            // Retrieve a list of books from the repository and return it as an HTTP response with status OK (200).
            var books = await _repository.GetProducts();
            return Ok(books);
        }

        // Get a book by its unique identifier.
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            // Attempt to retrieve a book by its unique identifier from the repository.
            var book = await _repository.GetProduct(id);

            // If the book is not found, log an error and return an HTTP response with status NotFound (404).
            if (book == null)
            {
                _logger.LogError($"Book with id: {id}, not found.");
                return NotFound();
            }

            // Return the retrieved book as an HTTP response with status OK (200).
            return Ok(book);
        }

        // Get a list of books based on their genre or category.
        [Route("[action]/{genre}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByGenre(string genre)
        {
            // Retrieve a list of books by their genre from the repository and return it as an HTTP response with status OK (200).
            var books = await _repository.GetProductByCategory(genre);
            return Ok(books);
        }

        // Create a new book.
        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] CreateProductDto request)
        {
            // Convert the incoming request into a Product object and create the book in the repository.
            var book = (Product)request;
            await _repository.CreateProduct(book);

            // Return the created book as an HTTP response with status OK (200) and a Location header pointing to the newly created resource.
            return CreatedAtRoute("GetProduct", new { id = book.Id }, book);
        }

        // Update an existing book.
        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDto request)
        {
            // Convert the incoming request into a Product object and update the book in the repository.
            var book = (Product)request;

            // Return the updated book as an HTTP response with status OK (200).
            return Ok(await _repository.UpdateProduct(book));
        }

        // Delete a book by its unique identifier.
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            // Delete a book by its unique identifier from the repository and return an HTTP response with status OK (200).
            return Ok(await _repository.DeleteProduct(id));
        }

    }
}
