using AutoMapper;
using GreenCoinService.DataContracts;
using GreenCoinService.DataContracts.Entities;
using GreenCoinService.Exceptions;
using GreenCoinService.Migrations;
using GreenCoinService.Stores.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.Stores
{
    public class CodesStore : ICodesStore
    {
        private readonly IDbContextFactory<GreenCoinServiceDbContext> _contextFactory;
        private readonly IMapper _mapper;

        public CodesStore(IDbContextFactory<GreenCoinServiceDbContext> contextFactory)
        {
            _contextFactory = contextFactory;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Code, CodeEntity>();
                cfg.CreateMap<CodeEntity, Code>();
                cfg.CreateMap<Recyclable, RecyclableEntity>();
                cfg.CreateMap<RecyclableEntity, Recyclable>();
            });
            _mapper = configuration.CreateMapper();
        }

        public async Task<Code> AddCode(Code code)
        {
            using var context = _contextFactory.CreateDbContext();
            code.Id = CreateCodeId();
            var codeEntity = _mapper.Map<CodeEntity>(code);
            context.Codes.Add(codeEntity);
            await context.SaveChangesAsync();
            return code;
        }

        public async Task DeleteCode(string codeId)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                var codeEntity = new CodeEntity
                {
                    Id = codeId
                };
                context.Codes.Remove(codeEntity);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new StorageErrorException($"Code entity with Id {codeId} was not found", 404);
            }
        }

        public async Task<Code> GetBusinessCode(string businessId)
        {
            using var context = _contextFactory.CreateDbContext();
            var codeEntity = await context.Codes.Where(code => code.BusinessId == businessId).FirstOrDefaultAsync();
            if (codeEntity == null)
            {
                throw new StorageErrorException($"Code entity with Business Id {businessId} was not found", 404);
            }
            var code = _mapper.Map<Code>(codeEntity);
            return code;
        }

        public async Task<Code> GetCode(string codeId)
        {
            using var context = _contextFactory.CreateDbContext();
            var codeEntity = await context.Codes
                .Include(code => code.Recyclable)
                .Where(code => code.Id == codeId)
                .FirstOrDefaultAsync();
            if (codeEntity == null)
            {
                throw new StorageErrorException($"Code entity with Id {codeId} was not found", 404);
            }
            var code = _mapper.Map<Code>(codeEntity);
            return code;
        }

        public async Task<List<Code>> GetWasteCodes(string userId)
        {
            using var context = _contextFactory.CreateDbContext();
            var codeEntities = await context.Codes
                .Include(code => code.Recyclable)
                .Where(code => code.UserId == userId)
                .ToListAsync();
            var codes = _mapper.Map<List<Code>>(codeEntities);
            return codes;
        }

        public async Task UpdateCode(Code code)
        {
            using var context = _contextFactory.CreateDbContext();
            var codeEntity = await context.Codes
                .Where(e => e.Id == code.Id)
                .FirstOrDefaultAsync();
            if (codeEntity == null)
            {
                throw new StorageErrorException($"Code entity with Id {code.Id} was not found", 404);
            }
            _mapper.Map(code, codeEntity);
            await context.SaveChangesAsync();
        }

        private string CreateCodeId()
        {
            return Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString();
        }
    }
}
