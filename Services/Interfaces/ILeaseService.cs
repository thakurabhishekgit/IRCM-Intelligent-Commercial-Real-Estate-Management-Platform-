using IRCM.DTOs.Lease;
using IRCM.Enums;

namespace IRCM.Interfaces;

public interface ILeaseService
{
    Task<LeaseResponseDto?>
        CreateLeaseAsync(
            Guid agentId,
            CreateLeaseDto dto
        );

    Task<List<LeaseResponseDto>>
        GetAgentLeasesAsync(
            Guid agentId
        );

    Task<List<LeaseResponseDto>>
        GetTenantLeasesAsync(
            Guid tenantId
        );

    Task<List<LeaseResponseDto>>
        GetPropertyLeasesAsync(
            Guid propertyId,
            Guid agentId
        );

    Task<LeaseResponseDto?>
        GetLeaseByIdAsync(
            Guid leaseId,
            Guid userId,
            string role
        );

    Task<LeaseResponseDto?>
        UpdateLeaseStatusAsync(
            Guid leaseId,
            Guid agentId,
            LeaseStatus status
        );
}