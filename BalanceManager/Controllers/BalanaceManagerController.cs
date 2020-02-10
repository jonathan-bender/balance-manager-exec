using System;
using System.Web.Http;
using BalanceManager.Services;

namespace BalanceManager.Controllers
{
    [RoutePrefix("api/balances/v1")]
    public class BalanaceManagerController : ApiController
    {
        private readonly BalanceService _balanceService;

        public BalanaceManagerController(BalanceService balanceService)
        {
            _balanceService = balanceService;
        }

        [Route("createbalance")]
        [HttpPost]
        public IHttpActionResult CreateBalance()
        {
            try
            {
                var balanceInfo = _balanceService.CreateBalance();

                return Ok(balanceInfo.BalanceId);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("getbalanceinfo/{balanceId:int}")]
        public IHttpActionResult GetBalanceInfo(int balanceId)
        {
            var balanceInfo = _balanceService.GetById(balanceId);
            if (balanceInfo == null)
                return NotFound();

            return Ok(balanceInfo);
        }

        [Route("charge/{balanceId:int}/{amount:float}")]
        [HttpPost]
        public IHttpActionResult Charge(int balanceId, float amount)
        {
            if (amount < 0)
                return BadRequest();

            try
            {
                _balanceService.Charge(balanceId, amount);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [Route("load/{balanceId:int}/{amount:float}")]
        [HttpPost]
        public IHttpActionResult Load(int balanceId, float amount)
        {
            if (amount < 0)
                return BadRequest();

            try
            {
                _balanceService.Load(balanceId, amount);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [Route("transfer/{fromBalanceId:int}/{toBalanceId:int}/{amount:float}")]
        [HttpPost]
        public IHttpActionResult Transfer(int fromBalanceId, int toBalanceId, float amount)
        {
            if (amount < 0 || fromBalanceId == toBalanceId)
                return BadRequest();

            try
            {
                _balanceService.Transfer(fromBalanceId, toBalanceId, amount);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }
    }
}