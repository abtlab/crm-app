using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Crm.Apps.Clients.Deals.Clients;
using Crm.Apps.Clients.Deals.Models;
using Crm.Apps.Clients.Deals.RequestParameters;
using Crm.Apps.Tests.Creator;
using Crm.Common.All.Types.AttributeType;
using Xunit;

namespace Crm.Apps.Tests.Tests.Deals
{
    public class DealAttributesTests
    {
        private readonly ICreate _create;
        private readonly IDealAttributesClient _dealAttributesClient;

        public DealAttributesTests(ICreate create, IDealAttributesClient dealAttributesClient)
        {
            _create = create;
            _dealAttributesClient = dealAttributesClient;
        }

        [Fact]
        public async Task WhenGetTypes_ThenSuccess()
        {
            var types = await _dealAttributesClient.GetTypesAsync();

            Assert.NotEmpty(types);
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var attributeId = (await _create.DealAttribute.BuildAsync()).Id;

            var attribute = await _dealAttributesClient.GetAsync(attributeId);

            Assert.NotNull(attribute);
            Assert.Equal(attributeId, attribute.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var attributeIds = (
                    await Task.WhenAll(
                        _create.DealAttribute
                            .WithKey("Test1")
                            .BuildAsync(),
                        _create.DealAttribute
                            .WithKey("Test2")
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            var attributes = await _dealAttributesClient.GetListAsync(attributeIds);

            Assert.NotEmpty(attributes);
            Assert.Equal(attributeIds.Count, attributes.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            await Task.WhenAll(
                _create.DealAttribute
                    .WithType(AttributeType.Text)
                    .WithKey("Test1")
                    .BuildAsync());
            var filterTypes = new List<AttributeType> {AttributeType.Text};

            var request = new DealAttributeGetPagedListRequestParameter
            {
                Key = "Test1",
                Types = filterTypes,
            };

            var attributes = await _dealAttributesClient.GetPagedListAsync(request);

            var results = attributes
                .Skip(1)
                .Zip(attributes, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(attributes);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var attribute = new DealAttribute
            {
                Type = AttributeType.Text,
                Key = "Test",
                IsDeleted = false
            };

            var createdAttributeId = await _dealAttributesClient.CreateAsync(attribute);

            var createdAttribute = await _dealAttributesClient.GetAsync(createdAttributeId);

            Assert.NotNull(createdAttribute);
            Assert.Equal(createdAttributeId, createdAttribute.Id);
            Assert.Equal(attribute.AccountId, createdAttribute.AccountId);
            Assert.Equal(attribute.Type, createdAttribute.Type);
            Assert.Equal(attribute.Key, createdAttribute.Key);
            Assert.Equal(attribute.IsDeleted, createdAttribute.IsDeleted);
            Assert.True(createdAttribute.CreateDateTime.IsMoreThanMinValue());
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            var attribute = await _create.DealAttribute
                .WithType(AttributeType.Text)
                .WithKey("Test")
                .BuildAsync();

            attribute.Type = AttributeType.Link;
            attribute.Key = "test.com";
            attribute.IsDeleted = true;

            await _dealAttributesClient.UpdateAsync(attribute);

            var updatedAttribute = await _dealAttributesClient.GetAsync(attribute.Id);

            Assert.Equal(attribute.Type, updatedAttribute.Type);
            Assert.Equal(attribute.Key, updatedAttribute.Key);
            Assert.Equal(attribute.IsDeleted, updatedAttribute.IsDeleted);
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var attributeIds = (
                    await Task.WhenAll(
                        _create.DealAttribute
                            .WithKey("Test1")
                            .BuildAsync(),
                        _create.DealAttribute
                            .WithKey("Test2")
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            await _dealAttributesClient.DeleteAsync(attributeIds);

            var attributes = await _dealAttributesClient.GetListAsync(attributeIds);

            Assert.All(attributes, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var attributeIds = (
                    await Task.WhenAll(
                        _create.DealAttribute
                            .WithKey("Test1")
                            .BuildAsync(),
                        _create.DealAttribute
                            .WithKey("Test2")
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            await _dealAttributesClient.RestoreAsync(attributeIds);

            var attributes = await _dealAttributesClient.GetListAsync(attributeIds);

            Assert.All(attributes, x => Assert.False(x.IsDeleted));
        }
    }
}