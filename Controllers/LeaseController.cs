using System.Security.Claims;
using IRCM.DTOs.LeaseRequest;
using IRCM.Enums;
using IRCM.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IRCM.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaseRequestController : ControllerBase
{
    private readonly ILeaseRequestService
        _leaseRequestService;

    public LeaseRequestController(
        ILeaseRequestService leaseRequestService
    )
    {
        _leaseRequestService =
            leaseRequestService;
    }

    // =========================
    // TENANT SEND REQUEST
    // =========================

    [Authorize(Roles = "Tenant")]
    [HttpPost]
    public async Task<IActionResult>
        CreateLeaseRequest(
            CreateLeaseRequestDto dto
        )
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier
        );

        var request =
            await _leaseRequestService
                .CreateLeaseRequestAsync(
                    Guid.Parse(userId!),
                    dto
                );

        if (request == null)
        {
            return BadRequest(new
            {
                success = false,
                message =
                    "Invalid request or duplicate request"
            });
        }

        return StatusCode(201, new
        {
            success = true,
            message =
                "Lease request sent successfully",
            data = request
        });
    }

    // =========================
    // TENANT GET OWN REQUESTS
    // =========================

    [Authorize(Roles = "Tenant")]
    [HttpGet("my-requests")]
    public async Task<IActionResult>
        GetMyLeaseRequests()
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier
        );

        var requests =
            await _leaseRequestService
                .GetMyLeaseRequestsAsync(
                    Guid.Parse(userId!)
                );

        return Ok(new
        {
            success = true,
            count = requests.Count,
            data = requests
        });
    }

    // =========================
    // AGENT GET REQUESTS
    // =========================

    [Authorize(Roles = "Agent,Admin")]
    [HttpGet("agent")]
    public async Task<IActionResult>
        GetAgentLeaseRequests()
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier
        );

        var requests =
            await _leaseRequestService
                .GetAgentLeaseRequestsAsync(
                    Guid.Parse(userId!)
                );

        return Ok(new
        {
            success = true,
            count = requests.Count,
            data = requests
        });
    }

    // =========================
    // APPROVE REQUEST
    // =========================

    [Authorize(Roles = "Agent,Admin")]
    [HttpPut("{id}/approve")]
    public async Task<IActionResult>
        ApproveLeaseRequest(Guid id)
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier
        );

        var request =
            await _leaseRequestService
                .UpdateLeaseRequestStatusAsync(
                    id,
                    Guid.Parse(userId!),
                    LeaseRequestStatus.Approved
                );

        if (request == null)
        {
            return NotFound(new
            {
                success = false,
                message =
                    "Lease request not found or unauthorized"
            });
        }

        return Ok(new
        {
            success = true,
            message =
                "Lease request approved successfully",
            data = request
        });
    }

    // =========================
    // REJECT REQUEST
    // =========================

    [Authorize(Roles = "Agent,Admin")]
    [HttpPut("{id}/reject")]
    public async Task<IActionResult>
        RejectLeaseRequest(Guid id)
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier
        );

        var request =
            await _leaseRequestService
                .UpdateLeaseRequestStatusAsync(
                    id,
                    Guid.Parse(userId!),
                    LeaseRequestStatus.Rejected
                );

        if (request == null)
        {
            return NotFound(new
            {
                success = false,
                message =
                    "Lease request not found or unauthorized"
            });
        }

        return Ok(new
        {
            success = true,
            message =
                "Lease request rejected successfully",
            data = request
        });
    }
}