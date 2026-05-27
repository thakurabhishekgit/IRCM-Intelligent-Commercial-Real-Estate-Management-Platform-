using System.Security.Claims;
using IRCM.DTOs.Lease;
using IRCM.Enums;
using IRCM.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IRCM.Controllers;

[ApiController]
[Route("api/lease")]
public class LeaseController
    : ControllerBase
{
    private readonly ILeaseService
        _leaseService;

    public LeaseController(
        ILeaseService leaseService
    )
    {
        _leaseService = leaseService;
    }

    // =========================
    // CREATE LEASE
    // =========================

    [Authorize(Roles = "Agent,Admin")]
    [HttpPost]
    public async Task<IActionResult>
        CreateLease(CreateLeaseDto dto)
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier
        );

        var lease =
            await _leaseService
                .CreateLeaseAsync(
                    Guid.Parse(userId!),
                    dto
                );

        if (lease == null)
        {
            return BadRequest(new
            {
                success = false,
                message =
                    "Lease creation failed"
            });
        }

        return Ok(new
        {
            success = true,
            message =
                "Lease created successfully",
            data = lease
        });
    }

    // =========================
    // AGENT LEASES
    // =========================

    [Authorize(Roles = "Agent,Admin")]
    [HttpGet("agent")]
    public async Task<IActionResult>
        GetAgentLeases()
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier
        );

        var leases =
            await _leaseService
                .GetAgentLeasesAsync(
                    Guid.Parse(userId!)
                );

        return Ok(new
        {
            success = true,
            count = leases.Count,
            data = leases
        });
    }

    // =========================
    // TENANT LEASES
    // =========================

    [Authorize(Roles = "Tenant")]
    [HttpGet("my-leases")]
    public async Task<IActionResult>
        GetTenantLeases()
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier
        );

        var leases =
            await _leaseService
                .GetTenantLeasesAsync(
                    Guid.Parse(userId!)
                );

        return Ok(new
        {
            success = true,
            count = leases.Count,
            data = leases
        });
    }

    // =========================
    // LEASE BY ID
    // =========================

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult>
        GetLeaseById(Guid id)
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier
        );

        var role = User.FindFirstValue(
            ClaimTypes.Role
        );

        var lease =
            await _leaseService
                .GetLeaseByIdAsync(
                    id,
                    Guid.Parse(userId!),
                    role!
                );

        if (lease == null)
        {
            return NotFound(new
            {
                success = false,
                message =
                    "Lease not found"
            });
        }

        return Ok(new
        {
            success = true,
            data = lease
        });
    }

    // =========================
    // PROPERTY LEASES
    // =========================

    [Authorize(Roles = "Agent,Admin")]
    [HttpGet(
        "property/{propertyId}"
    )]
    public async Task<IActionResult>
        GetPropertyLeases(
            Guid propertyId
        )
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier
        );

        var leases =
            await _leaseService
                .GetPropertyLeasesAsync(
                    propertyId,
                    Guid.Parse(userId!)
                );

        return Ok(new
        {
            success = true,
            count = leases.Count,
            data = leases
        });
    }

    // =========================
    // ACTIVATE
    // =========================

    [Authorize(Roles = "Agent,Admin")]
    [HttpPut("{id}/activate")]
    public async Task<IActionResult>
        ActivateLease(Guid id)
    {
        return await UpdateStatus(
            id,
            LeaseStatus.Active
        );
    }

    // =========================
    // EXPIRE
    // =========================

    [Authorize(Roles = "Agent,Admin")]
    [HttpPut("{id}/expire")]
    public async Task<IActionResult>
        ExpireLease(Guid id)
    {
        return await UpdateStatus(
            id,
            LeaseStatus.Expired
        );
    }

    // =========================
    // TERMINATE
    // =========================

    [Authorize(Roles = "Agent,Admin")]
    [HttpPut("{id}/terminate")]
    public async Task<IActionResult>
        TerminateLease(Guid id)
    {
        return await UpdateStatus(
            id,
            LeaseStatus.Terminated
        );
    }

    // =========================
    // COMMON STATUS METHOD
    // =========================

    private async Task<IActionResult>
        UpdateStatus(
            Guid leaseId,
            LeaseStatus status
        )
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier
        );

        var lease =
            await _leaseService
                .UpdateLeaseStatusAsync(
                    leaseId,
                    Guid.Parse(userId!),
                    status
                );

        if (lease == null)
        {
            return NotFound(new
            {
                success = false,
                message =
                    "Lease not found"
            });
        }

        return Ok(new
        {
            success = true,
            message =
                $"Lease marked as {status}",
            data = lease
        });
    }
}