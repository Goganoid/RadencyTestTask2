using Application.Books;
using Application.Books.DTO;
using Application.Books.DTO.Requests;
using Application.Books.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController : ControllerBase
{
    private readonly BookService _bookService;
    private readonly IConfiguration _configuration;
    private readonly IValidator<SaveBookRequestDTO> _bookValidator;
    private readonly IValidator<SaveReviewRequestDTO> _reviewValidator;
    private readonly IValidator<RateBookRequestDTO> _scoreValidator;

    public BooksController(BookService bookService, IConfiguration configuration,
        IValidator<SaveBookRequestDTO> bookValidator, IValidator<SaveReviewRequestDTO> reviewValidator,
        IValidator<RateBookRequestDTO> scoreValidator)
    {
        _bookService = bookService;
        _configuration = configuration;
        _bookValidator = bookValidator;
        _reviewValidator = reviewValidator;
        _scoreValidator = scoreValidator;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetBooks([FromQuery] BookOrderOptions order)
    {
        var books = await _bookService.GetBooks(order);
        return Ok(books);
    }

    [HttpGet]
    [Route("recommended")]
    public async Task<IActionResult> GetRecommendedBooks([FromQuery] string? genre)
    {
        var books = await _bookService.GetRecommendedBooks(genre);
        return Ok(books);
    }

    [HttpGet]
    [Route("{bookId:int}")]
    public async Task<IActionResult> GetBookDetails(int bookId)
    {
        var books = await _bookService.GetBookDetails(bookId);
        return Ok(books);
    }

    [HttpDelete]
    [Route("{bookId:int}")]
    public async Task<IActionResult> DeleteBook(int bookId, [FromQuery] string secret)
    {
        if (_configuration["SecretKey"] != secret) return Unauthorized();
        await _bookService.DeleteBook(bookId);
        return Ok();
    }

    [HttpPost]
    [Route("save")]
    public async Task<IActionResult> SaveBook([FromBody] SaveBookRequestDTO saveBookRequestDTO)
    {
        var result = await _bookValidator.ValidateAsync(saveBookRequestDTO);
        if (!result.IsValid) return BadRequest(result.Errors);
        var id = await _bookService.SaveBook(saveBookRequestDTO);
        return Ok(id);
    }

    [HttpPut]
    [Route("{bookId:int}/review")]
    public async Task<IActionResult> SaveReview(int bookId, [FromBody] SaveReviewRequestDTO saveReviewRequestDTO)
    {
        var result = await _reviewValidator.ValidateAsync(saveReviewRequestDTO);
        if (!result.IsValid) return BadRequest(result.Errors);
        var id = await _bookService.SaveReview(bookId, saveReviewRequestDTO);
        return Ok(id);
    }

    [HttpPut]
    [Route("{bookId:int}/rate")]
    public async Task<IActionResult> RateBook(int bookId, [FromBody] RateBookRequestDTO rateBookRequestDTO)
    {
        var result = await _scoreValidator.ValidateAsync(rateBookRequestDTO);
        if (!result.IsValid) return BadRequest(result.Errors);
        await _bookService.RateBook(bookId, rateBookRequestDTO);
        return Ok();
    }
}