using IRCM.Data;
using IRCM.DTOs.LeaseRequest;
using IRCM.Enums;
using IRCM.Interfaces;
using IRCM.Models;
using Microsoft.EntityFrameworkCore;

namespace IRCM.Services.Implementation;

public class LeaseRequestImplementation
    : ILeaseRequestService
{
    private readonly ApplicationDbContext _context;

    public LeaseRequestImplementation(
        ApplicationDbContext context
    )
    {
        _context = context;
    }

    // =========================
    // CREATE REQUEST
    // =========================

    public async Task<LeaseRequestResponseDto?>
        CreateLeaseRequestAsync(
            Guid tenantId,
            CreateLeaseRequestDto dto
        )
    {
        var property = await _context.Properties
            .Include(x => x.Agent)
            .FirstOrDefaultAsync(
                x => x.Id == dto.PropertyId
            );

        if (property == null)
        {
            return null;
        }

        // Prevent self request

        if (property.AgentId == tenantId)
        {
            return null;
        }

        // Prevent duplicate pending request

        var existingRequest =
            await _context.LeaseRequests
                .FirstOrDefaultAsync(x =>
                    x.PropertyId == dto.PropertyId &&
                    x.TenantId == tenantId &&
                    x.Status ==
                        LeaseRequestStatus.Pending
                );

        if (existingRequest != null)
        {
            return null;
        }

        var leaseRequest = new LeaseRequest
        {
            PropertyId = dto.PropertyId,

            TenantId = tenantId,

            AgentId = property.AgentId,

            Message = dto.Message
        };

        await _context.LeaseRequests
            .AddAsync(leaseRequest);

        await _context.SaveChangesAsync();

        var tenant = await _context.Users
            .FirstAsync(x => x.Id == tenantId);

        return new LeaseRequestResponseDto
        {
            Id = leaseRequest.Id,

            Message = leaseRequest.Message,

            Status =
                leaseRequest.Status.ToString(),

            RequestedAt =
                leaseRequest.RequestedAt,

            ReviewedAt =
                leaseRequest.ReviewedAt,

            Property = new PropertyDto
            {
                Id = property.Id,
                Title = property.Title,
                Location = property.Location,
                Price = property.Price,
                ThumbnailUrl =
                    property.ThumbnailUrl
            },

            Tenant = new TenantDto
            {
                Id = tenant.Id,
                FullName = tenant.FullName,
                Email = tenant.Email
            },

            Agent = new AgentDto
            {
                Id = property.Agent.Id,
                FullName =
                    property.Agent.FullName,
                Email =
                    property.Agent.Email
            }
        };
    }

    // =========================
    // TENANT REQUESTS
    // =========================

    public async Task<
        List<LeaseRequestResponseDto>
    >
    GetMyLeaseRequestsAsync(Guid tenantId)
    {
        return await _context.LeaseRequests
            .Include(x => x.Property)
            .Include(x => x.Tenant)
            .Include(x => x.Agent)

            .Where(x => x.TenantId == tenantId)

            .Select(x =>
                new LeaseRequestResponseDto
                {
                    Id = x.Id,

                    Message = x.Message,

                    Status =
                        x.Status.ToString(),

                    RequestedAt =
                        x.RequestedAt,

                    ReviewedAt =
                        x.ReviewedAt,

                    Property = new PropertyDto
                    {
                        Id = x.Property.Id,
                        Title =
                            x.Property.Title,
                        Location =
                            x.Property.Location,
                        Price =
                            x.Property.Price,
                        ThumbnailUrl =
                            x.Property
                                .ThumbnailUrl
                    },

                    Tenant = new TenantDto
                    {
                        Id = x.Tenant.Id,
                        FullName =
                            x.Tenant.FullName,
                        Email =
                            x.Tenant.Email
                    },

                    Agent = new AgentDto
                    {
                        Id = x.Agent.Id,
                        FullName =
                            x.Agent.FullName,
                        Email =
                            x.Agent.Email
                    }
                }
            )
            .ToListAsync();
    }

    // =========================
    // AGENT REQUESTS
    // =========================

    public async Task<
        List<LeaseRequestResponseDto>
    >
    GetAgentLeaseRequestsAsync(Guid agentId)
    {
        return await _context.LeaseRequests
            .Include(x => x.Property)
            .Include(x => x.Tenant)
            .Include(x => x.Agent)

            .Where(x => x.AgentId == agentId)

            .Select(x =>
                new LeaseRequestResponseDto
                {
                    Id = x.Id,

                    Message = x.Message,

                    Status =
                        x.Status.ToString(),

                    RequestedAt =
                        x.RequestedAt,

                    ReviewedAt =
                        x.ReviewedAt,

                    Property = new PropertyDto
                    {
                        Id = x.Property.Id,
                        Title =
                            x.Property.Title,
                        Location =
                            x.Property.Location,
                        Price =
                            x.Property.Price,
                        ThumbnailUrl =
                            x.Property
                                .ThumbnailUrl
                    },

                    Tenant = new TenantDto
                    {
                        Id = x.Tenant.Id,
                        FullName =
                            x.Tenant.FullName,
                        Email =
                            x.Tenant.Email
                    },

                    Agent = new AgentDto
                    {
                        Id = x.Agent.Id,
                        FullName =
                            x.Agent.FullName,
                        Email =
                            x.Agent.Email
                    }
                }
            )
            .ToListAsync();
    }

    // =========================
    // UPDATE STATUS
    // =========================

    public async Task<LeaseRequestResponseDto?>
        UpdateLeaseRequestStatusAsync(
            Guid requestId,
            Guid agentId,
            LeaseRequestStatus status
        )
    {
        var request =
            await _context.LeaseRequests
                .Include(x => x.Property)
                .Include(x => x.Tenant)
                .Include(x => x.Agent)

                .FirstOrDefaultAsync(x =>
                    x.Id == requestId &&
                    x.AgentId == agentId
                );

        if (request == null)
        {
            return null;
        }

        request.Status = status;

        request.ReviewedAt =
            DateTime.UtcNow;

        // =========================
        // APPROVED
        // =========================

        if (
            status ==
            LeaseRequestStatus.Approved
        )
        {
            request.Property.OccupiedUnits += 1;
        }

        await _context.SaveChangesAsync();

        return new LeaseRequestResponseDto
        {
            Id = request.Id,

            Message = request.Message,

            Status =
                request.Status.ToString(),

            RequestedAt =
                request.RequestedAt,

            ReviewedAt =
                request.ReviewedAt,

            Property = new PropertyDto
            {
                Id = request.Property.Id,
                Title =
                    request.Property.Title,
                Location =
                    request.Property.Location,
                Price =
                    request.Property.Price,
                ThumbnailUrl =
                    request.Property
                        .ThumbnailUrl
            },

            Tenant = new TenantDto
            {
                Id = request.Tenant.Id,
                FullName =
                    request.Tenant.FullName,
                Email =
                    request.Tenant.Email
            },

            Agent = new AgentDto
            {
                Id = request.Agent.Id,
                FullName =
                    request.Agent.FullName,
                Email =
                    request.Agent.Email
            }
        };
    }

    public async Task<
    List<LeaseRequestResponseDto>
        >
    GetRequestsByPropertyAsync(
        Guid propertyId,
        Guid agentId
    )
    {
        return await _context.LeaseRequests
            .Include(x => x.Property)
            .Include(x => x.Tenant)
            .Include(x => x.Agent)

            .Where(x =>
                x.PropertyId == propertyId &&
                x.AgentId == agentId
            )

            .Select(x =>
                new LeaseRequestResponseDto
                {
                    Id = x.Id,

                    Message = x.Message,

                    Status =
                        x.Status.ToString(),

                    RequestedAt =
                        x.RequestedAt,

                    ReviewedAt =
                        x.ReviewedAt,

                    Property = new PropertyDto
                    {
                        Id = x.Property.Id,

                        Title =
                            x.Property.Title,

                        Location =
                            x.Property.Location,

                        Price =
                            x.Property.Price,

                        ThumbnailUrl =
                            x.Property
                                .ThumbnailUrl
                    },

                    Tenant = new TenantDto
                    {
                        Id = x.Tenant.Id,

                        FullName =
                            x.Tenant.FullName,

                        Email =
                            x.Tenant.Email
                    },

                    Agent = new AgentDto
                    {
                        Id = x.Agent.Id,

                        FullName =
                            x.Agent.FullName,

                        Email =
                            x.Agent.Email
                    }
                }
            )
            .ToListAsync();
    }
}