using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EsApi.Tests;

public sealed class AttachmentEndpointTests
{
    [Fact]
    public async Task AttachmentListingOmitsStoragePathsAndDeletionLeavesOtherEntitiesUntouched()
    {
        using var app = new ApiFactory();
        using var client = app.CreateHttpsClient();
        await app.AuthenticateAsync(client, LoginUser.AdministratorRole);

        var firstId = await CreateAttachmentAsync(client, 42, "first.pdf");
        var secondId = await CreateAttachmentAsync(client, 43, "second.pdf");

        using var listing = await client.GetAsync("/api/v1/attachments/Invoice/42");
        Assert.Equal(HttpStatusCode.OK, listing.StatusCode);
        var attachments = await listing.Content.ReadFromJsonAsync<JsonElement>();
        var attachment = Assert.Single(attachments.EnumerateArray());
        Assert.Equal(firstId, attachment.GetProperty("idattachment").GetInt32());
        Assert.Equal("first.pdf", attachment.GetProperty("fileName").GetString());
        Assert.False(attachment.TryGetProperty("filePath", out _));

        using var deleted = await client.DeleteAsync($"/api/v1/attachments/{firstId}");
        using var missing = await client.DeleteAsync($"/api/v1/attachments/{firstId}");
        Assert.Equal(HttpStatusCode.NoContent, deleted.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, missing.StatusCode);

        using var remaining = await client.GetAsync("/api/v1/attachments/Invoice/43");
        Assert.Equal(HttpStatusCode.OK, remaining.StatusCode);
        var remainingAttachments = await remaining.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(secondId, Assert.Single(remainingAttachments.EnumerateArray())
            .GetProperty("idattachment").GetInt32());
    }

    [Theory]
    [InlineData(0, "invoice.pdf", "/internal/invoice.pdf")]
    [InlineData(42, "", "/internal/invoice.pdf")]
    [InlineData(42, "invoice.pdf", "")]
    public async Task InvalidAttachmentMetadataIsRejectedWithoutSaving(
        int entityId, string fileName, string filePath)
    {
        using var app = new ApiFactory();
        using var client = app.CreateHttpsClient();
        await app.AuthenticateAsync(client, LoginUser.AdministratorRole);
        using var response = await client.PostAsJsonAsync("/api/v1/attachments",
            new { EntityType = "Invoice", EntityId = entityId, FileName = fileName, FilePath = filePath });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        using var scope = app.Services.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        Assert.False(await database.Attachments.AnyAsync());
    }

    private static async Task<int> CreateAttachmentAsync(HttpClient client, int entityId, string fileName)
    {
        using var response = await client.PostAsJsonAsync("/api/v1/attachments", new
        {
            EntityType = "Invoice",
            EntityId = entityId,
            FileName = fileName,
            FilePath = $"/internal/{fileName}"
        });
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var attachment = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.False(attachment.TryGetProperty("filePath", out _));
        return attachment.GetProperty("idattachment").GetInt32();
    }
}
