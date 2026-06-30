using DataAccess.Data;
using DataAccess.Repositories.StoreRepo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {

        private readonly IStoreRepository _storeService;

        public StoresController(IStoreRepository storeService)
        {
            _storeService = storeService;
        }
        //get all stores
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stores = await _storeService.GetAllAsync();
            return Ok(stores);
        }
    }
}
