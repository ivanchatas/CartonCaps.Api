using Application.Dtos;
using Application.Interfaces.Services;
using Application.Wrapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferralsController : ControllerBase
    {
        private readonly IReferralService _service;

        public ReferralsController(IReferralService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all referrals
        /// </summary>
        /// <returns>All referrals</returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<ReferralResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll() 
            => Ok(await _service.GetAll());

        [HttpPost]
        [ProducesResponseType(typeof(Result<ReferralResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateInvitation([FromBody] CreateInvitationRequest request)
            => Ok(await _service.CreateInvitation(request));

        /// <summary>
        /// Get referrals by referrer user id
        /// </summary>
        /// <param name="userId">Referrer User Id (GUID).</param>
        /// <returns>All the invitations by the Referrer User</returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(Result<StatusReferralResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetInvitations(string userId)
            => Ok(await _service.GetInvitations(userId));

        /// <summary>
        /// Obtiene un producto por su identificador único.
        /// </summary>
        /// <param name="userId">Referrer User Id (GUID).</param>
        /// <returns>If Referral Code was completed already returns an exception message</returns>
        [HttpGet("resolve/{referralId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResolveReferral(string referralId)
        {
            await _service.ResolveReferral(referralId);
            return Ok();
        }
    }
}
