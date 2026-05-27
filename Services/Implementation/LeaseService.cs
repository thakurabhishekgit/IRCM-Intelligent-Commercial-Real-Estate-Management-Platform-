using IRCM.Data;
using IRCM.DTOs.Lease;
using IRCM.DTOs.LeaseRequest;
using IRCM.Enums;
using IRCM.Interfaces;
using IRCM.Models;
using Microsoft.EntityFrameworkCore;

namespace IRCM.Services.Implementation;

public class LeaseService
    : ILeaseService
{
    private readonly ApplicationDbContext
        _context;

    public LeaseService(
        ApplicationDbContext context
    )
    {
        _context = context;
    }

    // =========================
    // CREATE LEASE
    // =========================

    public async Task<LeaseResponseDto?>
        CreateLeaseAsync(
            Guid agentId,
            CreateLeaseDto dto
        )
    {
        var leaseRequest =
            await _context.LeaseRequests
                .Include(x => x.Property)
                .Include(x => x.Tenant)
                .Include(x => x.Agent)

                .FirstOrDefaultAsync(x =>
                    x.Id ==
                        dto.LeaseRequestId &&
                    x.Status ==
                        LeaseRequestStatus
                            .Approved
                );

        if (leaseRequest == null)
        {
            return null;
        }

        if (
            leaseRequest.AgentId !=
            agentId
        )
        {
            return null;
        }

        // Prevent duplicate active lease

        var existingLease =
            await _context.Leases
                .FirstOrDefaultAsync(x =>
                    x.PropertyId ==
                        dto.PropertyId &&
                    x.TenantId ==
                        dto.TenantId &&
                    x.Status ==
                        LeaseStatus.Active
                );

        if (existingLease != null)
        {
            return null;
        }

        var lease = new Lease
        {
            PropertyId = dto.PropertyId,

            TenantId = dto.TenantId,

            AgentId = agentId,

            LeaseRequestId =
                dto.LeaseRequestId,

            MonthlyRent =
                dto.MonthlyRent,

            SecurityDeposit =
                dto.SecurityDeposit,

            StartDate =
                dto.StartDate,

            EndDate =
                dto.EndDate
        };

        await _context.Leases
            .AddAsync(lease);

        await _context.SaveChangesAsync();

        return await MapLeaseResponse(
            lease.Id
        );
    }

    // =========================
    // AGENT LEASES
    // =========================

    public async Task<
        List<LeaseResponseDto>
    >
    GetAgentLeasesAsync(Guid agentId)
    {
        var leases =
            await _context.Leases
                .Where(x =>
                    x.AgentId == agentId
                )
                .Select(x => x.Id)
                .ToListAsync();

        var result =
            new List<LeaseResponseDto>();

        foreach (var leaseId in leases)
        {
            var dto =
                await MapLeaseResponse(
                    leaseId
                );

            if (dto != null)
            {
                result.Add(dto);
            }
        }

        return result;
    }

    // =========================
    // TENANT LEASES
    // =========================

    public async Task<
        List<LeaseResponseDto>
    >
    GetTenantLeasesAsync(
        Guid tenantId
    )
    {
        var leases =
            await _context.Leases
                .Where(x =>
                    x.TenantId == tenantId
                )
                .Select(x => x.Id)
                .ToListAsync();

        var result =
            new List<LeaseResponseDto>();

        foreach (var leaseId in leases)
        {
            var dto =
                await MapLeaseResponse(
                    leaseId
                );

            if (dto != null)
            {
                result.Add(dto);
            }
        }

        return result;
    }

    // =========================
    // PROPERTY LEASES
    // =========================

    public async Task<
        List<LeaseResponseDto>
    >
    GetPropertyLeasesAsync(
        Guid propertyId,
        Guid agentId
    )
    {
        var leases =
            await _context.Leases
                .Where(x =>
                    x.PropertyId ==
                        propertyId &&
                    x.AgentId == agentId
                )
                .Select(x => x.Id)
                .ToListAsync();

        var result =
            new List<LeaseResponseDto>();

        foreach (var leaseId in leases)
        {
            var dto =
                await MapLeaseResponse(
                    leaseId
                );

            if (dto != null)
            {
                result.Add(dto);
            }
        }

        return result;
    }

    // =========================
    // GET LEASE BY ID
    // =========================

    public async Task<LeaseResponseDto?>
        GetLeaseByIdAsync(
            Guid leaseId,
            Guid userId,
            string role
        )
    {
        var lease =
            await _context.Leases
                .FirstOrDefaultAsync(x =>
                    x.Id == leaseId
                );

        if (lease == null)
        {
            return null;
        }

        if (
            role == "Tenant" &&
            lease.TenantId != userId
        )
        {
            return null;
        }

        if (
            role == "Agent" &&
            lease.AgentId != userId
        )
        {
            return null;
        }

        return await MapLeaseResponse(
            leaseId
        );
    }

    // =========================
    // UPDATE STATUS
    // =========================

    public async Task<LeaseResponseDto?>
        UpdateLeaseStatusAsync(
            Guid leaseId,
            Guid agentId,
            LeaseStatus status
        )
    {
        var lease =
            await _context.Leases
                .FirstOrDefaultAsync(x =>
                    x.Id == leaseId &&
                    x.AgentId == agentId
                );

        if (lease == null)
        {
            return null;
        }

        lease.Status = status;

        lease.UpdatedAt =
            DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await MapLeaseResponse(
            lease.Id
        );
    }

    // =========================
    // MAPPER
    // =========================

    private async Task<
        LeaseResponseDto?
    >
    MapLeaseResponse(Guid leaseId)
    {
        return await _context.Leases
            .Include(x => x.Property)
            .Include(x => x.Tenant)
            .Include(x => x.Agent)

            .Select(x =>
                new LeaseResponseDto
                {
                    Id = x.Id,

                    MonthlyRent =
                        x.MonthlyRent,

                    SecurityDeposit =
                        x.SecurityDeposit,

                    StartDate =
                        x.StartDate,

                    EndDate =
                        x.EndDate,

                    Status =
                        x.Status.ToString(),

                    CreatedAt =
                        x.CreatedAt,

                    Property =
                        new PropertyDto
                        {
                            Id =
                                x.Property.Id,

                            Title =
                                x.Property.Title,

                            Location =
                                x.Property
                                    .Location,

                            Price =
                                x.Property
                                    .Price,

                            ThumbnailUrl =
                                x.Property
                                    .ThumbnailUrl
                        },

                    Tenant =
                        new TenantDto
                        {
                            Id =
                                x.Tenant.Id,

                            FullName =
                                x.Tenant
                                    .FullName,

                            Email =
                                x.Tenant
                                    .Email
                        },

                    Agent =
                        new AgentDto
                        {
                            Id =
                                x.Agent.Id,

                            FullName =
                                x.Agent
                                    .FullName,

                            Email =
                                x.Agent
                                    .Email
                        }
                }
            )
            .FirstOrDefaultAsync(x =>
                x.Id == leaseId
            );
    }
}