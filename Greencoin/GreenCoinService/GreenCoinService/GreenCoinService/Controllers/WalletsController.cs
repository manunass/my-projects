using GreenCoinService.DataContracts;
using GreenCoinService.DataContracts.Requests;
using GreenCoinService.DataContracts.Responses;
using GreenCoinService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GreenCoinService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletsService _walletsService;

        public WalletsController(IWalletsService walletsService)
        {
            _walletsService = walletsService;
        }
        /// <summary>
        /// Bulk-update multiple wallets at the same time. Can be used by municipality admins to increment the balance of multiple users at the same.
        /// </summary>
        /// <param name="municipalityId"></param>
        /// <param name="updateWalletsRequest"></param>
        /// <returns></returns>
        [HttpPatch("bulk-update/{municipalityId}")]
        public async Task<IActionResult> UpdateWallets(string municipalityId, [FromBody] UpdateWalletsRequest updateWalletsRequest)
        {
            foreach(var updateWallet in updateWalletsRequest.UpdateWallets)
            {
                await _walletsService.UpdateWallet(municipalityId, updateWallet.WalletId, updateWallet.NetAmount, updateWalletsRequest.Description);
            }
            return Ok();
        }

        /// <summary>
        /// Pay another user. Can be used by users to pay businesses for goods or services.
        /// </summary>
        /// <param name="payRequest"></param>
        /// <returns></returns>
        [HttpPatch("pay")]
        public async Task<IActionResult> Pay([FromBody] PayRequest payRequest)
        {
            await _walletsService.Pay(payRequest.FromWalletId, payRequest.ToWalletId, payRequest.Amount, payRequest.Description);
            return Ok();
        }

        /// <summary>
        /// Fetch a wallet.
        /// </summary>
        /// <param name="walletId"></param>
        /// <returns></returns>
        [HttpGet("{walletId}")]
        [ProducesResponseType(typeof(Wallet), 200)]
        public async Task<IActionResult> GetWallet(string walletId)
        {
            var wallet = await _walletsService.GetWallet(walletId);
            return Ok(wallet);
        }

        /// <summary>
        /// Fecth all the transactions related to a given wallet.
        /// </summary>
        /// <param name="walletId"></param>
        /// <returns></returns>
        [HttpGet("{walletId}/transactions")]
        [ProducesResponseType(typeof(GetTransactionsResponse), 200)]
        public async Task<IActionResult> GetTransactions(string walletId)
        {
            var transactions = await _walletsService.GetTransactions(walletId);
            var getTransactionsResponse = new GetTransactionsResponse
            {
                Transactions = transactions
            };
            return Ok(getTransactionsResponse);
        }
        /// <summary>
        /// Fetch a transaction.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        [HttpGet("{walletId}/transactions/{transactionId}")]
        [ProducesResponseType(typeof(Transaction), 200)]
        public async Task<IActionResult> GetTransaction(string transactionId)
        {
            var transaction = await _walletsService.GetTransaction(transactionId);
            return Ok(transaction);
        }
    }
}
