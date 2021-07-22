using AutoMapper;
using Dapper;
using HumanityService.DataContracts.CompositeDesignPattern;
using HumanityService.DataContracts.Requests;
using HumanityService.DataContracts.Results;
using HumanityService.Exceptions;
using HumanityService.Stores.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HumanityService.Stores
{
    public class TransactionStore : ITransactionStore
    {
        private readonly IConnectionFactory _sqlConnectionFactory;
        private readonly ILocationStore _locationStore;
        private readonly IMapper _mapper;

        public TransactionStore(IConnectionFactory sqlConnectionFactory, ILocationStore locationStore)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _locationStore = locationStore; 

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Campaign, CampaignEntity>();
                cfg.CreateMap<CampaignEntity, Campaign>();
                cfg.CreateMap<Contribution, ContributionEntity>();
                cfg.CreateMap<ContributionEntity, Contribution>();
                cfg.CreateMap<Process, ProcessEntity>();
                cfg.CreateMap<ProcessEntity, Process>();
                cfg.CreateMap<DeliveryDemand, DeliveryDemandEntity>();
                cfg.CreateMap<DeliveryDemandEntity, DeliveryDemand>();
            });
            _mapper = configuration.CreateMapper();
        }

        private static readonly string[] CampaignsTableColumns =
        {
            nameof(CampaignEntity.Id),
            nameof(CampaignEntity.Name),
            nameof(CampaignEntity.NgoName),
            nameof(CampaignEntity.Username),
            nameof(CampaignEntity.Status),
            nameof(CampaignEntity.Type),
            nameof(CampaignEntity.Category),
            nameof(CampaignEntity.CompletedCount),
            nameof(CampaignEntity.TimeCreated),
            nameof(CampaignEntity.TimeCompleted),
            nameof(CampaignEntity.Description)
        };

        private static readonly string[] ContributionsTableColumns =
        {
            nameof(ContributionEntity.Id),
            nameof(ContributionEntity.ProcessId),
            nameof(ContributionEntity.DeliveryDemandId),
            nameof(ContributionEntity.DeliveryCode),
            nameof(ContributionEntity.Type),
            nameof(ContributionEntity.Username),
            nameof(ContributionEntity.Status),
            nameof(ContributionEntity.TimeWindowStart),
            nameof(ContributionEntity.TimeWindowEnd),
            nameof(ContributionEntity.TimeCreated),
            nameof(ContributionEntity.TimeCompleted),
            nameof(ContributionEntity.OtherInfo)
        };

        private static readonly string[] ProcessesTableColumns =
        {
            nameof(ProcessEntity.Id),
            nameof(ProcessEntity.CampaignId),
            nameof(ProcessEntity.Status),
            nameof(ProcessEntity.TimeCreated),
            nameof(ProcessEntity.TimePickedUp),
            nameof(ProcessEntity.TimeCompleted),
            nameof(ProcessEntity.DeliveryCode)
        };

        private static readonly string[] DeliveryDemandsTableColumns =
        {
            nameof(DeliveryDemandEntity.Id),
            nameof(DeliveryDemandEntity.CampaignName),
            nameof(DeliveryDemandEntity.ProcessId),
            nameof(DeliveryDemandEntity.PickupUsername),
            nameof(DeliveryDemandEntity.DestinationUsername),
            nameof(DeliveryDemandEntity.TimeWindowStart),
            nameof(DeliveryDemandEntity.TimeWindowEnd),
            nameof(DeliveryDemandEntity.Status),
            nameof(DeliveryDemandEntity.TimeCreated),
            nameof(DeliveryDemandEntity.TimeCompleted),
            nameof(DeliveryDemandEntity.OtherInfo)
        };

        public async Task<string> AddCampaign(Campaign campaign)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();
            var sql = new QueryBuilder().InsertInto(CampaignEntity.TableName, CampaignsTableColumns).Build();
            var campaignEntity = ToCampaignEntity(campaign);
            await connection.ExecuteAsync(sql, campaignEntity);
            return campaign.Id;
        }

        public async Task<string> AddDeliveryDemand(DeliveryDemand deliveryDemand)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();
            var sql = new QueryBuilder().InsertInto(DeliveryDemandEntity.TableName, DeliveryDemandsTableColumns).Build();
            var deliveryDemandEntity = ToDeliveryDemandEntity(deliveryDemand);
            await connection.ExecuteAsync(sql, deliveryDemandEntity);
            return deliveryDemand.Id;
        }

        public async Task<string> AddProcess(Process process)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();
            var sql = new QueryBuilder().InsertInto(ProcessEntity.TableName, ProcessesTableColumns).Build();
            var processEntity = ToProcessEntity(process);
            await connection.ExecuteAsync(sql, processEntity);
            return process.Id;
        }

        public async Task<string> AddContribution(Contribution contribution)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();
            var sql = new QueryBuilder().InsertInto(ContributionEntity.TableName, ContributionsTableColumns).Build();
            var contributionEntity = ToContributionEntity(contribution);
            await connection.ExecuteAsync(sql, contribution);
            return contribution.Id;
        }

        public async Task DeleteCampaign(string campaignId)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();

            var sql = new QueryBuilder().DeleteFrom(CampaignEntity.TableName)
                .Where("Id = @CampaignId").Build();

            await connection.ExecuteAsync(sql, new
            {
                CampaignId = campaignId
            });
        }

        public async Task DeleteContribution(string contributionId)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();

            var sql = new QueryBuilder().DeleteFrom(ContributionEntity.TableName)
                .Where("Id = @ContributionId").Build();

            await connection.ExecuteAsync(sql, new
            {
                ContributionId = contributionId
            });
        }

        public async Task DeleteDeliveryDemand(string deliveryDemandId)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();

            var sql = new QueryBuilder().DeleteFrom(DeliveryDemandEntity.TableName)
                .Where("Id = @DeliveryDemandId").Build();

            await connection.ExecuteAsync(sql, new
            {
                DeliveryDemandId = deliveryDemandId
            });
        }

        public async Task DeleteProcess(string processId)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();

            var sql = new QueryBuilder().DeleteFrom(ProcessEntity.TableName)
                .Where("Id = @ProcessId").Build();

            await connection.ExecuteAsync(sql, new
            {
                ProcessId = processId
            });
        }

        public async Task UpdateCampaign(Campaign campaign)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();

            var sql = new QueryBuilder().Update(CampaignEntity.TableName, CampaignsTableColumns)
                .Where($"Id = @Id").Build();

            int rowsAffected = await connection.ExecuteAsync(sql, campaign);
            if (rowsAffected == 0)
            {
                throw new StorageErrorException($"Campaign entity with Id {campaign.Id} was not found", 404);
            }
        }

        public async Task UpdateContribution(Contribution contribution)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();

            var sql = new QueryBuilder().Update(ContributionEntity.TableName, ContributionsTableColumns)
                .Where($"Id = @Id").Build();


            int rowsAffected = await connection.ExecuteAsync(sql, contribution);
            if (rowsAffected == 0)
            {
                throw new StorageErrorException($"Contribution entity with Id {contribution.Id} was not found", 404);
            }
        }

        public async Task UpdateDeliveryDemand(DeliveryDemand deliveryDemand)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();

            var sql = new QueryBuilder().Update(DeliveryDemandEntity.TableName, DeliveryDemandsTableColumns)
                .Where($"Id = @Id").Build();

            int rowsAffected = await connection.ExecuteAsync(sql, deliveryDemand);
            if (rowsAffected == 0)
            {
                throw new StorageErrorException($"DeliveryDemand entity with Id {deliveryDemand.Id} was not found", 404);
            }
        }

        public async Task UpdateProcess(Process process)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();

            var sql = new QueryBuilder().Update(ProcessEntity.TableName, ProcessesTableColumns)
                .Where($"Id = @Id").Build();


            int rowsAffected = await connection.ExecuteAsync(sql, process);
            if (rowsAffected == 0)
            {
                throw new StorageErrorException($"Process entity with Id {process.Id} was not found", 404);
            }
        }

        public async Task<Campaign> GetCampaign(string campaignId)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();

            var sql = new QueryBuilder()
                .SelectColumns(CampaignEntity.TableName, CampaignsTableColumns)
                .Where("Id = @Id")
                .Build();

            var campaignEntity = await connection.QueryFirstOrDefaultAsync<CampaignEntity>(sql, new { Id = campaignId });
            if (campaignEntity == null)
            {
                throw new StorageErrorException($"Campaign entity with Id {campaignId} was not found", 404);
            }
            var location = await _locationStore.GetLocation(campaignEntity.Username);
            return ToCampaign(campaignEntity, location);
        }

        public async Task<GetCampaignsResult> GetCampaigns(GetCampaignsRequest request)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();

            var sql = new QueryBuilder()
                .SelectColumns(CampaignEntity.TableName, CampaignsTableColumns)
                .Where("(@NgoName IS NULL OR NgoName =  @NgoName)")
                .And("(@Username IS NULL OR Username =  @Username)")
                .And("(@Type IS NULL OR Type =  @Type)")
                .And("(@Category IS NULL OR Category =  @Category)")
                .And("(@Status IS NULL OR Status =  @Status)")
                .OrderBy("Name", "ASC")
                .Build();

            var campaignEntities = (await connection.QueryAsync<CampaignEntity>(sql, request)).ToList();
            List<Campaign> campaigns = new List<Campaign>();
            foreach(var entity in campaignEntities)
            {
                var location = await _locationStore.GetLocation(entity.Username);
                var campaign = ToCampaign(entity, location);
                campaigns.Add(campaign);
            }
            
            return new GetCampaignsResult
            {
                Campaigns = campaigns
            };
        }

        public async Task<Contribution> GetContribution(string contributionId)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();

            var sql = new QueryBuilder()
                .SelectColumns(ContributionEntity.TableName, ContributionsTableColumns)
                .Where("Id = @Id")
                .Build();

            var contributionEntity = await connection.QueryFirstOrDefaultAsync<ContributionEntity>(sql, new { Id = contributionId });

            if (contributionEntity == null)
            {
                throw new StorageErrorException($"Contribution entity with Id {contributionId} was not found", 404);
            }

            var location = await _locationStore.GetLocation(contributionEntity.Username);

            return ToContribution(contributionEntity, location);
        }

        public async Task<GetContributionsResult> GetContributions(GetContributionsRequest request)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();

            var sql = new QueryBuilder()
                .SelectColumns(ContributionEntity.TableName, ContributionsTableColumns)
                .Where("(@Username IS NULL OR Username =  @Username)")
                .And("(@ProcessId IS NULL OR ProcessId =  @ProcessId)")
                .And("(@DeliveryDemandId IS NULL OR DeliveryDemandId =  @DeliveryDemandId)")
                .And("(@Type IS NULL OR Type =  @Type)")
                .And("(@Status IS NULL OR Status =  @Status)")
                .OrderBy("TimeCreated", "DESC")
                .Build();

            var campaignEntities = (await connection.QueryAsync<ContributionEntity>(sql, request)).ToList();

            List<Contribution> contributions = new List<Contribution>();
            foreach (var entity in campaignEntities)
            {
                var location = await _locationStore.GetLocation(entity.Username);
                var contribution = ToContribution(entity, location);
                contributions.Add(contribution);
            }

            return new GetContributionsResult
            {
                Contributions = contributions
            };
        }

        public async Task<DeliveryDemand> GetDeliveryDemand(string deliveryDemandId)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();

            var sql = new QueryBuilder()
                .SelectColumns(DeliveryDemandEntity.TableName, DeliveryDemandsTableColumns)
                .Where("Id = @Id")
                .Build();

            var deliveryDemandEntity = await connection.QueryFirstOrDefaultAsync<DeliveryDemandEntity>(sql, new { Id = deliveryDemandId });
            if (deliveryDemandEntity == null)
            {
                throw new StorageErrorException($"DeliveryDemand entity with Id {deliveryDemandId} was not found", 404);
            }
            var pickupLocation = await _locationStore.GetLocation(deliveryDemandEntity.PickupUsername);
            var destinationLocation = await _locationStore.GetLocation(deliveryDemandEntity.DestinationUsername);
            return ToDeliveryDemand(deliveryDemandEntity, pickupLocation, destinationLocation);
        }

        public async Task<GetDeliveryDemandsResult> GetDeliveryDemands(GetDeliveryDemandsRequest request)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();

            var sql = new QueryBuilder()
                .SelectColumns(DeliveryDemandEntity.TableName, DeliveryDemandsTableColumns)
                .Where("(@CampaignName IS NULL OR CampaignName =  @CampaignName)")
                .And("(@ProcessId IS NULL OR ProcessId =  @ProcessId)")
                .And("(@PickupUsername IS NULL OR PickupUsername =  @PickupUsername)")
                .And("(@DestinationUsername IS NULL OR DestinationUsername =  @DestinationUsername)")
                .And("(@Status IS NULL OR Status =  @Status)")
                .OrderBy("TimeCreated", "DESC")
                .Build();

            var deliveryDemandEntities = (await connection.QueryAsync<DeliveryDemandEntity>(sql, request)).ToList();

            List<DeliveryDemand> deliveryDemands = new List<DeliveryDemand>();
            foreach (var entity in deliveryDemandEntities)
            {
                var pickupLocation = await _locationStore.GetLocation(entity.PickupUsername);
                var destinationLocation = await _locationStore.GetLocation(entity.DestinationUsername);
                var deliveryDemand = ToDeliveryDemand(entity, pickupLocation, destinationLocation);
                deliveryDemands.Add(deliveryDemand);
            }

            return new GetDeliveryDemandsResult
            {
                DeliveryDemands = deliveryDemands
            };
        }

        public async Task<Process> GetProcess(string processId)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();

            var sql = new QueryBuilder()
                .SelectColumns(ProcessEntity.TableName, ProcessesTableColumns)
                .Where("Id = @Id")
                .Build();

            var processEntity = await connection.QueryFirstOrDefaultAsync<ProcessEntity>(sql, new { Id = processId });
            var process = ToProcess(processEntity);

            if (process == null)
            {
                throw new StorageErrorException($"Process entity with Id {processId} was not found", 404);
            }

            return process;
        }

        public async Task<GetProcessesResult> GetProcesses(string campaignId)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();

            var sql = new QueryBuilder()
                .SelectColumns(ProcessEntity.TableName, ProcessesTableColumns)
                .Where("(@CampaignId IS NULL OR CampaignId =  @CampaignId)")
                .OrderBy("TimeCreated", "DESC")
                .Build();

            var processEntities = (await connection.QueryAsync<ProcessEntity>(sql, new { CampaignId = campaignId})).ToList();

            List<Process> processes = new List<Process>();
            foreach (var entity in processEntities)
            {
                var process = ToProcess(entity);
                processes.Add(process);
            }

            return new GetProcessesResult
            {
                Processes = processes
            };
        }

        public async Task<bool> EntityExists(string tableName, string Id)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();

            var sql = new QueryBuilder()
                .Count(tableName)
                .Where("Id = @Id")
                .Build();

            int count = await connection.QuerySingleAsync<int>(sql, new { Id });
            return count > 0;
        }

        private CampaignEntity ToCampaignEntity(Campaign campaign)
        {
            var entity = _mapper.Map<CampaignEntity>(campaign);
            return entity;
        }

        private Campaign ToCampaign(CampaignEntity entity, Location location)
        {
            if (entity == null)
            {
                return null;
            }

            var campaign = _mapper.Map<Campaign>(entity);
            campaign.Location = location;
            return campaign;
        }

        private ContributionEntity ToContributionEntity(Contribution contribution)
        {
            var entity = _mapper.Map<ContributionEntity>(contribution);
            return entity;
        }

        private Contribution ToContribution(ContributionEntity entity, Location location)
        {
            if (entity == null)
            {
                return null;
            }

            var contribution = _mapper.Map<Contribution>(entity);
            contribution.Location = location;
            return contribution;
        }
        private ProcessEntity ToProcessEntity(Process process)
        {
            var entity = _mapper.Map<ProcessEntity>(process);
            return entity;
        }

        private Process ToProcess(ProcessEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            var process = _mapper.Map<Process>(entity);
            return process;
        }
        private DeliveryDemandEntity ToDeliveryDemandEntity(DeliveryDemand deliveryDemand)
        {
            var entity = _mapper.Map<DeliveryDemandEntity>(deliveryDemand);
            return entity;
        }

        private DeliveryDemand ToDeliveryDemand(DeliveryDemandEntity entity, Location pickupLocation, Location destinationLocation)
        {
            if (entity == null)
            {
                return null;
            }

            var deliveryDemand = _mapper.Map<DeliveryDemand>(entity);
            deliveryDemand.PickupLocation = pickupLocation;
            deliveryDemand.DestinationLocation = destinationLocation;
            return deliveryDemand;
        }     
    }
}
