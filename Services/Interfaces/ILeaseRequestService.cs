using IRCM.DTOs.LeaseRequest;
using IRCM.Enums;

namespace IRCM.Interfaces;

public interface ILeaseRequestService
{
    Task<LeaseRequestResponseDto?>
        CreateLeaseRequestAsync(
            Guid tenantId,
            CreateLeaseRequestDto dto
        );

    Task<List<LeaseRequestResponseDto>>
        GetMyLeaseRequestsAsync(Guid tenantId);

    Task<List<LeaseRequestResponseDto>>
        GetAgentLeaseRequestsAsync(Guid agentId);

    Task<LeaseRequestResponseDto?>
        UpdateLeaseRequestStatusAsync(
            Guid requestId,
            Guid agentId,
            LeaseRequestStatus status
        );
}