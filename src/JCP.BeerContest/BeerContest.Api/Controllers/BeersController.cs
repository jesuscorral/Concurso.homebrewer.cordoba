using BeerContest.Api.Models;
using BeerContest.Application.Features.Beers.Commands.RegisterBeer;
using BeerContest.Application.Features.Beers.Commands.DeleteBeer;
using BeerContest.Application.Features.Beers.Commands.UpdateBeer;
using BeerContest.Application.Features.Beers.Queries.GetBeerById;
using BeerContest.Application.Features.Beers.Queries.GetParticipantBeers;
using BeerContest.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BeerContest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BeersController : ControllerBase
{
    private readonly IMediator _mediator;

    public BeersController(IMediator mediator)
    {
        _mediator = mediator;
    }    [HttpGet]
    public async Task<IActionResult> GetUserBeers()
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(userEmail))
            return Unauthorized();

        var query = new GetParticipantBeersQuery { ParticipantEmail = userEmail };
        var beers = await _mediator.Send(query);
        return Ok(beers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBeer(string id)
    {
        var query = new GetBeerByIdQuery { Id = id };
        var beer = await _mediator.Send(query);
        
        if (beer == null)
            return NotFound();

        return Ok(beer);
    }    [HttpPost]
    public async Task<IActionResult> CreateBeer([FromBody] RegisterBeerDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();        var command = new RegisterBeerCommand
        {
            Category = dto.Category,
            BeerStyle = dto.BeerStyle,
            AlcoholContent = dto.AlcoholContent,
            ElaborationDate = dto.ElaborationDate,
            BottleDate = dto.BottleDate,
            Malts = dto.Malts,
            Hops = dto.Hops,
            Yeast = dto.Yeast,
            Additives = dto.Additives,
            ParticipantId = userId,
            ContestId = dto.ContestId ?? string.Empty
        };

        var beerId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetBeer), new { id = beerId }, new { Id = beerId });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBeer(string id, [FromBody] UpdateBeerCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        
        if (string.IsNullOrEmpty(result))
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBeer(string id)
    {
        var command = new DeleteBeerCommand { BeerId = id };
        var success = await _mediator.Send(command);
        
        if (!success)
            return NotFound();

        return NoContent();
    }
}
